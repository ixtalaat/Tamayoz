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



// 4. Message Reply Modal Handler
document.addEventListener('DOMContentLoaded', function () {
    const modalEl = document.getElementById('replyMessageModal');
    if (!modalEl) return;

    let replyModalInstance = null;
    let currentTargetEmail = '';

    // Delegate click on reply buttons
    document.addEventListener('click', function (e) {
        const btn = e.target.closest('[data-reply-email]');
        if (!btn) return;

        e.preventDefault();

        const name = btn.getAttribute('data-reply-name') || 'الزائر';
        const email = btn.getAttribute('data-reply-email') || '';
        const phone = btn.getAttribute('data-reply-phone') || '';
        const date = btn.getAttribute('data-reply-date') || '';
        const message = btn.getAttribute('data-reply-message') || '';

        currentTargetEmail = email;

        // Set text
        const senderNameEl = document.getElementById('modalSenderName');
        const senderEmailEl = document.getElementById('modalSenderEmail');
        const messageDateEl = document.getElementById('modalMessageDate');
        const messageTextEl = document.getElementById('modalMessageText');

        if (senderNameEl) senderNameEl.textContent = name;
        if (senderEmailEl) senderEmailEl.textContent = email;
        if (messageDateEl) messageDateEl.textContent = date;
        if (messageTextEl) messageTextEl.textContent = message;

        const subject = `الرد على استفسارك في أكاديمية التميز`;
        const emailBody = `مرحبًا ${name}،\n\nبخصوص رسالتك:\n"${message}"\n\nنود إفادتك بـ: \n\nمع تحيات فريق أكاديمية التميز للخدمات الطلابية.\nhttps://tamayoz-academy.com`;

        // Gmail link
        const gmailLink = document.getElementById('modalGmailLink');
        if (gmailLink) {
            gmailLink.href = `https://mail.google.com/mail/?view=cm&fs=1&to=${encodeURIComponent(email)}&su=${encodeURIComponent(subject)}&body=${encodeURIComponent(emailBody)}`;
        }

        // Outlook link
        const outlookLink = document.getElementById('modalOutlookLink');
        if (outlookLink) {
            outlookLink.href = `https://outlook.live.com/mail/0/deeplink/compose?to=${encodeURIComponent(email)}&subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(emailBody)}`;
        }

        // Mailto link
        const mailtoLink = document.getElementById('modalMailtoLink');
        if (mailtoLink) {
            mailtoLink.href = `mailto:${encodeURIComponent(email)}?subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(emailBody)}`;
        }

        // WhatsApp link
        const waLink = document.getElementById('modalWhatsAppLink');
        const waPhoneEl = document.getElementById('modalWhatsAppPhone');
        if (waLink) {
            if (phone && phone.trim().length > 5) {
                let cleanPhone = phone.replace(/\s+/g, '').replace(/-/g, '').replace(/\+/g, '');
                if (cleanPhone.startsWith('01') && cleanPhone.length === 11) {
                    cleanPhone = '20' + cleanPhone.substring(1);
                } else if (!cleanPhone.startsWith('20') && cleanPhone.length === 10) {
                    cleanPhone = '20' + cleanPhone;
                }
                const waText = `مرحبًا ${name}، نتواصل معك من أكاديمية التميز بخصوص استفسارك...`;
                waLink.href = `https://wa.me/${cleanPhone}?text=${encodeURIComponent(waText)}`;
                if (waPhoneEl) waPhoneEl.textContent = phone;
                waLink.classList.remove('d-none');
            } else {
                waLink.classList.add('d-none');
            }
        }

        // Show modal
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            replyModalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
            replyModalInstance.show();
        }
    });

    // Copy email button
    const copyBtn = document.getElementById('modalCopyEmailBtn');
    if (copyBtn) {
        copyBtn.addEventListener('click', function () {
            if (!currentTargetEmail) return;
            navigator.clipboard.writeText(currentTargetEmail).then(function () {
                if (typeof toastr !== 'undefined') {
                    toastr.success(`تم نسخ البريد الإلكتروني (${currentTargetEmail}) إلى الحافظة بنجاح`, 'تم النسخ ✓');
                } else {
                    alert('تم نسخ البريد بنجاح');
                }
            }).catch(function () {
                prompt('انسخ البريد الإلكتروني:', currentTargetEmail);
            });
        });
    }
});
