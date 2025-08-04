using Bootcamp.BusinessLayer.Abstract;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class GeminiManager : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

        public GeminiManager(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<string> SummarizeVideoAsync(string videoUrl, string videoTitle, string videoDescription)
        {
            try
            {
                // Description alanındaki konu başlıklarına göre ders anlatımı oluştur
                return await CreateLessonFromDescription(videoTitle, videoDescription);
            }
            catch (Exception ex)
            {
                return $"Hata oluştu: {ex.Message}";
            }
        }

        private async Task<string> CreateLessonFromDescription(string videoTitle, string videoDescription)
        {
            try
            {
                                 string prompt = $@"
Bu eğitim videosunun konu başlıklarını analiz et ve her bir alt konu için ayrı ayrı açıklama oluştur:

Video Başlığı: {videoTitle}
Konu Başlıkları (Description): {videoDescription}

Bu konu başlıklarına dayalı olarak, her bir alt konu için ayrı ayrı açıklama hazırla:

Format şu şekilde olmalı:

📚 [Alt Konu Başlığı]

Bu konu hakkında detaylı açıklama, örnekler ve pratik bilgiler. normal açıklama halinde maddelere ayırmadan yaz:

🎯 Ana Kavram: [Temel kavram açıklaması - 2-3 cümle]

💡 Önemli Nokta: [Önemli bilgi - 2-3 cümle]

🔬 Pratik Örnek: [Günlük hayattan örnek - 2-3 cümle]

⚡ Püf Noktası: [Önemli ipucu - 2-3 cümle]

🌟 Ek Bilgi: [Ek detaylar ve açıklamalar - 2-3 cümle]

Her alt konu için bu formatı kullan ve şu kurallara uy:

- Her alt konu için ayrı başlık ve açıklama yaz
- Açıklamada konunun ne olduğunu, neden önemli olduğunu ve nasıl uygulandığını anlat
- Pratik örnekler ve günlük hayattan uygulamalar ekle
- Detaylı ve kapsamlı açıklamalar yap
- Her açıklama 4-5 madde olsun
- Emoji'ler kullanarak görsel hale getir
- Her madde için 2-3 cümle kullan

ÖNEMLİ KURALLAR:
1. Hiçbir markdown işareti kullanma (yıldız *, tire -, kare #, alt çizgi _)
2. Sadece normal metin olarak yaz
3. Başlıklar için normal büyük/küçük harf kullan
4. Liste öğeleri için sadece tire (-) kullan
5. Vurgu için sadece büyük harfler kullan
6. Tüm metni düz metin olarak yaz
7. Türkçe karakterleri (ç, ğ, ı, ö, ş, ü) doğru kullan
8. Emoji'leri koru ve görsel olarak çekici yap
9. Her alt konu için ayrı başlık ve açıklama formatı kullan
10. Detaylı ve kapsamlı açıklamalar yap

Not: Her alt konu için emoji ile başlayan başlık ve detaylı maddeler halinde açıklama yaz.
";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    
                    if (responseObject.TryGetProperty("candidates", out var candidates) && 
                        candidates.GetArrayLength() > 0)
                    {
                        var firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                            contentElement.TryGetProperty("parts", out var parts) &&
                            parts.GetArrayLength() > 0)
                        {
                            var firstPart = parts[0];
                            if (firstPart.TryGetProperty("text", out var textElement))
                            {
                                return textElement.GetString();
                            }
                        }
                    }
                    
                    return "Ders anlatımı oluşturulamadı. Lütfen daha sonra tekrar deneyin.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Hata oluştu: {ex.Message}";
            }
        }

        public async Task<string> GenerateVideoSummaryAsync(string videoContent)
        {
            try
            {
                                 string prompt = $@"
Bu video içeriğini analiz et ve özetle:

{videoContent}

Lütfen aşağıdaki formatta özetle:

📝 Ana Konular:
- [Ana konuları listele]

✅ Önemli Noktalar:
- [Önemli noktaları listele]

💡 Pratik İpuçları:
- [Pratik ipuçlarını listele]

🔧 Adım Adım Yöntemler:
- [Adım adım yöntemleri listele]

🎯 Öğrenme Hedefleri:
- [Öğrenme hedeflerini listele]

🔍 Detaylı Özet:
[Video içeriğinin detaylı özeti]

ÖNEMLİ KURALLAR:
1. Hiçbir markdown işareti kullanma (yıldız *, tire -, kare #, alt çizgi _)
2. Sadece normal metin olarak yaz
3. Başlıklar için normal büyük/küçük harf kullan, yıldız veya kare işareti kullanma
4. Liste öğeleri için sadece tire (-) kullan, yıldız (*) kullanma
5. Vurgu için sadece büyük harfler kullan, yıldız veya alt çizgi kullanma
6. Tüm metni düz metin olarak yaz, hiçbir formatlamaya ihtiyaç yok
7. Türkçe karakterleri (ç, ğ, ı, ö, ş, ü) doğru kullan
8. Emoji'leri koru ve görsel olarak çekici yap

Not: Sadece normal metin olarak yaz, hiçbir markdown formatlaması kullanma.
";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    
                    if (responseObject.TryGetProperty("candidates", out var candidates) && 
                        candidates.GetArrayLength() > 0)
                    {
                        var firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                            contentElement.TryGetProperty("parts", out var parts) &&
                            parts.GetArrayLength() > 0)
                        {
                            var firstPart = parts[0];
                            if (firstPart.TryGetProperty("text", out var textElement))
                            {
                                return textElement.GetString();
                            }
                        }
                    }
                    
                    return "Video özeti oluşturulamadı. Lütfen daha sonra tekrar deneyin.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Hata oluştu: {ex.Message}";
            }
        }

        public async Task<string> AskQuestionAsync(string question, string videoTitle, string videoDescription)
        {
            try
            {
                                 string prompt = $@"
Sen bir eğitim asistanısın. Öğrencinin sorduğu soruyu, verilen ders konularına göre KISA ve ÖZ bir şekilde cevapla.

Ders Başlığı: {videoTitle}
Ders Konuları (Description): {videoDescription}

Öğrencinin Sorusu: {question}

Lütfen aşağıdaki kurallara göre KISA cevap ver:

💡 Kısa Cevap Formatı:
- Soruyu anladığını göster (1 cümle)
- Ana kavramı açıkla (2-3 cümle)
- Önemli püf noktasını ver (1 cümle)
- Pratik örnek ver (1 cümle)

🎯 Önemli Kurallar:
- Maksimum 5-6 cümle kullan
- Çok kısa ve öz ol
- Sadece en önemli bilgiyi ver
- Chat mesajı formatında yaz
- Gereksiz detay verme

ÖNEMLİ KURALLAR:
1. Hiçbir markdown işareti kullanma (yıldız *, tire -, kare #, alt çizgi _)
2. Sadece normal metin olarak yaz
3. Başlıklar için normal büyük/küçük harf kullan
4. Liste öğeleri için sadece tire (-) kullan
5. Vurgu için sadece büyük harfler kullan
6. Tüm metni düz metin olarak yaz
7. Türkçe karakterleri (ç, ğ, ı, ö, ş, ü) doğru kullan
8. Emoji'leri koru ve görsel olarak çekici yap
9. Öğrenci dostu bir dil kullan
10. ÇOK KISA ve ÖZ cevap ver (maksimum 5-6 cümle)

Not: Chat mesajı formatında, çok kısa ve öz cevap ver. Uzun açıklamalar yapma.
";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    
                    if (responseObject.TryGetProperty("candidates", out var candidates) && 
                        candidates.GetArrayLength() > 0)
                    {
                        var firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                            contentElement.TryGetProperty("parts", out var parts) &&
                            parts.GetArrayLength() > 0)
                        {
                            var firstPart = parts[0];
                            if (firstPart.TryGetProperty("text", out var textElement))
                            {
                                return textElement.GetString();
                            }
                        }
                    }
                    
                    return "Sorunuz cevaplanamadı. Lütfen daha sonra tekrar deneyin.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Hata oluştu: {ex.Message}";
            }
        }
    }
} 