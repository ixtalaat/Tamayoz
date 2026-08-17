// Tamayoz Academy - Client Scripts & Notification Handlers

// 1. Toastr Default RTL Arabic Configuration
if (typeof toastr !== 'undefined') {
    toastr.options = {
        closeButton: true,
        debug: false,
        newestOnTop: true,
        progressBar: true,
        positionClass: "toast-top-left",
        preventDuplicates: true,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "4500",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut",
        rtl: true
    };
}

// 2. SweetAlert2 Global Helper Functions
window.showSwalToast = function (title, icon = 'success') {
    if (typeof Swal === 'undefined') return;
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3500,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
        }
    });
    return Toast.fire({ icon, title });
};

window.showSuccessAlert = function (title, text = '') {
    if (typeof Swal === 'undefined') return;
    return Swal.fire({
        icon: 'success',
        title: title,
        text: text,
        confirmButtonText: 'حسنًا',
        confirmButtonColor: '#175f55'
    });
};

window.showErrorAlert = function (title, text = '') {
    if (typeof Swal === 'undefined') return;
    return Swal.fire({
        icon: 'error',
        title: title,
        text: text,
        confirmButtonText: 'موافق',
        confirmButtonColor: '#dc3545'
    });
};

window.confirmSwal = function (options = {}) {
    if (typeof Swal === 'undefined') {
        return Promise.resolve({ isConfirmed: window.confirm(options.text || options.title || 'هل أنت متأكد؟') });
    }

    return Swal.fire({
        title: options.title || 'هل أنت متأكد؟',
        text: options.text || '',
        icon: options.icon || 'warning',
        showCancelButton: true,
        confirmButtonColor: options.confirmColor || '#dc3545',
        cancelButtonColor: options.cancelColor || '#6c757d',
        confirmButtonText: options.confirmText || 'نعم، استمر',
        cancelButtonText: options.cancelText || 'إلغاء',
        reverseButtons: true
    });
};

// 3. Global DOM Ready Event Listeners
document.addEventListener('DOMContentLoaded', function () {
    // Back to top smooth scroll
    const backToTopBtn = document.getElementById('backToTopBtn');
    if (backToTopBtn) {
        backToTopBtn.addEventListener('click', function (e) {
            e.preventDefault();
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    // Intercept all elements with [data-confirm] using SweetAlert2
    document.addEventListener('click', function (e) {
        const trigger = e.target.closest('[data-confirm]');
        if (!trigger) return;

        e.preventDefault();
        const message = trigger.getAttribute('data-confirm') || 'هل أنت متأكد من تنفيذ هذا الإجراء؟';
        const title = trigger.getAttribute('data-confirm-title') || 'تأكيد الإجراء';
        const confirmBtn = trigger.getAttribute('data-confirm-btn') || 'نعم، استمر';
        const form = trigger.closest('form');

        window.confirmSwal({
            title: title,
            text: message,
            icon: 'warning',
            confirmText: confirmBtn,
            cancelText: 'إلغاء',
            confirmColor: trigger.classList.contains('btn-danger') || trigger.classList.contains('btn-outline-danger') ? '#dc3545' : '#175f55'
        }).then((result) => {
            if (result.isConfirmed) {
                if (form) {
                    form.submit();
                } else if (trigger.tagName === 'A' && trigger.href) {
                    window.location.href = trigger.href;
                }
            }
        });
    });
});


