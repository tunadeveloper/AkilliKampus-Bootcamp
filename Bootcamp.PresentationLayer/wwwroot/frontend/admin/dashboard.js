// Smooth scrolling and animations
document.addEventListener('DOMContentLoaded', function() {
  // Add fade-in animation to elements
  const elements = document.querySelectorAll('.admin-sidebar-modern, .admin-content-modern, .stat-card-modern');
  elements.forEach((el, index) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    setTimeout(() => {
      el.style.transition = 'all 0.6s ease';
      el.style.opacity = '1';
      el.style.transform = 'translateY(0)';
    }, index * 200);
  });
  
  // Table row hover effects (desktop only)
  const tableRows = document.querySelectorAll('.table-modern tbody tr');
  tableRows.forEach(row => {
    // Desktop hover effects
    row.addEventListener('mouseenter', function() {
      if (window.innerWidth > 768) {
        this.style.transform = 'scale(1.01)';
      }
    });
    
    row.addEventListener('mouseleave', function() {
      if (window.innerWidth > 768) {
        this.style.transform = 'scale(1)';
      }
    });
    
    // Mobile touch effects
    row.addEventListener('touchstart', function() {
      this.style.backgroundColor = 'rgba(111,66,193,0.1)';
    });
    
    row.addEventListener('touchend', function() {
      setTimeout(() => {
        this.style.backgroundColor = '';
      }, 200);
    });
  });
  
  // Sidebar navigation
  const navLinks = document.querySelectorAll('.nav-link-modern');
  navLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      e.preventDefault();
      navLinks.forEach(l => l.classList.remove('active'));
      this.classList.add('active');
      
      // Mobile: Close sidebar if open
      if (window.innerWidth <= 991) {
        closeSidebar();
      }
    });
  });
  
  // Sidebar Toggle Functionality
  const sidebarToggle = document.getElementById('sidebarToggle');
  const sidebarClose = document.getElementById('sidebarClose');
  const adminSidebar = document.getElementById('adminSidebar');
  const sidebarOverlay = document.getElementById('sidebarOverlay');
  
  function openSidebar() {
    adminSidebar.classList.add('sidebar-open');
    sidebarOverlay.classList.add('active');
    document.body.style.overflow = 'hidden';
  }
  
  function closeSidebar() {
    adminSidebar.classList.remove('sidebar-open');
    sidebarOverlay.classList.remove('active');
    document.body.style.overflow = '';
  }
  
  // Toggle sidebar on button click
  if (sidebarToggle) {
    sidebarToggle.addEventListener('click', function() {
      openSidebar();
    });
  }
  
  // Close sidebar on close button click
  if (sidebarClose) {
    sidebarClose.addEventListener('click', function() {
      closeSidebar();
    });
  }
  
  // Close sidebar on overlay click
  if (sidebarOverlay) {
    sidebarOverlay.addEventListener('click', function() {
      closeSidebar();
    });
  }
  
  // Close sidebar on escape key
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape') {
      closeSidebar();
    }
  });
  
  // Handle window resize
  window.addEventListener('resize', function() {
    if (window.innerWidth > 991) {
      closeSidebar();
    }
  });
  
  // Responsive table handling
  function handleResponsiveTable() {
    const table = document.querySelector('.table-modern');
    if (window.innerWidth <= 576) {
      // Add mobile-specific classes
      table.classList.add('table-mobile');
    } else {
      table.classList.remove('table-mobile');
    }
  }
  
  // Call on load and resize
  handleResponsiveTable();
  window.addEventListener('resize', handleResponsiveTable);
  
  // Touch-friendly button interactions
  const buttons = document.querySelectorAll('.btn');
  buttons.forEach(button => {
    button.addEventListener('touchstart', function() {
      this.style.transform = 'scale(0.95)';
    });
    
    button.addEventListener('touchend', function() {
      setTimeout(() => {
        this.style.transform = '';
      }, 150);
    });
  });
  
  // Prevent zoom on double tap (iOS)
  let lastTouchEnd = 0;
  document.addEventListener('touchend', function(event) {
    const now = (new Date()).getTime();
    if (now - lastTouchEnd <= 300) {
      event.preventDefault();
    }
    lastTouchEnd = now;
  }, false);
  
  // Handle orientation change
  window.addEventListener('orientationchange', function() {
    setTimeout(() => {
      handleResponsiveTable();
      // Close sidebar on orientation change
      if (window.innerWidth <= 991) {
        closeSidebar();
      }
    }, 100);
  });
  
  // Add smooth scroll to sidebar links
  const sidebarLinks = document.querySelectorAll('.admin-sidebar-modern .nav-link');
  sidebarLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      // Add smooth scroll effect
      this.style.transform = 'scale(0.98)';
      setTimeout(() => {
        this.style.transform = '';
      }, 150);
    });
  });
});

