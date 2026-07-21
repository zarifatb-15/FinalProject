const AdminDashboard = {
    users: [],
    auctions: [],
    categories: [],
    editingCategoryId: null,
    charts: {},

    async init() {
        const user = await Auth.requireAuth(['Admin']);
        if (!user && !Auth.currentUser) return;

        this.renderAdminIdentity(user || Auth.currentUser);
        this.injectAdminOperationsPanel();
        this.setupSearch();

        await this.loadDashboard();
        await this.loadUsers();
        await this.loadAuctions();
        await this.loadCategories();
        await this.loadNotifications();
    },

    renderAdminIdentity(user) {
        const username = user?.username || user?.email || 'Admin';
        const avatar = document.getElementById('adminAvatar');
        const name = document.getElementById('adminName');

        if (avatar) avatar.textContent = username.charAt(0).toUpperCase();
        if (name) name.textContent = username;
    },

    injectAdminOperationsPanel() {
        const grid = document.querySelector('main .grid');
        if (!grid || document.getElementById('admin-category-panel')) return;

        const panel = document.createElement('div');
        panel.id = 'admin-category-panel';
        panel.className = 'lg:col-span-12 bg-panelBg rounded-xl p-5 shadow-lg border border-gray-800/50';

        panel.innerHTML = `
            <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4 mb-5">
                <div>
                    <h3 class="text-white font-bold tracking-wide">Kateqoriyalar</h3>
                    <p class="text-sm text-gray-500 mt-1">Hərrac kateqoriyalarını yaradın, yeniləyin və idarə edin.</p>
                </div>

                <div class="flex flex-col md:flex-row gap-3">
                    <input id="admin-category-name" type="text" placeholder="Yeni kateqoriya"
                        class="bg-gray-800/50 border border-gray-700 rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-brandBlue">

                    <input id="admin-category-description" type="text" placeholder="Qısa açıqlama"
                        class="bg-gray-800/50 border border-gray-700 rounded-lg px-3 py-2 text-sm text-white focus:outline-none focus:border-brandBlue">

                    <button id="admin-category-save-btn"
                        class="bg-brandBlue/20 text-brandBlue px-4 py-2 rounded-lg text-sm font-semibold hover:bg-brandBlue/30 transition">
                        Əlavə Et
                    </button>

                    <button id="admin-category-reset-btn"
                        class="hidden bg-gray-700/60 text-gray-200 px-4 py-2 rounded-lg text-sm font-semibold hover:bg-gray-700 transition">
                        Ləğv
                    </button>
                </div>
            </div>

            <div class="overflow-x-auto">
                <table class="w-full text-left">
                    <thead class="text-gray-500 uppercase text-xs">
                        <tr>
                            <th class="py-3 px-4">Ad</th>
                            <th class="py-3 px-4">Qısa açıqlama</th>
                            <th class="py-3 px-4 text-right">Əməliyyatlar</th>
                        </tr>
                    </thead>
                    <tbody id="admin-categories-table"></tbody>
                </table>
            </div>
        `;

        grid.appendChild(panel);

        document.getElementById('admin-category-save-btn')
            ?.addEventListener('click', () => this.saveCategory());

        document.getElementById('admin-category-reset-btn')
            ?.addEventListener('click', () => this.resetCategoryForm());
    },

    setupSearch() {
        const input = document.querySelector('header input[type="text"]');
        if (!input) return;

        input.addEventListener('input', () => {
            const term = input.value.trim().toLowerCase();

            document.querySelectorAll('#users-table tr, #admin-auctions-table tr, #admin-categories-table tr')
                .forEach(row => {
                    row.style.display = row.innerText.toLowerCase().includes(term) ? '' : 'none';
                });
        });
    },

    async loadDashboard() {
        try {
            const data = await API.get('/api/Admin/dashboard');
            const s = data.summary || data || {};

            document.getElementById('total-auctions').innerText =
                this.formatNumber(s.totalAuctions ?? s.totalAuctionCount ?? s.auctionCount ?? 0);

            document.getElementById('total-users').innerText =
                this.formatNumber(s.totalUsers ?? s.totalUserCount ?? s.userCount ?? 0);

            document.getElementById('total-bids').innerText =
                this.formatNumber(s.totalBids ?? s.totalBidCount ?? s.bidCount ?? 0);

            document.getElementById('total-categories').innerText =
                this.formatNumber(s.totalCategories ?? s.totalCategoryCount ?? s.categoryCount ?? 0);

            this.renderCharts(s);
        } catch (err) {
            Common.showToast(err.errors || 'Dashboard məlumatları yüklənə bilmədi', 'error');
        }
    },

    formatNumber(num) {
        if (num === null || num === undefined) return '0';
        return Number(num).toLocaleString();
    },

    destroyChart(id) {
        if (this.charts[id]) {
            this.charts[id].destroy();
            delete this.charts[id];
        }
    },

    renderCharts(s) {
        Chart.defaults.color = '#6b7280';
        Chart.defaults.font.family = 'sans-serif';

        const active = s.activeAuctions ?? s.activeAuctionCount ?? 0;
        const completed = s.completedAuctions ?? s.completedAuctionCount ?? 0;
        const cancelled = s.cancelledAuctions ?? s.cancelledAuctionCount ?? 0;
        const users = s.totalUsers ?? s.totalUserCount ?? 0;
        const bids = s.totalBids ?? s.totalBidCount ?? 0;
        const categories = s.totalCategories ?? s.totalCategoryCount ?? 0;

        if (document.getElementById('salesChart')) {
            this.destroyChart('salesChart');
            this.charts.salesChart = new Chart(document.getElementById('salesChart'), {
                type: 'line',
                data: {
                    labels: ['Aktiv', 'Bitmiş', 'Ləğv', 'User', 'Bid', 'Kateq.'],
                    datasets: [{
                        label: 'Platform',
                        data: [active, completed, cancelled, users, bids, categories],
                        borderColor: '#00c8ff',
                        tension: 0.4,
                        borderWidth: 3,
                        pointRadius: 3
                    }, {
                        label: 'Hərrac',
                        data: [active, completed, cancelled, active + completed, bids, categories],
                        borderColor: '#ff007b',
                        tension: 0.4,
                        borderWidth: 3,
                        pointRadius: 3
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: { legend: { display: false } },
                    scales: {
                        x: { grid: { display: false, drawBorder: false } },
                        y: { beginAtZero: true, grid: { color: '#374151' } }
                    }
                }
            });
        }

        if (document.getElementById('orderChart')) {
            this.destroyChart('orderChart');
            this.charts.orderChart = new Chart(document.getElementById('orderChart'), {
                type: 'bar',
                data: {
                    labels: ['Aktiv', 'Bitmiş', 'Ləğv', 'Kateq.', 'User', 'Bid'],
                    datasets: [{
                        data: [active, completed, cancelled, categories, users, bids],
                        backgroundColor: '#ff007b',
                        borderRadius: 20,
                        barThickness: 8
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: { legend: { display: false } },
                    scales: {
                        x: { grid: { display: false } },
                        y: { beginAtZero: true, grid: { color: '#374151' } }
                    }
                }
            });
        }

        if (document.getElementById('donutChart')) {
            this.destroyChart('donutChart');
            this.charts.donutChart = new Chart(document.getElementById('donutChart'), {
                type: 'doughnut',
                data: {
                    labels: ['Aktiv', 'Bitmiş', 'Ləğv'],
                    datasets: [{
                        data: [active, completed, cancelled],
                        backgroundColor: ['#10b981', '#00c8ff', '#ff007b'],
                        borderWidth: 3,
                        borderColor: '#1e1e2d'
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    cutout: '75%',
                    plugins: { legend: { display: false } }
                }
            });
        }
    },

    async loadUsers() {
        try {
            this.users = await API.get('/api/Admin/users');
            const tbody = document.getElementById('users-table');
            if (!tbody) return;

            if (!this.users || this.users.length === 0) {
                tbody.innerHTML = `<tr><td colspan="3" class="text-center py-8 text-gray-400">İstifadəçi tapılmadı.</td></tr>`;
                return;
            }

            tbody.innerHTML = this.users.map(u => {
                const roles = Array.isArray(u.roles) ? u.roles : (u.role ? [u.role] : ['User']);

                return `
                    <tr class="border-b border-gray-800/50 hover:bg-gray-800/20 transition">
                        <td class="py-3 px-4 font-medium text-gray-200">${u.username || u.userName || '-'}</td>
                        <td class="py-3 px-4 text-gray-400">${u.email || '-'}</td>
                        <td class="py-3 px-4">
                            <span class="px-2 py-1 text-xs font-semibold rounded-md bg-indigo-500/20 text-indigo-400">
                                ${roles.join(', ')}
                            </span>
                        </td>
                    </tr>
                `;
            }).join('');
        } catch (e) {
            Common.showToast(e.errors || 'İstifadəçilər yüklənə bilmədi', 'error');
        }
    },

    async loadAuctions() {
        try {
            this.auctions = await API.get('/api/Admin/auctions');
            const tbody = document.getElementById('admin-auctions-table');
            if (!tbody) return;

            if (!this.auctions || this.auctions.length === 0) {
                tbody.innerHTML = `<tr><td colspan="4" class="text-center py-8 text-gray-400">Hərrac tapılmadı.</td></tr>`;
                return;
            }

            tbody.innerHTML = this.auctions.map(a => {
                const status = a.status || '-';
                const isActive = status === 'Active';

                return `
                    <tr class="border-b border-gray-800/50 hover:bg-gray-800/20 transition">
                        <td class="py-3 px-4 font-medium text-gray-200">
                            <button onclick="AdminDashboard.openAuction('${a.id}')" class="hover:text-brandBlue transition">
                                ${a.title || '-'}
                            </button>
                        </td>
                        <td class="py-3 px-4 text-gray-400">${Common.formatCurrency(a.currentPrice || a.startingPrice || 0)}</td>
                        <td class="py-3 px-4">
                            <span class="px-2 py-1 text-xs font-semibold rounded-full ${
                                status === 'Active' ? 'bg-emerald-500/20 text-emerald-400' :
                                status === 'Completed' ? 'bg-blue-500/20 text-blue-400' :
                                'bg-red-500/20 text-red-400'
                            }">${status}</span>
                        </td>
                        <td class="py-3 px-4 text-right">
                            ${isActive ? `
                                <button onclick="AdminDashboard.cancelAuction('${a.id}')" class="bg-red-500/20 text-red-400 px-3 py-1 rounded-md text-xs font-semibold hover:bg-red-500/30 transition">
                                    Ləğv Et
                                </button>
                            ` : `<span class="text-xs text-gray-500">—</span>`}
                        </td>
                    </tr>
                `;
            }).join('');
        } catch (e) {
            Common.showToast(e.errors || 'Hərraclar yüklənə bilmədi', 'error');
        }
    },

    async loadCategories() {
        try {
            this.categories = await API.get('/api/Categories');
            this.renderCategories();
        } catch (e) {
            Common.showToast(e.errors || 'Kateqoriyalar yüklənə bilmədi', 'error');
        }
    },

    renderCategories() {
        const tbody = document.getElementById('admin-categories-table');
        if (!tbody) return;

        if (!this.categories || this.categories.length === 0) {
            tbody.innerHTML = `<tr><td colspan="3" class="text-center py-8 text-gray-400">Kateqoriya yoxdur.</td></tr>`;
            return;
        }

        tbody.innerHTML = this.categories.map(c => `
            <tr class="border-b border-gray-800/50 hover:bg-gray-800/20 transition">
                <td class="py-3 px-4 font-medium text-gray-200">${c.name || '-'}</td>
                <td class="py-3 px-4 text-gray-400">${c.description || '-'}</td>
                <td class="py-3 px-4 text-right space-x-2">
                    <button data-action="edit-category" data-id="${c.id}" class="bg-brandBlue/20 text-brandBlue px-3 py-1 rounded-md text-xs font-semibold hover:bg-brandBlue/30 transition">
                        Redaktə
                    </button>
                    <button data-action="delete-category" data-id="${c.id}" class="bg-red-500/20 text-red-400 px-3 py-1 rounded-md text-xs font-semibold hover:bg-red-500/30 transition">
                        Sil
                    </button>
                </td>
            </tr>
        `).join('');

        tbody.querySelectorAll('[data-action="edit-category"]').forEach(btn => {
            btn.addEventListener('click', () => this.startEditCategory(btn.dataset.id));
        });

        tbody.querySelectorAll('[data-action="delete-category"]').forEach(btn => {
            btn.addEventListener('click', () => this.deleteCategory(btn.dataset.id));
        });
    },

    startEditCategory(id) {
        const category = this.categories.find(c => c.id === id);
        if (!category) return;

        this.editingCategoryId = id;

        document.getElementById('admin-category-name').value = category.name || '';
        document.getElementById('admin-category-description').value = category.description || '';
        document.getElementById('admin-category-save-btn').textContent = 'Yenilə';
        document.getElementById('admin-category-reset-btn').classList.remove('hidden');
    },

    resetCategoryForm() {
        this.editingCategoryId = null;

        document.getElementById('admin-category-name').value = '';
        document.getElementById('admin-category-description').value = '';
        document.getElementById('admin-category-save-btn').textContent = 'Əlavə Et';
        document.getElementById('admin-category-reset-btn').classList.add('hidden');
    },

    async saveCategory() {
        const name = document.getElementById('admin-category-name').value.trim();
        const description = document.getElementById('admin-category-description').value.trim();

        if (!name) {
            Common.showToast('Yeni kateqoriya boş ola bilməz', 'warning');
            return;
        }

        const body = { name, description };

        try {
            if (this.editingCategoryId) {
                await API.put(`/api/Categories/${this.editingCategoryId}`, body);
                Common.showToast('Kateqoriya yeniləndi', 'success');
            } else {
                await API.post('/api/Categories', body);
                Common.showToast('Kateqoriya əlavə olundu', 'success');
            }

            this.resetCategoryForm();
            await this.loadCategories();
            await this.loadDashboard();
        } catch (e) {
            Common.showToast(e.errors || 'Kateqoriya əməliyyatı alınmadı', 'error');
        }
    },

    async deleteCategory(id) {
        if (!confirm('Bu kateqoriyanı silmək istədiyinizə əminsiniz?')) return;

        try {
            await API.delete(`/api/Categories/${id}`);
            Common.showToast('Kateqoriya silindi', 'success');
            await this.loadCategories();
            await this.loadDashboard();
        } catch (e) {
            Common.showToast(e.errors || 'Kateqoriya silinə bilmədi. Bu kateqoriyaya bağlı hərrac ola bilər.', 'error');
        }
    },

    openAuction(id) {
        if (!id) return;
        window.location.href = `auction-details.html?id=${id}`;
    },

    async cancelAuction(id) {
        if (!confirm('Admin tərəfindən bu hərracı ləğv etmək istədiyinizə əminsiniz?')) return;

        try {
            await API.patch(`/api/Admin/auctions/${id}/cancel`);
            Common.showToast('Hərrac ləğv olundu', 'success');
            await this.loadDashboard();
            await this.loadAuctions();
        } catch (err) {
            Common.showToast(err.errors || 'Hərrac ləğv edilə bilmədi', 'error');
        }
    },

    async loadNotifications() {
        try {
            const list = await API.get('/api/Notifications/my');
            const unread = (list || []).filter(n => !n.isRead).length;
            const badge = document.querySelector('header a[href="notifications.html"] span');

            if (badge) {
                badge.textContent = unread;
                badge.classList.toggle('hidden', unread === 0);
            }
        } catch (e) {
            // Admin panel notification xətasına görə qırılmasın
        }
    }
};
