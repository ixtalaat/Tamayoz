// Tamayoz Academy - Client Scripts
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
});

