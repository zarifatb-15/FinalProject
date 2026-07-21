const Auth = {
    currentUser: null,

    async init() {
        try {
            this.currentUser = await API.get('/api/Auth/me');
            return this.currentUser;
        } catch (err) {
            this.currentUser = null;
            return null;
        }
    },

    async login(usernameOrEmail, password) {
        await API.post('/api/Auth/login', { usernameOrEmail, password });
        return await this.init();
    },

    async register(userData) {
        return await API.post('/api/Auth/register', userData);
    },

    async logout() {
        try {
            await API.post('/api/Auth/logout');
        } catch (e) {
            console.error('Logout failed:', e);
        } finally {
            this.currentUser = null;
            window.location.href = 'login.html';
        }
    },

    hasRole(role) {
        if (!this.currentUser || !this.currentUser.roles) return false;
        return this.currentUser.roles.includes(role);
    },

    async requireAuth(allowedRoles = []) {
        const user = await this.init();
        if (!user) {
            window.location.href = 'login.html';
            return null;
        }
        if (allowedRoles.length > 0) {
            const hasAccess = allowedRoles.some(r => this.hasRole(r));
            if (!hasAccess) {
                window.location.href = 'unauthorized.html';
                return null;
            }
        }
        return user;
    }
};