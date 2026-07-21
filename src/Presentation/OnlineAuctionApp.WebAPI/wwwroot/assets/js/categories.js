const CategoriesPage = {
    async init() {
        await Auth.requireAuth(['Seller', 'Admin']);
        await this.loadCategories();
    },

    async loadCategories() {
        try {
            const list = await API.get('/api/Categories');
            document.getElementById('categories-table').innerHTML = list.map(c => `
                <tr class="border-b border-gray-100">
                    <td class="py-3 px-4 font-bold text-gray-800">${c.name}</td>
                    <td class="py-3 px-4 text-gray-500">${c.description || '-'}</td>
                    <td class="py-3 px-4 text-right space-x-2">
                        <button onclick="CategoriesPage.deleteCategory('${c.id}')" class="text-xs bg-red-50 text-red-600 px-3 py-1.5 rounded-lg hover:bg-red-100 font-medium">Sil</button>
                    </td>
                </tr>
            `).join('');
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    async createCategory(e) {
        e.preventDefault();
        const data = {
            name: document.getElementById('cat-name').value,
            description: document.getElementById('cat-desc').value
        };

        try {
            await API.post('/api/Categories', data);
            Common.showToast('Kategoriya yaradıldı!', 'success');
            document.getElementById('cat-form').reset();
            await this.loadCategories();
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    async deleteCategory(id) {
        if (!confirm('Kategoriyanı silməyə əminsiniz?')) return;
        try {
            await API.delete(`/api/Categories/${id}`);
            Common.showToast('Kategoriya silindi', 'success');
            await this.loadCategories();
        } catch (err) {
            Common.showToast(err.errors);
        }
    }
};