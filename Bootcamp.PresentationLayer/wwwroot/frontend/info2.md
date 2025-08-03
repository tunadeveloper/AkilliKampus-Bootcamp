# 🎓 AI Destekli Eğitim Platformu - Frontend Tasarım Dokümantasyonu

## 🎨 Genel Bilgiler

- **Teknolojiler:** HTML, CSS, Bootstrap 5, Vanilla JavaScript
- **Tasarım Hedefi:** Modern, etkileşimli, dikkat çekici ve kullanıcı dostu
- **Renk Teması:**  
  - **Ana Renk:** #6F42C1 (Mor)
  - **İkincil Renk:** #ffffff (Beyaz)
  - **Vurgu Rengi:** #FFC107 (Amber/Sarı)
  - **Yardımcı Renk:** #212529 (Koyu gri/navy)

---

## 🗂️ Sayfa Listesi

### 1. `/index.html` - **Ana Sayfa**
**Amaç:** Kullanıcıyı karşılayan, platformun ne sunduğunu anlatan modern landing page.

- **Header:**
  - Logo (sol)
  - Menü (Eğitimler, Hakkında, Giriş Yap, Kayıt Ol)
- **Hero Section:**
  - Büyük başlık: "Geleceğin Eğitimi Burada!"
  - Kısa açıklama: "Yapay zeka destekli eğitimle daha hızlı öğrenin."
  - Call to Action butonları:
    - "Eğitimleri Görüntüle" → `/lessons.html`
    - "Hemen Başla" → `/register.html`
- **Özellikler Bölümü (3 Kart):**
  - AI Tabanlı Öneriler
  - Eğitim Takibi
  - Soru Cevap Asistanı
- **Popüler Eğitimler (Slider veya Grid):**
  - Her kartta:
    - Görsel
    - Başlık
    - Kısa açıklama
    - "Detay" butonu → `/lesson-detail.html`
- **Footer:**
  - Sosyal medya linkleri
  - Hakkında
  - İletişim

---

### 2. `/lessons.html` - **Eğitim Listesi Sayfası**
**Amaç:** Eğitimleri listelemek, filtrelemek ve detaylara yönlendirmek.

- **Filtre Alanı (Sidebar veya üstte):**
  - Kategori filtresi
  - Zorluk seviyesi filtresi
  - Arama alanı
- **Eğitim Kartları (Grid):**
  - Görsel
  - Eğitim Adı
  - Açıklama (kısa)
  - "Detay" butonu → `/lesson-detail.html`

---

### 3. `/lesson-detail.html` - **Eğitim Detay Sayfası**
**Amaç:** Seçilen eğitimin detaylarını göstermek ve başlatmak.

- **Başlık & Açıklama:**
  - Eğitim adı
  - Eğitmen bilgisi
  - Zorluk, kategori bilgisi
- **Açıklayıcı Sekmeler (Bootstrap Tab yapısı):**
  - Hakkında
  - Müfredat (ders listesi)
  - Yorumlar
- **"Eğitime Başla" Butonu:**
  - Login olmayan kullanıcıyı `/login.html` sayfasına yönlendirir.
  - Giriş yapılmışsa `/start-lesson.html`’a gider.
- **Sidebar Bilgileri:**
  - Süre: 4 Saat
  - Videolar: 10
  - Sertifika: Var/Yok
  - Buton: "Kayıt Ol"

---

### 4. `/login.html` - **Giriş Sayfası**
- Email / şifre girişi
- "Beni Hatırla" checkbox
- "Şifremi unuttum" bağlantısı
- "Giriş Yap" butonu → `/profile.html`
- "Kayıt Ol" linki → `/register.html`

---

### 5. `/register.html` - **Kayıt Sayfası**
- Ad, soyad, e-posta, şifre, şifre tekrar
- "Kayıt Ol" butonu → `/profile.html`

---

### 6. `/profile.html` - **Kullanıcı Paneli**
- Hoşgeldiniz mesajı
- Eğitim geçmişi
- Devam edilen eğitimler
- "Eğitimlere Git" butonu → `/lessons.html`
- "Çıkış Yap" butonu

---

### 7. `/start-lesson.html` - **Ders Başlatma Sayfası**
- Sol tarafta video oynatıcı
- Sağda ders notları ve “AI Asistan” chat penceresi
- Altta "Sonraki Derse Geç" butonu

---

### 8. `/admin/login.html` - **Admin Giriş Sayfası**
- Email/Şifre giriş alanı
- "Giriş" butonu → `/admin/dashboard.html`

---

### 9. `/admin/dashboard.html` - **Admin Panel Ana Sayfası**
- Sol menü:
  - Eğitimler
  - Kullanıcılar
  - Kategoriler
  - Yorumlar
- İçerik alanı:
  - Eğitim ekle
  - Eğitim düzenle/sil
  - İstatistik grafikleri (HTML/CSS ile temsili gösterim)

---

## 🧠 UX Notları

- Mobil uyumlu tasarım (Bootstrap grid)
- Hover animasyonları ve buton geçiş efektleri
- Sticky header
- Modal kullanımı (Giriş, kayıt, yorum ekleme gibi işlemler)

---

## 🧩 JavaScript Fonksiyonları

- Arama filtrelemesi (`lessons.html`)
- Accordion/müfredat kontrolü (`lesson-detail.html`)
- Basit yorum gönderme işlemleri (yerel JS array ile simülasyon)
- Video ilerleme simülasyonu

---

## ✍️ Ekstra Not

Cursor AI, bu arayüzleri modern ve sade bir estetikle, temiz arayüz hiyerarşisiyle oluşturmalı. Renk teması tutarlı olmalı, özellikle butonlar ve başlıklar vurgulu renk olan `#6F42C1` (mor) ile öne çıkarılmalı.

---

