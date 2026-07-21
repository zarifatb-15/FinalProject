const AuctionDetailsPage = {
    auctionId: null,
    auction: null,

    async init() {
        const params = new URLSearchParams(window.location.search);
        this.auctionId = params.get('id');
        if (!this.auctionId) {
            window.location.href = 'auctions.html';
            return;
        }

        await this.loadAuction();
        await this.loadBids();
        setInterval(() => this.updateTimer(), 1000);
    },

    async loadAuction() {
        try {
            this.auction = await API.get(`/api/Auctions/${this.auctionId}`);
            this.renderDetails();
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    async loadBids() {
        try {
            const bids = await API.get(`/api/Auctions/${this.auctionId}/bids`);
            this.renderBids(bids);
        } catch (err) {
            console.error(err);
        }
    },

    renderDetails() {
        const a = this.auction;
        document.getElementById('auction-title').innerText = a.title;
        document.getElementById('auction-category').innerText = a.categoryName || 'Ümumi';
        document.getElementById('auction-description').innerText = a.description;
        document.getElementById('auction-current-price').innerText = Common.formatCurrency(a.currentPrice);
        document.getElementById('auction-start-price').innerText = Common.formatCurrency(a.startingPrice);

        const imgEl = document.getElementById('main-image');
        if (a.images && a.images.length > 0) {
            imgEl.src = a.images.find(i => i.isPrimary)?.imageUrl || a.images[0].imageUrl;
        } else {
            imgEl.src = 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=600';
        }

        // Bidding Form Control based on auth role
        const bidSection = document.getElementById('bid-section');
        if (Auth.currentUser && Auth.hasRole('Buyer') && a.status === 'Active') {
            bidSection.classList.remove('hidden');
            const minimumBid = Number(a.currentPrice || a.startingPrice || 0) + 1;
            document.getElementById('min-bid-amount').innerText = Common.formatCurrency(minimumBid);
            document.getElementById('bid-amount').min = minimumBid;
        } else {
            bidSection.classList.add('hidden');
        }

        if (a.winnerUsername) {
            document.getElementById('winner-info').classList.remove('hidden');
            document.getElementById('winner-name').innerText = a.winnerUsername;
        }
    },

    renderBids(bids) {
        const container = document.getElementById('bids-history');
        document.getElementById('bids-count').innerText = bids.length;

        if (bids.length === 0) {
            container.innerHTML = `<p class="text-gray-400 text-sm py-4">Hələ heç bir təklif verilməyib.</p>`;
            return;
        }

        container.innerHTML = bids.map(b => `
            <div class="flex items-center justify-between py-3 border-b border-gray-100 last:border-0">
                <div class="flex items-center space-x-3">
                    <div class="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center text-xs font-bold text-gray-600">
                        ${b.buyerUsername ? b.buyerUsername[0].toUpperCase() : 'B'}
                    </div>
                    <div>
                        <p class="font-semibold text-sm text-gray-800">${b.buyerUsername || 'Alıcı'}</p>
                        <p class="text-xs text-gray-400">${new Date(b.bidTime || b.createdDate || b.createdAt || Date.now()).toLocaleString()}</p>
                    </div>
                </div>
                <span class="font-bold text-emerald-600">${Common.formatCurrency(b.amount)}</span>
            </div>
        `).join('');
    },

    async placeBid() {
        const amount = parseFloat(document.getElementById('bid-amount').value);
        if (!amount) return;

        try {
            await API.post(`/api/Auctions/${this.auctionId}/bids`, { amount });
            Common.showToast('Təklifiniz uğurla qəbul edildi!', 'success');
            await this.loadAuction();
            await this.loadBids();
        } catch (err) {
            Common.showToast(err.errors);
        }
    },

    updateTimer() {
        if (!this.auction) return;
        const cd = Common.formatCountdown(this.auction.endTime);
        document.getElementById('detail-timer').innerText = cd.text;
    }
};