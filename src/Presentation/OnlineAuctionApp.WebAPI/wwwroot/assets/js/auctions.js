const AuctionsPage = {
    items: [],

    async init() {
        await this.loadCategories();
        await this.loadAuctions();
        setInterval(() => this.updateTimers(), 1000);
    },

    async loadCategories() {
        try {
            const categories = await API.get('/api/Categories');
            const select = document.getElementById('category-filter');
            if (!select) return;
            categories.forEach(c => {
                const opt = document.createElement('option');
                opt.value = c.id;
                opt.textContent = c.name;
                select.appendChild(opt);
            });
        } catch (e) {
            console.error(e);
        }
    },

    async loadAuctions() {
        const search = document.getElementById('search-input')?.value || '';
        const categoryId = document.getElementById('category-filter')?.value || '';
        const minPrice = document.getElementById('min-price')?.value || '';
        const maxPrice = document.getElementById('max-price')?.value || '';

        let query = [];
        if (search) query.push(`Search=${encodeURIComponent(search)}`);
        if (categoryId) query.push(`CategoryId=${categoryId}`);
        if (minPrice) query.push(`MinPrice=${minPrice}`);
        if (maxPrice) query.push(`MaxPrice=${maxPrice}`);

        const queryString = query.length > 0 ? '?' + query.join('&') : '';

        try {
            this.items = await API.get('/api/Auctions' + queryString);
            this.render();
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    render() {
        const grid = document.getElementById('auctions-grid');
        if (!grid) return;

        if (this.items.length === 0) {
            grid.innerHTML = `
                <div class="col-span-full py-16 text-center text-gray-500">
                    <i class="fa-solid fa-box-open text-5xl mb-4 text-gray-300"></i>
                    <p class="text-lg font-medium">Axtarışınıza uyğun hərrac tapılmadı.</p>
                </div>
            `;
            return;
        }

        grid.innerHTML = this.items.map(a => {
            const img = (a.images && a.images.length > 0) 
                ? (a.images.find(i => i.isPrimary)?.imageUrl || a.images[0].imageUrl)
                : 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=600';

            return `
                <div class="bg-white rounded-2xl border border-gray-100 shadow-sm hover:shadow-xl transition-all duration-300 flex flex-col overflow-hidden group">
                    <div class="relative h-48 bg-gray-100 overflow-hidden">
                        <img src="${img}" alt="${a.title}" class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500">
                        <span class="absolute top-3 left-3 bg-white/90 backdrop-blur-md text-gray-800 text-xs px-3 py-1 rounded-full font-semibold shadow-sm">
                            ${a.categoryName || 'General'}
                        </span>
                    </div>
                    <div class="p-5 flex-1 flex flex-col justify-between space-y-4">
                        <div>
                            <h3 class="font-bold text-gray-900 text-lg line-clamp-1 group-hover:text-indigo-600 transition">${a.title}</h3>
                            <p class="text-gray-500 text-sm line-clamp-2 mt-1">${a.description}</p>
                        </div>
                        <div class="border-t pt-4 border-gray-50 space-y-3">
                            <div class="flex justify-between items-end">
                                <div>
                                    <span class="text-xs text-gray-400 font-medium block">Cari Qiymət</span>
                                    <span class="text-xl font-black text-indigo-600">${Common.formatCurrency(a.currentPrice || a.startingPrice)}</span>
                                </div>
                                <div class="text-right">
                                    <span class="text-xs text-gray-400 font-medium block">Qalan Vaxt</span>
                                    <span class="auction-timer text-xs font-bold text-amber-600 bg-amber-50 px-2 py-1 rounded-md" data-endtime="${a.endTime}">
                                        Hesablanır...
                                    </span>
                                </div>
                            </div>
                            <a href="auction-details.html?id=${a.id}" class="block w-full text-center bg-gray-900 text-white font-medium py-2.5 rounded-xl hover:bg-indigo-600 transition">
                                Detallara Bax
                            </a>
                        </div>
                    </div>
                </div>
            `;
        }).join('');

        this.updateTimers();
    },

    updateTimers() {
        document.querySelectorAll('.auction-timer').forEach(el => {
            const end = el.getAttribute('data-endtime');
            const cd = Common.formatCountdown(end);
            el.textContent = cd.text;
            if (cd.isEnded) {
                el.className = 'auction-timer text-xs font-bold text-red-600 bg-red-50 px-2 py-1 rounded-md';
            }
        });
    }
};