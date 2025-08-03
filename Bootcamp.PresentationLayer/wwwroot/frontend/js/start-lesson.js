// Smooth scrolling and animations
document.addEventListener('DOMContentLoaded', function() {
  // Add fade-in animation to elements
  const elements = document.querySelectorAll('.video-container, .ai-chat-modern, .lesson-notes-modern');
  elements.forEach((el, index) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    setTimeout(() => {
      el.style.transition = 'all 0.6s ease';
      el.style.opacity = '1';
      el.style.transform = 'translateY(0)';
    }, index * 200);
  });
  
  // Progress bar animation
  const progressBar = document.querySelector('.progress-bar-modern');
  setTimeout(() => {
    progressBar.style.width = '25%';
  }, 500);
  
  // Chat functionality
  const chatForm = document.querySelector('.chat-messages').parentElement.querySelector('form');
  const chatInput = chatForm.querySelector('input');
  const chatMessages = document.querySelector('.chat-messages');
  
  chatForm.addEventListener('submit', function(e) {
    e.preventDefault();
    const message = chatInput.value.trim();
    if (message) {
      addMessage('user', message);
      chatInput.value = '';
      
      // Simulate AI response
      setTimeout(() => {
        addMessage('ai', getAIResponse(message));
      }, 1000);
    }
  });
  
  // Quick action buttons
  const quickActions = document.querySelectorAll('.btn-outline-primary, .btn-outline-warning, .btn-outline-success');
  quickActions.forEach(button => {
    button.addEventListener('click', function() {
      const action = this.textContent.trim();
      addMessage('user', action);
      
      setTimeout(() => {
        addMessage('ai', getQuickActionResponse(action));
      }, 800);
    });
  });
  
  // Navigation buttons
  const prevButton = document.querySelector('.btn-modern:not(.btn-next)');
  const nextButton = document.querySelector('.btn-next');
  
  if (prevButton) {
    prevButton.addEventListener('click', function() {
      showNotification('Önceki derse geçiliyor...');
    });
  }
  
  if (nextButton) {
    nextButton.addEventListener('click', function() {
      showNotification('Sonraki derse geçiliyor...');
    });
  }
});

// Add message to chat
function addMessage(type, text) {
  const chatMessages = document.querySelector('.chat-messages');
  const messageDiv = document.createElement('div');
  messageDiv.className = `message ${type}`;
  messageDiv.innerHTML = `<strong>${type === 'ai' ? 'AI' : 'Sen'}:</strong> ${text}`;
  chatMessages.appendChild(messageDiv);
  chatMessages.scrollTop = chatMessages.scrollHeight;
}

// Get AI response based on user message
function getAIResponse(message) {
  const responses = {
    'denklem': 'Denklemler matematikte çok önemli bir konudur. x + 5 = 12 gibi basit denklemlerden başlayarak karmaşık denklemlere kadar ilerleyebiliriz.',
    'kavram': 'Matematik kavramları temel olarak sayılar, işlemler ve problem çözme becerilerini içerir.',
    'örnek': 'İşte bir örnek: 2x + 3 = 11 denklemini çözmek için önce 3\'ü çıkarırız: 2x = 8, sonra 2\'ye böleriz: x = 4',
    'yardım': 'Size nasıl yardımcı olabilirim? Hangi konuda zorlanıyorsunuz?'
  };
  
  const lowerMessage = message.toLowerCase();
  for (let key in responses) {
    if (lowerMessage.includes(key)) {
      return responses[key];
    }
  }
  
  return 'Anladım! Bu konuda size yardımcı olmaya çalışacağım. Başka bir sorunuz var mı?';
}

// Get quick action response
function getQuickActionResponse(action) {
  const responses = {
    'Kavram Açıklaması': 'Bu kavram, matematikte temel bir yapı taşıdır. Detaylı açıklama için ders notlarına bakabilirsiniz.',
    'İpucu İste': 'İpucu: Bu tür problemlerde önce bilinmeyen değeri belirlemeye odaklanın.',
    'Çözümü Göster': 'Çözüm adım adım şu şekilde: 1) Denklemi yazın 2) Bilinmeyen değeri izole edin 3) Sonucu kontrol edin'
  };
  
  return responses[action] || 'Bu özellik henüz geliştirme aşamasında.';
}

// Show notification
function showNotification(message) {
  const notification = document.createElement('div');
  notification.className = 'alert alert-info alert-dismissible fade show position-fixed';
  notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
  notification.innerHTML = `
    ${message}
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  `;
  
  document.body.appendChild(notification);
  
  // Auto remove after 3 seconds
  setTimeout(() => {
    if (notification.parentNode) {
      notification.remove();
    }
  }, 3000);
}

// Video player enhancements
document.addEventListener('DOMContentLoaded', function() {
  const video = document.querySelector('video');
  if (video) {
    // Add custom controls if needed
    video.addEventListener('play', function() {
      console.log('Video başlatıldı');
    });
    
    video.addEventListener('pause', function() {
      console.log('Video duraklatıldı');
    });
    
    video.addEventListener('ended', function() {
      showNotification('Video tamamlandı! Sonraki derse geçebilirsiniz.');
    });
  }
});

// Progress tracking
function updateProgress(percentage) {
  const progressBar = document.querySelector('.progress-bar-modern');
  const progressBadge = document.querySelector('.badge');
  
  if (progressBar) {
    progressBar.style.width = percentage + '%';
  }
  
  if (progressBadge) {
    progressBadge.textContent = `%${percentage} Tamamlandı`;
  }
}

// Export functions for external use
window.startLessonUtils = {
  addMessage,
  updateProgress,
  showNotification
}; 