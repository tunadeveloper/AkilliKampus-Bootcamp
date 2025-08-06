# 🎓 AkilliKampus-Bootcamp

**AkilliKampus-Bootcamp**, ASP.NET Core 8.0 ile geliştirilen, katmanlı mimariye sahip, yapay zeka destekli ve sertifika odaklı online eğitim platformudur. Kullanıcılar kurslara katılabilir, videolar izleyebilir, ilerlemelerini takip edebilir ve kursları tamamladıklarında sertifika alabilirler.

---

## 📦 Kullanılan Teknolojiler

- **Backend**: ASP.NET Core 8.0 MVC
- **Frontend**: Razor View, HTML5, CSS3, JavaScript, Bootstrap 5
- **Database**: SQL Server (Code First)
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Validation**: FluentValidation
- **AI**: Google Gemini API (Ders özetleme, Soru-Cevap, PDF Özetleme)
- **PDF**: iTextSharp ile dinamik yazı çevirme
- **Video**: YoutubeExplode kütüphanesi ile video işlemleri
- **Architecture**: N-Layer Architecture, Clean Architecture, Repository Pattern
- **Structures**: ASP.NET Areas, Component & Partial Views

---

## 🧱 Proje Yapısı

```bash
AkilliKampus-Bootcamp/
├── Bootcamp.EntityLayer/         # Veri modelleri (Entities)
├── Bootcamp.DataAccessLayer/     # Entity Framework, Repositories
├── Bootcamp.BusinessLayer/       # İş kuralları (Services, Managers)
└── Bootcamp.PresentationLayer/   # UI, Views, Controllers, Areas
```

---

## 🚀 Özellikler

### 👤 Kullanıcı Yönetimi
- ASP.NET Core Identity ile kullanıcı kayıt/giriş
- Rol bazlı yetkilendirme (Admin, Öğrenci)
- Profil düzenleme, şifre güncelleme

### 📚 Kurs & Video Sistemi
- Kategorilere ve seviyelere ayrılmış kurslar
- Eğitmen bilgileri, kazanımlar, açıklamalar
- Video süresi takibi ve kurs ilerleme yüzdesi
- AI ile video içeriği özetleme (Gemini API)

### 🛠️ Admin Paneli
- Tüm tablolar için CRUD işlemleri
- Kullanıcı, kurs ve video yönetimi
- DataTables ile arama, filtreleme ve sıralama
- Site ayarları ve istatistik ekranları

---

## 🔒 Güvenlik Önlemleri

- ASP.NET Core Identity ile authentication
- Role-based authorization
- CSRF, XSS ve SQL Injection korumaları
- Input validation ve veri temizleme

---

## ⚡ Performans Optimizasyonları

- Eager loading ve Include() kullanımı
- Veritabanı indexing
- Lazy loading ile görsel optimizasyonu
- Minify edilmiş CSS/JS dosyaları
- CDN kullanımı

---


### Kurulum Adımları

```bash
# 1. Repo'yu klonla
git clone [repository-url]

# 2. Veritabanını oluştur
dotnet ef database update

# 3. Uygulamayı başlat
dotnet run --project Bootcamp.PresentationLayer
```

---

## 📝 Sonuç

AkilliKampus-Bootcamp, modern yazılım geliştirme standartlarına uygun, genişletilebilir ve kullanıcı odaklı bir eğitim platformudur. Güvenlik, performans ve yapay zeka entegrasyonları ile profesyonel bir deneyim sunar. Gelecekte canlı yayın, ödeme sistemleri ve mobil uygulama gibi özelliklerle genişletilmeye hazırdır.
