// Lessons Page JavaScript

// Smooth scroll ve animasyonlar
document.addEventListener('DOMContentLoaded', function() {
  // Navbar scroll efekti
  window.addEventListener('scroll', function() {
    const navbar = document.querySelector('.modern-header');
    if (window.scrollY > 50) {
      navbar.classList.add('scrolled');
    } else {
      navbar.classList.remove('scrolled');
    }
  });

  // Sayfa yüklendiğinde kartları başlangıçta gizle
  const lessonCards = document.querySelectorAll('.modern-lesson-card');
  lessonCards.forEach(card => {
    card.style.opacity = '0';
    card.style.transform = 'translateY(50px)';
    card.style.transition = 'all 0.8s ease-out';
  });

  // Intersection Observer ile animasyonları tetikle
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('animate-fade-in');
      }
    });
  }, { threshold: 0.1 });

  // Stagger animation için - kartlar sırayla görünür
  const staggerObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const delay = parseFloat(entry.target.dataset.delay) || 0;
        setTimeout(() => {
          entry.target.style.opacity = '1';
          entry.target.style.transform = 'translateY(0)';
          entry.target.classList.add('animate');
        }, delay * 200); // Daha yavaş animasyon için 200ms
      }
    });
  }, { 
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px' // Kartlar biraz daha erken tetiklensin
  });

  // Tüm kartları gözlemle
  document.querySelectorAll('.modern-lesson-card').forEach(card => {
    staggerObserver.observe(card);
  });

  // Sayfa yüklendiğinde ilk kartları hemen göster
  setTimeout(() => {
    const firstCards = document.querySelectorAll('.modern-lesson-card[data-delay="0"], .modern-lesson-card[data-delay="0.1"], .modern-lesson-card[data-delay="0.2"]');
    firstCards.forEach((card, index) => {
      setTimeout(() => {
        card.style.opacity = '1';
        card.style.transform = 'translateY(0)';
        card.classList.add('animate');
      }, index * 200); // Her kart 200ms arayla görünür
    });
  }, 500); // Sayfa yüklendikten 500ms sonra başla

  // Filtre fonksiyonalitesi
  const filterBtn = document.querySelector('.btn-primary');
  const searchInput = document.getElementById('arama');
  const categorySelect = document.getElementById('kategori');
  const difficultySelect = document.getElementById('zorluk');

  if (filterBtn) {
    filterBtn.addEventListener('click', function() {
      // Filtreleme mantığı burada implement edilebilir
      console.log('Filtreleme:', {
        search: searchInput.value,
        category: categorySelect.value,
        difficulty: difficultySelect.value
      });
    });
  }

  // Kart hover efektleri
  document.querySelectorAll('.modern-lesson-card').forEach(card => {
    card.addEventListener('mouseenter', function() {
      this.style.transform = 'translateY(-8px) scale(1.02)';
    });
    
    card.addEventListener('mouseleave', function() {
      this.style.transform = 'translateY(0) scale(1)';
    });
  });

  // Load more button click
  const loadMoreBtn = document.querySelector('.load-more-btn');
  if (loadMoreBtn) {
    loadMoreBtn.addEventListener('click', function() {
      // Load more functionality
      console.log('Daha fazla eğitim yükleniyor...');
      
      // Burada AJAX ile daha fazla eğitim yüklenebilir
      // Örnek: loadMoreLessons();
    });
  }

  // Smooth scroll için tüm iç linkler
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

  // Form validation
  const forms = document.querySelectorAll('.needs-validation');
  
  forms.forEach(form => {
    form.addEventListener('submit', function(event) {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }
      form.classList.add('was-validated');
    });
  });

  // Search functionality
  if (searchInput) {
    searchInput.addEventListener('input', function() {
      const searchTerm = this.value.toLowerCase();
      const cards = document.querySelectorAll('.modern-lesson-card');
      
      cards.forEach(card => {
        const title = card.querySelector('h3').textContent.toLowerCase();
        const description = card.querySelector('p').textContent.toLowerCase();
        
        if (title.includes(searchTerm) || description.includes(searchTerm)) {
          card.style.display = 'block';
          // Arama sonucunda kartları tekrar animasyonla göster
          setTimeout(() => {
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
          }, 100);
        } else {
          card.style.opacity = '0';
          card.style.transform = 'translateY(20px)';
          setTimeout(() => {
            card.style.display = 'none';
          }, 300);
        }
      });
    });
  }

  // Category filter
  if (categorySelect) {
    categorySelect.addEventListener('change', function() {
      const selectedCategory = this.value;
      const cards = document.querySelectorAll('.modern-lesson-card');
      
      cards.forEach(card => {
        const category = card.querySelector('.category-badge').textContent;
        
        if (selectedCategory === 'Tümü' || category === selectedCategory) {
          card.style.display = 'block';
          setTimeout(() => {
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
          }, 100);
        } else {
          card.style.opacity = '0';
          card.style.transform = 'translateY(20px)';
          setTimeout(() => {
            card.style.display = 'none';
          }, 300);
        }
      });
    });
  }

  // Difficulty filter
  if (difficultySelect) {
    difficultySelect.addEventListener('change', function() {
      const selectedDifficulty = this.value;
      const cards = document.querySelectorAll('.modern-lesson-card');
      
      cards.forEach(card => {
        const difficulty = card.querySelector('.badge').textContent;
        
        if (selectedDifficulty === 'Tümü' || difficulty === selectedDifficulty) {
          card.style.display = 'block';
          setTimeout(() => {
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
          }, 100);
        } else {
          card.style.opacity = '0';
          card.style.transform = 'translateY(20px)';
          setTimeout(() => {
            card.style.display = 'none';
          }, 300);
        }
      });
    });
  }

  // Performance optimization için debounce fonksiyonu
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

  // Kart animasyonları için performans optimizasyonu
  const cardObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.style.opacity = '1';
        entry.target.style.transform = 'translateY(0)';
      }
    });
  }, { threshold: 0.1 });

  // Tüm kartları gözlemle
  document.querySelectorAll('.modern-lesson-card').forEach(card => {
    cardObserver.observe(card);
  });

  // Keyboard navigation
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape') {
      // Modal'ları kapat veya arama alanını temizle
      if (searchInput) {
        searchInput.value = '';
        searchInput.dispatchEvent(new Event('input'));
      }
    }
  });

  // Accessibility improvements
  document.querySelectorAll('.modern-lesson-card').forEach((card, index) => {
    card.setAttribute('tabindex', '0');
    card.setAttribute('role', 'button');
    card.setAttribute('aria-label', `Eğitim kartı ${index + 1}`);
    
    card.addEventListener('keydown', function(e) {
      if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        const link = this.querySelector('a');
        if (link) {
          link.click();
        }
      }
    });
  });

  // Loading state management
  function showLoading() {
    const loadMoreBtn = document.querySelector('.load-more-btn');
    if (loadMoreBtn) {
      loadMoreBtn.innerHTML = '<i class="bi bi-arrow-clockwise me-2"></i>Yükleniyor...';
      loadMoreBtn.disabled = true;
    }
  }

  function hideLoading() {
    const loadMoreBtn = document.querySelector('.load-more-btn');
    if (loadMoreBtn) {
      loadMoreBtn.innerHTML = '<i class="bi bi-arrow-down me-2"></i>Daha Fazla Eğitim Yükle';
      loadMoreBtn.disabled = false;
    }
  }

  // Global functions
  window.lessonsPage = {
    showLoading,
    hideLoading,
    filterLessons: function(criteria) {
      console.log('Filtreleme kriterleri:', criteria);
      // Filtreleme mantığı burada implement edilebilir
    },
    loadMoreLessons: function() {
      showLoading();
      // AJAX ile daha fazla eğitim yükle
      setTimeout(() => {
        hideLoading();
      }, 2000);
    }
  };
}); 