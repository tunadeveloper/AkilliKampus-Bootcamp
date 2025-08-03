// Lesson Detail Page JavaScript

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

  // Sayfa yüklendiğinde elementleri başlangıçta gizle
  const animatedElements = document.querySelectorAll('.stagger-animation');
  animatedElements.forEach(element => {
    element.style.opacity = '0';
    element.style.transform = 'translateY(30px)';
    element.style.transition = 'all 0.8s ease-out';
  });

  // Intersection Observer ile animasyonları tetikle
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const delay = parseFloat(entry.target.dataset.delay) || 0;
        setTimeout(() => {
          entry.target.style.opacity = '1';
          entry.target.style.transform = 'translateY(0)';
          entry.target.classList.add('animate');
        }, delay * 200);
      }
    });
  }, { 
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
  });

  // Tüm animasyonlu elementleri gözlemle
  animatedElements.forEach(element => {
    observer.observe(element);
  });

  // Sayfa yüklendiğinde ilk elementleri hemen göster
  setTimeout(() => {
    const firstElements = document.querySelectorAll('.stagger-animation[data-delay="0"], .stagger-animation[data-delay="0.1"]');
    firstElements.forEach((element, index) => {
      setTimeout(() => {
        element.style.opacity = '1';
        element.style.transform = 'translateY(0)';
        element.classList.add('animate');
      }, index * 200);
    });
  }, 500);

  // Tab switching animasyonları
  const tabLinks = document.querySelectorAll('.nav-link');
  const tabPanes = document.querySelectorAll('.tab-pane');

  tabLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      e.preventDefault();
      
      // Aktif tab'ı değiştir
      tabLinks.forEach(l => l.classList.remove('active'));
      this.classList.add('active');
      
      // Tab içeriğini göster
      const targetId = this.getAttribute('data-bs-target');
      const targetPane = document.querySelector(targetId);
      
      tabPanes.forEach(pane => {
        pane.classList.remove('show', 'active');
      });
      
      targetPane.classList.add('show', 'active');
      
      // Tab içeriğini animasyonla göster
      targetPane.style.opacity = '0';
      targetPane.style.transform = 'translateY(20px)';
      
      setTimeout(() => {
        targetPane.style.transition = 'all 0.4s ease-out';
        targetPane.style.opacity = '1';
        targetPane.style.transform = 'translateY(0)';
      }, 100);
    });
  });

  // Curriculum item hover efektleri
  const curriculumItems = document.querySelectorAll('.curriculum-item');
  curriculumItems.forEach(item => {
    item.addEventListener('mouseenter', function() {
      this.style.transform = 'translateX(5px)';
    });
    
    item.addEventListener('mouseleave', function() {
      this.style.transform = 'translateX(0)';
    });
  });

  // Comment form handling
  const commentForm = document.querySelector('.comment-form');
  if (commentForm) {
    commentForm.addEventListener('submit', function(e) {
      e.preventDefault();
      
      const commentInput = document.getElementById('commentInput');
      const commentText = commentInput.value.trim();
      
      if (commentText) {
        addNewComment(commentText);
        commentInput.value = '';
        
        // Form animasyonu
        this.style.transform = 'scale(0.98)';
        setTimeout(() => {
          this.style.transform = 'scale(1)';
        }, 150);
      }
    });
  }

  // Yeni yorum ekleme fonksiyonu
  function addNewComment(text) {
    const commentsContainer = document.querySelector('.comments-container');
    const newComment = document.createElement('div');
    newComment.className = 'comment-item stagger-animation';
    newComment.setAttribute('data-delay', '0');
    
    const currentDate = new Date().toLocaleDateString('tr-TR');
    
    newComment.innerHTML = `
      <div class="comment-author">Yeni Kullanıcı</div>
      <div class="comment-date">${currentDate}</div>
      <div class="comment-text">${text}</div>
    `;
    
    commentsContainer.appendChild(newComment);
    
    // Yeni yorumu animasyonla göster
    setTimeout(() => {
      newComment.style.opacity = '1';
      newComment.style.transform = 'translateY(0)';
      newComment.classList.add('animate');
    }, 100);
  }

  // Progress bar animasyonu
  const progressBar = document.querySelector('.progress-bar');
  if (progressBar) {
    const progress = 65; // Örnek ilerleme yüzdesi
    progressBar.style.width = '0%';
    
    setTimeout(() => {
      progressBar.style.width = progress + '%';
    }, 1000);
  }

  // Sidebar button hover efektleri
  const sidebarButtons = document.querySelectorAll('.sidebar-btn');
  sidebarButtons.forEach(button => {
    button.addEventListener('mouseenter', function() {
      this.style.transform = 'translateY(-2px)';
    });
    
    button.addEventListener('mouseleave', function() {
      this.style.transform = 'translateY(0)';
    });
  });

  // Lesson image hover efekti
  const lessonImage = document.querySelector('.lesson-image');
  if (lessonImage) {
    lessonImage.addEventListener('mouseenter', function() {
      this.style.transform = 'scale(1.05)';
    });
    
    lessonImage.addEventListener('mouseleave', function() {
      this.style.transform = 'scale(1)';
    });
  }

  // Smooth scroll için iç linkler
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
  }, 16);

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
          window.scrollBy({
            top: 100,
            behavior: 'smooth'
          });
        } else {
          // Aşağı swipe - scroll up
          window.scrollBy({
            top: -100,
            behavior: 'smooth'
          });
        }
      }
    }
  }

  // Keyboard navigation
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape') {
      // Modal'ları kapat veya form alanlarını temizle
      const commentInput = document.getElementById('commentInput');
      if (commentInput) {
        commentInput.value = '';
      }
    }
  });

  // Accessibility improvements
  const interactiveElements = document.querySelectorAll('.curriculum-item, .comment-item, .sidebar-btn');
  interactiveElements.forEach((element, index) => {
    element.setAttribute('tabindex', '0');
    element.setAttribute('role', 'button');
    element.setAttribute('aria-label', `İnteraktif element ${index + 1}`);
    
    element.addEventListener('keydown', function(e) {
      if (e.key === 'Enter' || e.key === ' ') {
        e.preventDefault();
        this.click();
      }
    });
  });

  // Loading state management
  function showLoading(element) {
    element.classList.add('loading');
  }

  function hideLoading(element) {
    element.classList.remove('loading');
  }

  // Lesson start functionality
  const startLessonBtn = document.querySelector('.start-lesson-btn');
  if (startLessonBtn) {
    startLessonBtn.addEventListener('click', function() {
      showLoading(this);
      
      // Simulate lesson start
      setTimeout(() => {
        hideLoading(this);
        window.location.href = 'start-lesson.html';
      }, 2000);
    });
  }

  // Enroll functionality
  const enrollBtn = document.querySelector('.enroll-btn');
  if (enrollBtn) {
    enrollBtn.addEventListener('click', function() {
      showLoading(this);
      
      // Simulate enrollment
      setTimeout(() => {
        hideLoading(this);
        this.innerHTML = '<i class="bi bi-check-circle me-2"></i>Kayıt Olundu';
        this.classList.remove('btn-warning');
        this.classList.add('btn-success');
      }, 1500);
    });
  }

  // Rating system
  const ratingStars = document.querySelectorAll('.rating-star');
  ratingStars.forEach((star, index) => {
    star.addEventListener('click', function() {
      const rating = index + 1;
      updateRating(rating);
    });
    
    star.addEventListener('mouseenter', function() {
      highlightStars(index);
    });
    
    star.addEventListener('mouseleave', function() {
      resetStars();
    });
  });

  function updateRating(rating) {
    ratingStars.forEach((star, index) => {
      if (index < rating) {
        star.classList.add('active');
      } else {
        star.classList.remove('active');
      }
    });
  }

  function highlightStars(rating) {
    ratingStars.forEach((star, index) => {
      if (index <= rating) {
        star.style.color = '#FFC107';
      } else {
        star.style.color = '#e9ecef';
      }
    });
  }

  function resetStars() {
    ratingStars.forEach(star => {
      star.style.color = '';
    });
  }

  // Global functions
  window.lessonDetailPage = {
    showLoading,
    hideLoading,
    addNewComment,
    updateRating,
    startLesson: function() {
      console.log('Ders başlatılıyor...');
    },
    enrollCourse: function() {
      console.log('Kursa kayıt olunuyor...');
    }
  };

  // Page load completion
  setTimeout(() => {
    document.body.classList.add('page-loaded');
  }, 1000);
}); 