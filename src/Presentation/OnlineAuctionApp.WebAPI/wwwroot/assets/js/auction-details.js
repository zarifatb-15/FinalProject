function auctionDetailsPage() {
  return {
    auction: null,
    bids: [],
    user: null,
    loading: true,
    error: "",
    bidAmount: "",
    bidError: "",
    successMessage: "",
    bidSubmitting: false,
    countdownText: "",
    countdownTimer: null,

    async init() {
      const auctionId = this.getAuctionId();

      if (!auctionId) {
        this.error = "Auction id is missing.";
        this.loading = false;
        return;
      }

      try {
        await Promise.all([
          this.loadCurrentUser(),
          this.loadAuction(auctionId),
          this.loadBidHistory(auctionId),
        ]);

        this.startCountdown();
      } catch (error) {
        this.error = error.message;
      } finally {
        this.loading = false;
      }
    },

    getAuctionId() {
      const params = new URLSearchParams(window.location.search);
      return params.get("id");
    },

    async loadCurrentUser() {
      try {
        this.user = await apiRequest("/api/Auth/me");
      } catch {
        this.user = null;
      }
    },

    async loadAuction(auctionId) {
      this.auction = await apiRequest(`/api/Auctions/${auctionId}`);
      this.updateCountdown();
    },

    async loadBidHistory(auctionId) {
      this.bids = await apiRequest(`/api/Auctions/${auctionId}/bids`);
    },

    get mainImageUrl() {
      if (this.auction?.images && this.auction.images.length > 0) {
        return this.auction.images[0].imageUrl;
      }

      return null;
    },

    get isBuyer() {
      return this.user?.roles?.includes("Buyer");
    },

    get minimumBidPlaceholder() {
      if (!this.auction) {
        return "Enter bid amount";
      }

      return `More than ${formatPrice(this.auction.currentPrice)}`;
    },

    formatPriceValue(value) {
      return formatPrice(value);
    },

    formatDateValue(value) {
      return parseApiDate(value).toLocaleString("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
      });
    },

    startCountdown() {
      this.updateCountdown();

      this.countdownTimer = setInterval(() => {
        this.updateCountdown();
      }, 1000);
    },

    updateCountdown() {
      if (!this.auction) {
        this.countdownText = "";
        return;
      }

      const end = parseApiDate(this.auction.endTime).getTime();
      const now = new Date().getTime();
      const diff = end - now;

      if (diff <= 0) {
        this.countdownText = "Ended";
        return;
      }

      const days = Math.floor(diff / (1000 * 60 * 60 * 24));
      const hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
      const minutes = Math.floor((diff / (1000 * 60)) % 60);
      const seconds = Math.floor((diff / 1000) % 60);

      if (days > 0) {
        this.countdownText = `${days}d ${hours}h ${minutes}m`;
        return;
      }

      this.countdownText = `${hours}h ${minutes}m ${seconds}s`;
    },

    async placeBid() {
      this.bidError = "";
      this.successMessage = "";

      const amount = Number(this.bidAmount);

      if (!Number.isFinite(amount) || amount <= 0) {
        this.bidError = "Bid amount must be greater than zero.";
        return;
      }

      if (amount <= this.auction.currentPrice) {
        this.bidError = "Bid amount must be greater than the current bid.";
        return;
      }

      this.bidSubmitting = true;

      try {
        const auctionId = this.getAuctionId();

        await apiRequest(`/api/Auctions/${auctionId}/bids`, {
          method: "POST",
          body: JSON.stringify({ amount }),
        });

        await this.loadAuction(auctionId);
        await this.loadBidHistory(auctionId);

        this.bidAmount = "";
        this.successMessage = "Your bid was placed successfully.";
      } catch (error) {
        this.bidError = error.message;
      } finally {
        this.bidSubmitting = false;
      }
    },
  };
}
