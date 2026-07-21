const SellerDashboard = {
    async init() {
        await Auth.requireAuth(['Seller']);
        await this.loadSummary();
        await this.loadAuctions();
        await this.loadCategories();
    },

    async loadSummary() {
        try {
            const summary = await API.get('/api/Auctions/seller/dashboard-summary');
            document.getElementById('stat-total').innerText = summary.totalAuctionCount ?? summary.totalAuctions ?? 0;
            document.getElementById('stat-active').innerText = summary.activeAuctionCount ?? summary.activeAuctions ?? 0;
            document.getElementById('stat-completed').innerText = summary.completedAuctionCount ?? summary.completedAuctions ?? 0;
        } catch (err) {
            console.error(err);
        }
    },

    async loadCategories() {
        try {
            const categories = await API.get('/api/Categories');
            const select = document.getElementById('create-category');
            if (select) {
                select.innerHTML = categories.map(c => `<option value="${c.id}">${c.name}</option>`).join('');
            }
        } catch (e) {
            console.error(e);
        }
    },

    async loadAuctions() {
        try {
            const auctions = await API.get('/api/Auctions/seller/my');
            const tbody = document.getElementById('seller-auctions-table');

            if (auctions.length === 0) {
                tbody.innerHTML = `<tr><td colspan="5" class="text-center py-8 text-gray-400">Hələ hərracınız yoxdur.</td></tr>`;
                return;
            }

            tbody.innerHTML = auctions.map(a => `
                <tr class="border-b border-gray-100 hover:bg-gray-50 transition">
                    <td class="py-4 px-4 font-medium text-gray-900">${a.title}</td>
                    <td class="py-4 px-4 text-gray-600">${Common.formatCurrency(a.currentPrice)}</td>
                    <td class="py-4 px-4">
                        <span class="px-2.5 py-1 rounded-full text-xs font-semibold ${
                            a.status === 'Active' ? 'bg-emerald-100 text-emerald-700' : 'bg-gray-100 text-gray-600'
                        }">${a.status}</span>
                    </td>
                    <td class="py-4 px-4 text-xs text-gray-500">${new Date(a.endTime).toLocaleDateString()}</td>
                    <td class="py-4 px-4 text-right space-x-2">
                        <button onclick="SellerDashboard.openImageModal('${a.id}')" class="text-xs bg-indigo-50 text-indigo-600 px-3 py-1.5 rounded-lg hover:bg-indigo-100 font-medium">Şəkil Yüklə</button>
                        ${a.status === 'Active' ? `
                            <button onclick="SellerDashboard.cancelAuction('${a.id}')" class="text-xs bg-red-50 text-red-600 px-3 py-1.5 rounded-lg hover:bg-red-100 font-medium">Ləğv Et</button>
                        ` : ''}
                    </td>
                </tr>
            `).join('');
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    async createAuction(e) {
        e.preventDefault();
        const data = {
            title: document.getElementById('create-title').value,
            description: document.getElementById('create-description').value,
            startingPrice: parseFloat(document.getElementById('create-price').value),
            endTime: new Date(document.getElementById('create-endtime').value).toISOString(),
            categoryId: document.getElementById('create-category').value
        };

        try {
            await API.post('/api/Auctions', data);
            Common.showToast('Hərrac uğurla yaradıldı!', 'success');
            document.getElementById('create-modal').classList.add('hidden');
            await this.loadAuctions();
            await this.loadSummary();
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    async cancelAuction(id) {
        if (!confirm('Hərracı ləğv etmək istədiyinizə əminsiniz?')) return;
        try {
            await API.patch(`/api/Auctions/${id}/cancel`);
            Common.showToast('Hərrac ləğv edildi', 'success');
            await this.loadAuctions();
        } catch (err) {
            // Backend 409 Conflict if auction already has bids
            Common.showToast(err.errors);
        }
    },

    openImageModal(id) {
        document.getElementById('upload-auction-id').value = id;
        document.getElementById('image-modal').classList.remove('hidden');
    },

    async uploadImage(e) {
        e.preventDefault();
        const id = document.getElementById('upload-auction-id').value;
        const fileInput = document.getElementById('image-file');
        if (!fileInput.files[0]) return;

        try {
            await API.uploadFile(`/api/Auctions/${id}/images`, fileInput.files[0]);
            Common.showToast('Şəkil yükləndi!', 'success');
            document.getElementById('image-modal').classList.add('hidden');
            await this.loadAuctions();
        } catch (err) {
            Common.showToast(err.errors);
        }
    }
};