// Export functions for external use
window.dashboardUtils = {
  // Function to refresh stats
  refreshStats: function() {
    console.log('Stats refreshed');
    // Add loading animation
    const statCards = document.querySelectorAll('.stat-card-modern');
    statCards.forEach(card => {
      card.style.opacity = '0.7';
      setTimeout(() => {
        card.style.opacity = '1';
      }, 500);
    });
  },
  
  // Function to add new education
  addNewEducation: function() {
    console.log('New education added');
    // Show success message
    showNotification('Yeni eğitim başarıyla eklendi!', 'success');
  },
  
  // Function to edit education
  editEducation: function(id) {
    console.log('Editing education with ID:', id);
    showNotification('Eğitim düzenleme modu açıldı', 'info');
  },
  
  // Function to delete education
  deleteEducation: function(id) {
    console.log('Deleting education with ID:', id);
    if (confirm('Bu eğitimi silmek istediğinizden emin misiniz?')) {
      showNotification('Eğitim başarıyla silindi!', 'success');
    }
  },
  
  // Mobile-friendly notification system
  showNotification: function(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} notification-toast`;
    notification.style.cssText = `
      position: fixed;
      top: 20px;
      right: 20px;
      z-index: 9999;
      max-width: 300px;
      border-radius: 10px;
      box-shadow: 0 10px 30px rgba(0,0,0,0.2);
      animation: slideIn 0.3s ease;
    `;
    notification.textContent = message;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
      notification.style.animation = 'slideOut 0.3s ease';
      setTimeout(() => {
        document.body.removeChild(notification);
      }, 300);
    }, 3000);
  },
  
  // Toggle sidebar function
  toggleSidebar: function() {
    const adminSidebar = document.getElementById('adminSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    if (adminSidebar.classList.contains('sidebar-open')) {
      adminSidebar.classList.remove('sidebar-open');
      sidebarOverlay.classList.remove('active');
      document.body.style.overflow = '';
    } else {
      adminSidebar.classList.add('sidebar-open');
      sidebarOverlay.classList.add('active');
      document.body.style.overflow = 'hidden';
    }
  },
  
  // Open sidebar function
  openSidebar: function() {
    const adminSidebar = document.getElementById('adminSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    adminSidebar.classList.add('sidebar-open');
    sidebarOverlay.classList.add('active');
    document.body.style.overflow = 'hidden';
  },
  
  // Close sidebar function
  closeSidebar: function() {
    const adminSidebar = document.getElementById('adminSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    adminSidebar.classList.remove('sidebar-open');
    sidebarOverlay.classList.remove('active');
    document.body.style.overflow = '';
  }
};

// Add CSS animations for notifications and sidebar
const style = document.createElement('style');
style.textContent = `
  @keyframes slideIn {
    from { transform: translateX(100%); opacity: 0; }
    to { transform: translateX(0); opacity: 1; }
  }
  
  @keyframes slideOut {
    from { transform: translateX(0); opacity: 1; }
    to { transform: translateX(100%); opacity: 0; }
  }
  
  @keyframes slideInLeft {
    from { 
      transform: translateX(-100%); 
      opacity: 0; 
    }
    to { 
      transform: translateX(0); 
      opacity: 1; 
    }
  }
  
  @keyframes slideOutLeft {
    from { 
      transform: translateX(0); 
      opacity: 1; 
    }
    to { 
      transform: translateX(-100%); 
      opacity: 0; 
    }
  }
  
  @media (max-width: 768px) {
    .notification-toast {
      right: 10px !important;
      left: 10px !important;
      max-width: none !important;
    }
  }
  
  /* Sidebar animations */
  .admin-sidebar-modern {
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  }
  
  .sidebar-overlay {
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  }
`;
document.head.appendChild(style); 