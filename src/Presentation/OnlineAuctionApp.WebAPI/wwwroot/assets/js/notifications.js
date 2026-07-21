const Notifications = {
    connection: null,

    async init() {
        if (typeof Auth !== 'undefined' && !Auth.currentUser) {
            return;
        }

        this.loadUnreadCount();
        this.setupSignalR();
    },

    async loadUnreadCount() {
        try {
            const list = await API.get('/api/Notifications/my');
            const unread = list.filter(n => !n.isRead).length;
            const badge = document.getElementById('unread-count-badge');
            if (badge) {
                if (unread > 0) {
                    badge.innerText = unread;
                    badge.classList.remove('hidden');
                } else {
                    badge.classList.add('hidden');
                }
            }
        } catch (e) {
            console.error('Failed to load notifications count', e);
        }
    },

    setupSignalR() {
        if (typeof signalR === 'undefined') return;

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/notifications')
            .withAutomaticReconnect()
            .build();

        this.connection.on('ReceiveNotification', (notification) => {
            Common.showToast(notification.message || 'Yeni bildirişiniz var!', 'info');
            this.loadUnreadCount();
        });

        this.connection.start().catch(err => console.log('SignalR connection error: ', err));
    }
};