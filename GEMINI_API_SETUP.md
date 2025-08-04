# Gemini API Kurulum Talimatları

Bu proje, YouTube videolarını Gemini AI kullanarak özetlemek için tasarlanmıştır.

## Kurulum Adımları

### 1. Gemini API Key Alın
1. [Google AI Studio](https://makersuite.google.com/app/apikey) adresine gidin
2. Google hesabınızla giriş yapın
3. "Create API Key" butonuna tıklayın
4. API key'inizi kopyalayın

### 2. API Key'i Projeye Ekleyin
1. `Bootcamp.PresentationLayer/appsettings.json` dosyasını açın
2. `"YOUR_GEMINI_API_KEY_HERE"` kısmını gerçek API key'inizle değiştirin:

```json
{
  "GeminiApi": {
    "ApiKey": "AIzaSyC...your-actual-api-key-here"
  }
}
```

### 3. Projeyi Çalıştırın
1. Visual Studio'da projeyi açın
2. NuGet paketlerinin yüklenmesini bekleyin
3. Projeyi çalıştırın (F5)

### 4. Video Özetleme Özelliğini Test Edin
1. Bir kursa giriş yapın
2. "StartLesson" sayfasına gidin
3. Sağ taraftaki "Dersi Özetle" butonuna tıklayın
4. Gemini AI video özetini oluşturacaktır

## Özellikler

- **YouTube Video Analizi**: YouTube video URL'lerini analiz eder
- **Akıllı Özetleme**: Gemini AI ile video içeriğini özetler
- **Türkçe Destek**: Özetler Türkçe olarak oluşturulur
- **Hata Yönetimi**: API hatalarını kullanıcı dostu mesajlarla gösterir

## API Kullanımı

Gemini API, aşağıdaki formatta video özetleri oluşturur:

```
📝 Ana Konular:
- [Ana konular listesi]

✅ Önemli Noktalar:
- [Önemli noktalar listesi]

💡 Öğrenme Hedefleri:
- [Öğrenme hedefleri listesi]

🔍 Detaylı Özet:
[Video içeriğinin detaylı özeti]
```

## Güvenlik Notları

- API key'inizi asla public repository'lere yüklemeyin
- Production ortamında API key'i environment variable olarak saklayın
- API kullanım limitlerini kontrol edin

## Sorun Giderme

### API Key Hatası
- API key'in doğru olduğundan emin olun
- API key'in aktif olduğunu kontrol edin

### Video Özetlenemiyor
- Video URL'sinin geçerli YouTube URL'si olduğunu kontrol edin
- Video'nun public olduğundan emin olun

### Bağlantı Hatası
- İnternet bağlantınızı kontrol edin
- Firewall ayarlarını kontrol edin 