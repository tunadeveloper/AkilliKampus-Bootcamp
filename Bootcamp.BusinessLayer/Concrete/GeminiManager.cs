using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class GeminiManager : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _defaultApiKey;
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeminiManager(HttpClient httpClient, string defaultApiKey, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _defaultApiKey = defaultApiKey;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<string> GetUserApiKeyAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                return !string.IsNullOrEmpty(user?.GeminiApiKey) ? user.GeminiApiKey : _defaultApiKey;
            }
            catch
            {
                return _defaultApiKey;
            }
        }

        public async Task<string> SummarizeVideoAsync(string videoUrl, string videoTitle, string videoDescription)
        {
            try
            {
                return await CreateLessonFromDescription(videoTitle, videoDescription);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("RESOURCE_EXHAUSTED"))
                {
                    return "🤖 API Kullanım Limiti Aşıldı\n\n" +
                           "Gemini AI'nin günlük ücretsiz kullanım limitini aştık. Bu durumda size manuel bir özet sunuyoruz:\n\n" +
                           $"📚 {videoTitle}\n\n" +
                           $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                           "💡 Bu konuları çalışırken şu noktalara dikkat edin:\n" +
                           "- Her konuyu adım adım takip edin\n" +
                           "- Önemli kavramları not alın\n" +
                           "- Pratik örnekler üzerinde çalışın\n" +
                           "- Konuları günlük hayatla ilişkilendirin\n\n" +
                           "🔄 Daha detaylı AI özeti için yarın tekrar deneyebilirsiniz.";
                }
                
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

                var userApiKey = await GetUserApiKeyAsync();
                
                Console.WriteLine($"API Key kullanılıyor: {userApiKey.Substring(0, Math.Min(10, userApiKey.Length))}...");
                Console.WriteLine($"API URL: {_apiUrl}");
                
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={userApiKey}", content);
                
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
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || 
                        errorContent.Contains("429") || 
                        errorContent.Contains("Too Many Requests") || 
                        errorContent.Contains("RESOURCE_EXHAUSTED") ||
                        errorContent.Contains("quota"))
                    {
                        return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                               "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                               "📅 Ne Yapabilirsiniz:\n" +
                               "- Yarın tekrar deneyebilirsiniz\n" +
                               "- Şimdilik manuel özet kullanabilirsiniz\n" +
                               "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                               "💡 Manuel Özet:\n" +
                               $"📚 {videoTitle}\n\n" +
                               $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                               "✅ Çalışma İpuçları:\n" +
                               "- Her konuyu adım adım takip edin\n" +
                               "- Önemli kavramları not alın\n" +
                               "- Pratik örnekler üzerinde çalışın\n" +
                               "- Konuları günlük hayatla ilişkilendirin\n\n" +
                               "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                    }
                    
                     if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable || 
                         errorContent.Contains("503") || 
                         errorContent.Contains("overloaded") ||
                         errorContent.Contains("UNAVAILABLE"))
                     {
                         return "🤖 AI Servisi Meşgul\n\n" +
                                "Gemini AI şu anda çok yoğun. Lütfen birkaç dakika sonra tekrar deneyin.\n\n" +
                                "📅 Ne Yapabilirsiniz:\n" +
                                "- Birkaç dakika bekleyip tekrar deneyin\n" +
                                "- Şimdilik manuel özet kullanabilirsiniz\n" +
                                "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                                "💡 Manuel Özet:\n" +
                                $"📚 {videoTitle}\n\n" +
                                $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                                "✅ Çalışma İpuçları:\n" +
                                "- Her konuyu adım adım takip edin\n" +
                                "- Önemli kavramları not alın\n" +
                                "- Pratik örnekler üzerinde çalışın\n" +
                                "- Konuları günlük hayatla ilişkilendirin\n\n" +
                                "🔄 Birkaç dakika sonra tekrar deneyin!";
                     }
                     
                     if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || 
                         errorContent.Contains("400") || 
                         errorContent.Contains("API key not valid") ||
                         errorContent.Contains("INVALID_ARGUMENT") ||
                         errorContent.Contains("API_KEY_INVALID"))
                     {
                         return "🔑 API Anahtarınızda Sorun Var\n\n" +
                                "Girdiğiniz API anahtarı geçersiz veya yanlış format.\n\n" +
                                "🔍 Kontrol Edilecekler:\n" +
                                "- API anahtarınızın doğru kopyalandığından emin olun\n" +
                                "- Gemini AI Studio'dan aldığınız anahtarı kullandığınızdan emin olun\n" +
                                "- Anahtarın tam ve eksiksiz olduğunu kontrol edin\n\n" +
                                "💡 Çözüm:\n" +
                                "1. Profil sayfanıza gidin\n" +
                                "2. API anahtarınızı tekrar kontrol edin\n" +
                                "3. Gerekirse yeni bir anahtar alın\n" +
                                "4. Doğru anahtarı kaydedin\n\n" +
                                "📚 Manuel Özet:\n" +
                                $"📚 {videoTitle}\n\n" +
                                $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                                "✅ Çalışma İpuçları:\n" +
                                "- Her konuyu adım adım takip edin\n" +
                                "- Önemli kavramları not alın\n" +
                                "- Pratik örnekler üzerinde çalışın\n" +
                                "- Konuları günlük hayatla ilişkilendirin\n\n" +
                                "🔄 API anahtarınızı düzelttikten sonra tekrar deneyin!";
                     }
                     
                     return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout") || ex.Message.Contains("canceled"))
                {
                    return "⏰ API Yanıt Süresi Aşıldı\n\n" +
                           "API isteği zaman aşımına uğradı. Bu genellikle şu sebeplerden olur:\n\n" +
                           "🔍 Olası Sebepler:\n" +
                           "- API anahtarı yanlış format (Google Cloud API yerine Gemini AI Studio API kullanın)\n" +
                           "- Ağ bağlantısı sorunu\n" +
                           "- API servisi yoğun\n\n" +
                           "💡 Çözüm Önerileri:\n" +
                           "- API anahtarınızı kontrol edin (AI- ile başlamalı)\n" +
                           "- İnternet bağlantınızı kontrol edin\n" +
                           "- Birkaç dakika sonra tekrar deneyin\n\n" +
                           "📚 Manuel Özet:\n" +
                           $"📚 {videoTitle}\n\n" +
                           $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                           "✅ Çalışma İpuçları:\n" +
                           "- Her konuyu adım adım takip edin\n" +
                           "- Önemli kavramları not alın\n" +
                           "- Pratik örnekler üzerinde çalışın\n" +
                           "- Konuları günlük hayatla ilişkilendirin\n\n" +
                           "🔄 Sorun çözülünce tekrar deneyin!";
                }
                
                if (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("RESOURCE_EXHAUSTED") || ex.Message.Contains("quota"))
                {
                    return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                           "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                           "📅 Ne Yapabilirsiniz:\n" +
                           "- Yarın tekrar deneyebilirsiniz\n" +
                           "- Şimdilik manuel özet kullanabilirsiniz\n" +
                           "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                           "💡 Manuel Özet:\n" +
                           $"📚 {videoTitle}\n\n" +
                           $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                           "✅ Çalışma İpuçları:\n" +
                           "- Her konuyu adım adım takip edin\n" +
                           "- Önemli kavramları not alın\n" +
                           "- Pratik örnekler üzerinde çalışın\n" +
                           "- Konuları günlük hayatla ilişkilendirin\n\n" +
                           "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                }
                
                 if (ex.Message.Contains("503") || ex.Message.Contains("overloaded") || ex.Message.Contains("UNAVAILABLE") || ex.Message.Contains("Service Unavailable"))
                 {
                     return "🤖 AI Servisi Meşgul\n\n" +
                            "Gemini AI şu anda çok yoğun. Lütfen birkaç dakika sonra tekrar deneyin.\n\n" +
                            "📅 Ne Yapabilirsiniz:\n" +
                            "- Birkaç dakika bekleyip tekrar deneyin\n" +
                            "- Şimdilik manuel özet kullanabilirsiniz\n" +
                            "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                            "💡 Manuel Özet:\n" +
                            $"📚 {videoTitle}\n\n" +
                            $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                            "✅ Çalışma İpuçları:\n" +
                            "- Her konuyu adım adım takip edin\n" +
                            "- Önemli kavramları not alın\n" +
                            "- Pratik örnekler üzerinde çalışın\n" +
                            "- Konuları günlük hayatla ilişkilendirin\n\n" +
                            "🔄 Birkaç dakika sonra tekrar deneyin!";
                 }
                 
                 if (ex.Message.Contains("400") || ex.Message.Contains("Bad Request") || ex.Message.Contains("API key not valid") || ex.Message.Contains("INVALID_ARGUMENT") || ex.Message.Contains("API_KEY_INVALID"))
                 {
                     return "🔑 API Anahtarınızda Sorun Var\n\n" +
                            "Girdiğiniz API anahtarı geçersiz veya yanlış format.\n\n" +
                            "🔍 Kontrol Edilecekler:\n" +
                            "- API anahtarınızın doğru kopyalandığından emin olun\n" +
                            "- Gemini AI Studio'dan aldığınız anahtarı kullandığınızdan emin olun\n" +
                            "- Anahtarın tam ve eksiksiz olduğunu kontrol edin\n\n" +
                            "💡 Çözüm:\n" +
                            "1. Profil sayfanıza gidin\n" +
                            "2. API anahtarınızı tekrar kontrol edin\n" +
                            "3. Gerekirse yeni bir anahtar alın\n" +
                            "4. Doğru anahtarı kaydedin\n\n" +
                            "📚 Manuel Özet:\n" +
                            $"📚 {videoTitle}\n\n" +
                            $"📖 Konu Başlıkları:\n{videoDescription}\n\n" +
                            "✅ Çalışma İpuçları:\n" +
                            "- Her konuyu adım adım takip edin\n" +
                            "- Önemli kavramları not alın\n" +
                            "- Pratik örnekler üzerinde çalışın\n" +
                            "- Konuları günlük hayatla ilişkilendirin\n\n" +
                            "🔄 API anahtarınızı düzelttikten sonra tekrar deneyin!";
                 }
                
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

                var userApiKey = await GetUserApiKeyAsync();
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={userApiKey}", content);
                
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
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || 
                        errorContent.Contains("429") || 
                        errorContent.Contains("Too Many Requests") || 
                        errorContent.Contains("RESOURCE_EXHAUSTED") ||
                        errorContent.Contains("quota"))
                    {
                        return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                               "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                               "📅 Ne Yapabilirsiniz:\n" +
                               "- Yarın tekrar deneyebilirsiniz\n" +
                               "- Şimdilik manuel özet kullanabilirsiniz\n" +
                               "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                               "💡 Manuel Özet:\n" +
                               "📝 Ana Konular:\n" +
                               "- Video içeriğindeki temel kavramlar\n" +
                               "- Önemli konu başlıkları\n\n" +
                               "✅ Önemli Noktalar:\n" +
                               "- Her konuyu adım adım takip edin\n" +
                               "- Önemli kavramları not alın\n\n" +
                               "💡 Pratik İpuçları:\n" +
                               "- Konuları günlük hayatla ilişkilendirin\n" +
                               "- Pratik örnekler üzerinde çalışın\n\n" +
                               "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                    }
                    
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("RESOURCE_EXHAUSTED") || ex.Message.Contains("quota"))
                {
                    return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                           "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                           "📅 Ne Yapabilirsiniz:\n" +
                           "- Yarın tekrar deneyebilirsiniz\n" +
                           "- Şimdilik manuel özet kullanabilirsiniz\n" +
                           "- PDF indirme özelliği çalışmaya devam eder\n\n" +
                           "💡 Manuel Özet:\n" +
                           "📝 Ana Konular:\n" +
                           "- Video içeriğindeki temel kavramlar\n" +
                           "- Önemli konu başlıkları\n\n" +
                           "✅ Önemli Noktalar:\n" +
                           "- Her konuyu adım adım takip edin\n" +
                           "- Önemli kavramları not alın\n\n" +
                           "💡 Pratik İpuçları:\n" +
                           "- Konuları günlük hayatla ilişkilendirin\n" +
                           "- Pratik örnekler üzerinde çalışın\n\n" +
                           "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                }
                
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

                var userApiKey = await GetUserApiKeyAsync();
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={userApiKey}", content);
                
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
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || 
                        errorContent.Contains("429") || 
                        errorContent.Contains("Too Many Requests") || 
                        errorContent.Contains("RESOURCE_EXHAUSTED") ||
                        errorContent.Contains("quota"))
                    {
                        return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                               "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                               "📅 Ne Yapabilirsiniz:\n" +
                               "- Yarın tekrar deneyebilirsiniz\n" +
                               "- Şimdilik genel cevaplar alabilirsiniz\n\n" +
                               $"💡 {question} sorusu için genel cevap:\n\n" +
                               $"📚 {videoTitle} konusunda bu soru önemli bir kavramı ele alıyor.\n" +
                               "📖 Konu başlıklarını tekrar gözden geçirin ve ilgili bölümleri dikkatlice inceleyin.\n" +
                               "🔍 Pratik örnekler üzerinde çalışarak konuyu daha iyi anlayabilirsiniz.\n\n" +
                               "🔄 Yarın tekrar AI cevabı alabilirsiniz!";
                    }
                    
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("RESOURCE_EXHAUSTED") || ex.Message.Contains("quota"))
                {
                    return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                           "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                           "📅 Ne Yapabilirsiniz:\n" +
                           "- Yarın tekrar deneyebilirsiniz\n" +
                           "- Şimdilik genel cevaplar alabilirsiniz\n\n" +
                           $"💡 {question} sorusu için genel cevap:\n\n" +
                           $"📚 {videoTitle} konusunda bu soru önemli bir kavramı ele alıyor.\n" +
                           "📖 Konu başlıklarını tekrar gözden geçirin ve ilgili bölümleri dikkatlice inceleyin.\n" +
                           "🔍 Pratik örnekler üzerinde çalışarak konuyu daha iyi anlayabilirsiniz.\n\n" +
                           "🔄 Yarın tekrar AI cevabı alabilirsiniz!";
                }
                
                return $"Hata oluştu: {ex.Message}";
            }
        }

        public async Task<byte[]> GeneratePdfFromSummaryAsync(string summary, string courseName, string videoTitle)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont;
                    
                    try
                    {
                        baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    catch
                    {
                        baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    }
                    Font titleFont = new Font(baseFont, 18, Font.BOLD, BaseColor.DARK_GRAY);
                    Font subtitleFont = new Font(baseFont, 14, Font.BOLD, BaseColor.GRAY);
                    Font normalFont = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);
                    Font smallFont = new Font(baseFont, 10, Font.NORMAL, BaseColor.GRAY);

                    Paragraph title = new Paragraph($"📚 {courseName}", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20f;
                    document.Add(title);

                    Paragraph subtitle = new Paragraph($"📖 {videoTitle}", subtitleFont);
                    subtitle.Alignment = Element.ALIGN_CENTER;
                    subtitle.SpacingAfter = 30f;
                    document.Add(subtitle);

                    Paragraph date = new Paragraph($"📅 Oluşturulma Tarihi: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}", smallFont);
                    date.Alignment = Element.ALIGN_RIGHT;
                    date.SpacingAfter = 20f;
                    document.Add(date);

                    LineSeparator line = new LineSeparator();
                    line.LineWidth = 1f;
                    line.LineColor = BaseColor.LIGHT_GRAY;
                    document.Add(line);
                    document.Add(new Paragraph(" ", normalFont));

                    string[] lines = summary.Split('\n');
                    foreach (string lineText in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(lineText))
                        {
                            Paragraph paragraph = new Paragraph(lineText, normalFont);
                            paragraph.SpacingAfter = 8f;
                            document.Add(paragraph);
                        }
                    }

                    document.Add(new Paragraph(" ", normalFont));
                    document.Add(line);
                    Paragraph footer = new Paragraph("🤖 Bu özet Gemini AI tarafından otomatik olarak oluşturulmuştur.", smallFont);
                    footer.Alignment = Element.ALIGN_CENTER;
                    footer.SpacingAfter = 10f;
                    document.Add(footer);

                    document.Close();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<string> SummarizePdfContentAsync(string pdfContent, string fileName)
        {
            try
            {
                string prompt = $@"
Bu PDF dosyasının içeriğini analiz et ve detaylı bir özet oluştur:

Dosya Adı: {fileName}
PDF İçeriği: {pdfContent}

Lütfen aşağıdaki formatta özetle:

📚 PDF Özeti

📝 Ana Konular:
- [Ana konuları listele]

✅ Önemli Noktalar:
- [Önemli noktaları listele]

💡 Ana Kavramlar:
- [Temel kavramları listele]

🔍 Detaylı Özet:
[PDF içeriğinin detaylı özeti]

🎯 Öğrenme Hedefleri:
- [Öğrenme hedeflerini listele]

💡 Pratik İpuçları:
- [Pratik ipuçlarını listele]

ÖNEMLİ KURALLAR:
1. Hiçbir markdown işareti kullanma (yıldız *, tire -, kare #, alt çizgi _)
2. Sadece normal metin olarak yaz
3. Başlıklar için normal büyük/küçük harf kullan
4. Liste öğeleri için sadece tire (-) kullan
5. Vurgu için sadece büyük harfler kullan
6. Tüm metni düz metin olarak yaz
7. Türkçe karakterleri (ç, ğ, ı, ö, ş, ü) doğru kullan
8. Emoji'leri koru ve görsel olarak çekici yap
9. PDF'in konusuna göre uygun özet yap
10. Akademik, teknik veya eğitim materyali olabilir

Not: PDF'in türüne göre (ders notu, makale, rapor vb.) uygun özet formatı kullan.
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

                var userApiKey = await GetUserApiKeyAsync();
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={userApiKey}", content);
                
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
                    
                    return "PDF özeti oluşturulamadı. Lütfen daha sonra tekrar deneyin.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests || 
                        errorContent.Contains("429") || 
                        errorContent.Contains("Too Many Requests") || 
                        errorContent.Contains("RESOURCE_EXHAUSTED") ||
                        errorContent.Contains("quota"))
                    {
                        return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                               "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                               "📅 Ne Yapabilirsiniz:\n" +
                               "- Yarın tekrar deneyebilirsiniz\n" +
                               "- Şimdilik manuel özet kullanabilirsiniz\n\n" +
                               "💡 Manuel Özet:\n" +
                               $"📚 {fileName} dosyası için genel özet:\n\n" +
                               "📝 Ana Konular:\n" +
                               "- PDF içeriğindeki temel kavramlar\n" +
                               "- Önemli konu başlıkları\n\n" +
                               "✅ Önemli Noktalar:\n" +
                               "- Her konuyu adım adım takip edin\n" +
                               "- Önemli kavramları not alın\n\n" +
                               "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                    }
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || 
                        errorContent.Contains("400") || 
                        errorContent.Contains("API key not valid") ||
                        errorContent.Contains("INVALID_ARGUMENT") ||
                        errorContent.Contains("API_KEY_INVALID"))
                    {
                        return "🔑 API Anahtarınızda Sorun Var\n\n" +
                               "Girdiğiniz API anahtarı geçersiz veya yanlış format.\n\n" +
                               "🔍 Kontrol Edilecekler:\n" +
                               "- API anahtarınızın doğru kopyalandığından emin olun\n" +
                               "- Gemini AI Studio'dan aldığınız anahtarı kullandığınızdan emin olun\n" +
                               "- Anahtarın tam ve eksiksiz olduğunu kontrol edin\n\n" +
                               "💡 Çözüm:\n" +
                               "1. Profil sayfanıza gidin\n" +
                               "2. API anahtarınızı tekrar kontrol edin\n" +
                               "3. Gerekirse yeni bir anahtar alın\n" +
                               "4. Doğru anahtarı kaydedin\n\n" +
                               "🔄 API anahtarınızı düzelttikten sonra tekrar deneyin!";
                    }
                    
                    return $"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout") || ex.Message.Contains("canceled"))
                {
                    return "⏰ API Yanıt Süresi Aşıldı\n\n" +
                           "API isteği zaman aşımına uğradı. Bu genellikle şu sebeplerden olur:\n\n" +
                           "🔍 Olası Sebepler:\n" +
                           "- API anahtarı yanlış format (Google Cloud API yerine Gemini AI Studio API kullanın)\n" +
                           "- Ağ bağlantısı sorunu\n" +
                           "- API servisi yoğun\n\n" +
                           "💡 Çözüm Önerileri:\n" +
                           "- API anahtarınızı kontrol edin (AI- ile başlamalı)\n" +
                           "- İnternet bağlantınızı kontrol edin\n" +
                           "- Birkaç dakika sonra tekrar deneyin\n\n" +
                           "🔄 Sorun çözülünce tekrar deneyin!";
                }
                
                if (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("RESOURCE_EXHAUSTED") || ex.Message.Contains("quota"))
                {
                    return "🤖 Günlük API Kullanım Limiti Aşıldı\n\n" +
                           "Gemini AI'nin günlük ücretsiz kullanım limitini (50 istek/gün) aştık.\n\n" +
                           "📅 Ne Yapabilirsiniz:\n" +
                           "- Yarın tekrar deneyebilirsiniz\n" +
                           "- Şimdilik manuel özet kullanabilirsiniz\n\n" +
                           "💡 Manuel Özet:\n" +
                           $"📚 {fileName} dosyası için genel özet:\n\n" +
                           "📝 Ana Konular:\n" +
                           "- PDF içeriğindeki temel kavramlar\n" +
                           "- Önemli konu başlıkları\n\n" +
                           "✅ Önemli Noktalar:\n" +
                           "- Her konuyu adım adım takip edin\n" +
                           "- Önemli kavramları not alın\n\n" +
                           "🔄 Yarın tekrar AI özeti alabilirsiniz!";
                }
                
                return $"Hata oluştu: {ex.Message}";
            }
        }

        public async Task<(int used, int total)> GetApiUsageAsync()
        {
            try
            {
                var testRequest = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = "Merhaba"
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(testRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var userApiKey = await GetUserApiKeyAsync();
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={userApiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return (1, 50); 
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                    if (errorContent.Contains("quotaValue"))
                    {
                        try
                        {
                            var responseObject = JsonSerializer.Deserialize<JsonElement>(errorContent);
                            if (responseObject.TryGetProperty("error", out var error) &&
                                error.TryGetProperty("details", out var details))
                            {
                                foreach (var detail in details.EnumerateArray())
                                {
                                    if (detail.TryGetProperty("violations", out var violations))
                                    {
                                        foreach (var violation in violations.EnumerateArray())
                                        {
                                            if (violation.TryGetProperty("quotaValue", out var quotaValue))
                                            {
                                                var total = quotaValue.GetString();
                                                if (int.TryParse(total, out int totalValue))
                                                {
                                                    return (totalValue, totalValue); 
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    
                    return (50, 50);
                }
                else
                {
                    return (0, 50); 
                }
            }
            catch (Exception)
            {
                return (0, 50);
            }
        }
    }
} 