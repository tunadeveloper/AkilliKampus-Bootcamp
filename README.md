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

<img width="1280" height="2144" alt="Image" src="https://github.com/user-attachments/assets/0dc37ac2-3c09-4a3e-a871-de7283f47d6c" />
<img width="1280" height="2429" alt="Image" src="https://github.com/user-attachments/assets/5e0a7c01-534f-4857-94d5-30615385ba43" />
<img width="1280" height="2238" alt="Image" src="https://github.com/user-attachments/assets/d025f604-6759-47fd-9a26-40d9e11dd327" />
<img width="1280" height="5389" alt="Image" src="https://github.com/user-attachments/assets/c7910565-1790-4142-a282-0e69c1ecdece" />
<img width="1280" height="2515" alt="Image" src="https://github.com/user-attachments/assets/ffb8fd8a-83e8-4938-b3d3-9a2d6470f7e5" />
<img width="1280" height="1589" alt="Image" src="https://github.com/user-attachments/assets/f03959d2-2188-49ea-b87c-6c1f880e24e0" />
<img width="1280" height="1979" alt="Image" src="https://github.com/user-attachments/assets/b7f348d9-825a-45ff-88a9-b33a3654a956" />
<img width="1280" height="1119" alt="Image" src="https://github.com/user-attachments/assets/8b4c1314-d328-421e-b72f-9a88ab6d8950" />
<img width="1280" height="889" alt="Image" src="https://github.com/user-attachments/assets/864e3e8e-8ed1-432f-89ba-17a72f57c477" />
<img width="1280" height="889" alt="Image" src="https://github.com/user-attachments/assets/d2451efe-ae5e-4747-b62a-e479eaa815d7" />

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

📺 [Detaylı İnceleme](https://www.youtube.com/watch?v=iz1inUpFArw)


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
