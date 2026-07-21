const Theme = {
    init() {
        const savedTheme = localStorage.getItem('app_theme') || 'light';
        if (savedTheme === 'dark') {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    },

    toggle() {
        const isDark = document.documentElement.classList.toggle('dark');
        localStorage.setItem('app_theme', isDark ? 'dark' : 'light');
    }
};

Theme.init();