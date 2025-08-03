// Typewriter animasyonu
const typewriterTexts = [
  'Geleceğin Eğitimi Burada!',
  'Yapay Zeka ile Öğren!',
  'Kişiselleştirilmiş Eğitim Deneyimi!',
  'Başarıya Birlikte Ulaş!'
];
let typeIndex = 0, charIndex = 0, isDeleting = false;
const typewriterEl = document.getElementById('typewriter');
const cursorEl = document.querySelector('.typewriter-cursor');

function typeWriterLoop() {
  if (!typewriterEl) return;
  const currentText = typewriterTexts[typeIndex];
  if (isDeleting) {
    typewriterEl.textContent = currentText.substring(0, charIndex - 1);
    charIndex--;
    if (charIndex === 0) {
      isDeleting = false;
      typeIndex = (typeIndex + 1) % typewriterTexts.length;
      setTimeout(typeWriterLoop, 800); // Silme bittikten sonra bekleme süresi
    } else {
      setTimeout(typeWriterLoop, 50); // Silme hızı
    }
  } else {
    typewriterEl.textContent = currentText.substring(0, charIndex + 1);
    charIndex++;
    if (charIndex === currentText.length) {
      isDeleting = true;
      setTimeout(typeWriterLoop, 1700); // Yazma bittikten sonra 3 saniye bekle
    } else {
      setTimeout(typeWriterLoop, 50); // Yazma hızı artırıldı
    }
  }
}

// Açılış animasyonlarını tetikle
window.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('.animate-fadein, .animate-fadein-delay1, .animate-fadein-delay2, .animate-fadein-delay3, .animate-fadein-delay4, .animate-fadein-delay5')
    .forEach(el => {
      el.classList.add('animated');
    });
});

// Testimonial Slider
const slider = document.getElementById('testimonial-slider');
const prevBtn = document.getElementById('testimonial-prev');
const nextBtn = document.getElementById('testimonial-next');
let testimonialIndex = 0;
const testimonialCards = slider ? slider.children.length : 0;
const visibleCards = 2; // Mobilde 1, desktopta 2-3 olabilir, basit tutuyoruz

function updateSlider(animated = false) {
  if (!slider) return;
  for (let i = 0; i < testimonialCards; i++) {
    const card = slider.children[i];
    if (i === testimonialIndex) {
      card.classList.add('active');
      if (animated) {
        card.classList.remove('fade-in');
        void card.offsetWidth; // reflow
        card.classList.add('fade-in');
      }
    } else {
      card.classList.remove('active', 'fade-in');
    }
  }
  if (testimonialCards <= 1) {
    prevBtn.disabled = true;
    nextBtn.disabled = true;
  } else {
    prevBtn.disabled = false;
    nextBtn.disabled = false;
  }
}
if (prevBtn && nextBtn && slider) {
  prevBtn.addEventListener('click', () => {
    testimonialIndex = (testimonialIndex - 1 + testimonialCards) % testimonialCards;
    updateSlider(true);
  });
  nextBtn.addEventListener('click', () => {
    testimonialIndex = (testimonialIndex + 1) % testimonialCards;
    updateSlider(true);
  });
  setInterval(() => {
    if (testimonialCards > 1) {
      testimonialIndex = (testimonialIndex + 1) % testimonialCards;
      updateSlider(true);
    }
  }, 4000);
  updateSlider();
}

// Scroll down butonu
const scrollDownBtn = document.getElementById('scroll-down-btn');
if (scrollDownBtn) {
  scrollDownBtn.addEventListener('click', function(e) {
    e.preventDefault();
    const target = document.getElementById('features');
    if (target) {
      target.scrollIntoView({ behavior: 'smooth' });
    }
  });
}

// --- HTML'den Taşınan JavaScript Kodları ---

// Smooth scroll için tüm iç linkler
document.addEventListener('DOMContentLoaded', function() {
  // Tüm iç linkleri seç
  const internalLinks = document.querySelectorAll('a[href^="#"]');
  
  internalLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      e.preventDefault();
      
      const targetId = this.getAttribute('href');
      const targetElement = document.querySelector(targetId);
      
      if (targetElement) {
        targetElement.scrollIntoView({
          behavior: 'smooth',
          block: 'start'
        });
      }
    });
  });
});

// Navbar scroll efekti
window.addEventListener('scroll', function() {
  const navbar = document.getElementById('mainNavbar');
  if (navbar) {
    if (window.scrollY > 50) {
      navbar.classList.add('scrolled');
    } else {
      navbar.classList.remove('scrolled');
    }
  }
});

// Intersection Observer ile animasyonları tetikle
const observerOptions = {
  threshold: 0.1,
  rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver(function(entries) {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('animate-in');
    }
  });
}, observerOptions);

// Animasyonlu elementleri gözlemle
document.addEventListener('DOMContentLoaded', function() {
  const animatedElements = document.querySelectorAll('.animate-fadein, .animate-fadein-delay1, .animate-fadein-delay2, .animate-fadein-delay3, .animate-fadein-delay4, .animate-fadein-delay5');
  
  animatedElements.forEach(el => {
    observer.observe(el);
  });
});

// Kart hover efektleri için touch cihazlarda özel davranış
document.addEventListener('DOMContentLoaded', function() {
  const cards = document.querySelectorAll('.lesson-card-anim, .team-card-advanced, .success-story-card, .success-stat-card');
  
  cards.forEach(card => {
    // Touch cihazlarda hover yerine click kullan
    if ('ontouchstart' in window) {
      card.addEventListener('touchstart', function() {
        this.classList.add('touch-hover');
      });
      
      card.addEventListener('touchend', function() {
        setTimeout(() => {
          this.classList.remove('touch-hover');
        }, 300);
      });
    }
  });
});

// Performans optimizasyonu için debounce fonksiyonu
function debounce(func, wait) {
  let timeout;
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}

// Scroll performansını optimize et
const optimizedScrollHandler = debounce(function() {
  // Scroll event handler içeriği buraya
}, 16); // ~60fps

window.addEventListener('scroll', optimizedScrollHandler);

// Sayfa yüklendiğinde tüm animasyonları başlat
document.addEventListener('DOMContentLoaded', function() {
  // Typewriter animasyonunu başlat - sadece bir kez başlat
  if (typewriterEl && !window.typewriterStarted) {
    window.typewriterStarted = true;
    setTimeout(typeWriterLoop, 1000);
  }
  
  // Açılış animasyonlarını tetikle
  const animatedElements = document.querySelectorAll('.animate-fadein, .animate-fadein-delay1, .animate-fadein-delay2, .animate-fadein-delay3, .animate-fadein-delay4, .animate-fadein-delay5');
  
  animatedElements.forEach((el, index) => {
    setTimeout(() => {
      el.classList.add('animated');
    }, index * 200);
  });
});

// Mobil cihazlarda touch gesture'ları
if ('ontouchstart' in window) {
  let touchStartY = 0;
  let touchEndY = 0;
  
  document.addEventListener('touchstart', function(e) {
    touchStartY = e.touches[0].clientY;
  });
  
  document.addEventListener('touchend', function(e) {
    touchEndY = e.changedTouches[0].clientY;
    handleSwipe();
  });
  
  function handleSwipe() {
    const swipeThreshold = 50;
    const diff = touchStartY - touchEndY;
    
    if (Math.abs(diff) > swipeThreshold) {
      if (diff > 0) {
        // Yukarı swipe - scroll down
        const scrollDownBtn = document.getElementById('scroll-down-btn');
        if (scrollDownBtn) {
          scrollDownBtn.click();
        }
      }
    }
  }
} 