export function initializeAnimations(dotNetRef) {
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const element = entry.target;
                const animation = element.getAttribute('data-animation');

                if (animation) {
                    element.classList.add(animation);

                    observer.unobserve(element);

                    if (element.classList.contains('animate__animated')) {
                        element.addEventListener('animationend', () => {
                            dotNetRef.invokeMethodAsync('OnAnimationComplete', element.id);
                        });
                    }
                }
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    });

    document.querySelectorAll('[data-animation]').forEach(el => {
        observer.observe(el);
    });

    return {
        dispose: () => {
            observer.disconnect();
        }
    };
}