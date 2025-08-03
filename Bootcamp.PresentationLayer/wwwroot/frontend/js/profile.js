// Profile Page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Floating action button functionality
    const floatingBtn = document.querySelector('.floating-action-btn');
    if (floatingBtn) {
        floatingBtn.addEventListener('click', function() {
            // Quick access menu could be implemented here
            alert('Hızlı erişim menüsü yakında!');
        });
    }

    // Add smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // Profile avatar hover effect
    const profileAvatar = document.querySelector('.profile-avatar');
    if (profileAvatar) {
        profileAvatar.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.05)';
        });
        
        profileAvatar.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    }

    // Modern card hover effects
    const modernCards = document.querySelectorAll('.modern-card');
    modernCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // Lesson item hover effects
    const lessonItems = document.querySelectorAll('.lesson-item');
    lessonItems.forEach(item => {
        item.addEventListener('mouseenter', function() {
            this.style.transform = 'translateX(5px)';
        });
        
        item.addEventListener('mouseleave', function() {
            this.style.transform = 'translateX(0)';
        });
    });

    // Progress bar animation
    const progressBars = document.querySelectorAll('.progress-modern .progress-bar');
    progressBars.forEach(bar => {
        const width = bar.style.width;
        bar.style.width = '0%';
        
        setTimeout(() => {
            bar.style.transition = 'width 1s ease-in-out';
            bar.style.width = width;
        }, 500);
    });

    // Stat number counter animation
    const statNumbers = document.querySelectorAll('.stat-number');
    statNumbers.forEach(stat => {
        const finalValue = stat.textContent;
        const isPercentage = finalValue.includes('%');
        const isNumber = !isNaN(parseInt(finalValue));
        
        if (isNumber) {
            const targetValue = parseInt(finalValue);
            let currentValue = 0;
            const increment = targetValue / 50; // 50 steps
            
            const counter = setInterval(() => {
                currentValue += increment;
                if (currentValue >= targetValue) {
                    currentValue = targetValue;
                    clearInterval(counter);
                }
                stat.textContent = isPercentage ? Math.floor(currentValue) + '%' : Math.floor(currentValue);
            }, 20);
        }
    });

    // Add loading animation for page elements
    const animatedElements = document.querySelectorAll('.animate-fadein, .animate-fadein-delay1, .animate-fadein-delay2');
    animatedElements.forEach((element, index) => {
        element.style.opacity = '0';
        element.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            element.style.transition = 'opacity 0.8s ease-in-out, transform 0.8s ease-in-out';
            element.style.opacity = '1';
            element.style.transform = 'translateY(0)';
        }, index * 200);
    });

    // Add scroll-based animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe all cards for scroll animations
    const cards = document.querySelectorAll('.modern-card, .lesson-item');
    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.6s ease-in-out, transform 0.6s ease-in-out';
        observer.observe(card);
    });
});

// Utility functions
function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

// Export functions for global use
window.ProfileUtils = {
    showNotification
}; 