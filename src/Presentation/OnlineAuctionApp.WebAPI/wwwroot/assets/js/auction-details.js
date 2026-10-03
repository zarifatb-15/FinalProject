const AuctionDetailsPage = {
  auctionId: null,
  auction: null,

  async init() {
    const params = new URLSearchParams(window.location.search);
    this.auctionId = params.get("id");
    if (!this.auctionId) {
      window.location.href = "auctions.html";
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
    document.getElementById("auction-title").innerText = a.title;
    document.getElementById("auction-category").innerText =
      a.categoryName || "Ümumi";
    document.getElementById("auction-description").innerText = a.description;
    document.getElementById("auction-current-price").innerText =
      Common.formatCurrency(a.currentPrice);
    document.getElementById("auction-start-price").innerText =
      Common.formatCurrency(a.startingPrice);

    const imgEl = document.getElementById("main-image");
    const demoImage = (() => {
      const title = (a.title || "").toLowerCase();
      const category = (a.categoryName || a.category?.name || "").toLowerCase();
      const u = (id) =>
        `https://images.unsplash.com/${id}?w=900&auto=format&fit=crop`;

      if (a.images && a.images.length > 0) {
        const latestImage = a.images[a.images.length - 1];
        return latestImage.imageUrl;
      }

      if (
        title.includes("headphone") ||
        title.includes("sony") ||
        title.includes("qulaqcıq")
      ) {
        return u("photo-1505740420928-5e560c06d30e");
      }

      if (title.includes("ipad") || title.includes("tablet")) {
        return u("photo-1544244015-0df4b3ffc6b0");
      }

      if (
        title.includes("iphone") ||
        title.includes("phone") ||
        title.includes("telefon")
      ) {
        return u("photo-1592750475338-74b7b21085ab");
      }

      if (title.includes("macbook") || title.includes("laptop")) {
        return u("photo-1496181133206-80ce9b88a853");
      }

      if (
        title.includes("camera") ||
        title.includes("canon") ||
        title.includes("kamera")
      ) {
        return u("photo-1502920917128-1aa500764cbd");
      }

      if (title.includes("playstation") || title.includes("console")) {
        return u("photo-1606144042614-b2417e99c4e3");
      }

      if (title.includes("keyboard") || title.includes("klaviatura")) {
        return u("photo-1587829741301-dc798b83add3");
      }

      if (
        title.includes("office chair") ||
        title.includes("ofis kreslosu") ||
        title.includes("kreslo") ||
        title.includes("chair")
      ) {
        return u("photo-1580480055273-228ff5388ef8");
      }

      if (title.includes("bag") || title.includes("çanta")) {
        return u("photo-1590874103328-eac38a683ce7");
      }

      if (
        title.includes("painting") ||
        title.includes("rəsm") ||
        title.includes("tablo") ||
        title.includes("dekorativ")
      ) {
        return u("photo-1579783902614-a3fb3927b6a5");
      }

      if (
        title.includes("vintage") ||
        title.includes("collectible") ||
        category.includes("collect")
      ) {
        return u("photo-1516035069371-29a1b244cc32");
      }

      if (
        title.includes("speaker") ||
        title.includes("səs") ||
        title.includes("dinamik")
      ) {
        return u("photo-1608043152269-423dbba4e7e1");
      }

      if (category.includes("gaming")) {
        return u("photo-1550745165-9bc0b252726f");
      }

      if (category.includes("home")) {
        return u("photo-1555041469-a586c61ea9bc");
      }

      if (category.includes("art")) {
        return u("photo-1579783902614-a3fb3927b6a5");
      }

      return u("photo-1523275335684-37898b6baf30");
    })();

    imgEl.src = demoImage;

    // Bidding Form Control based on auth role
    const bidSection = document.getElementById("bid-section");
    if (Auth.currentUser && Auth.hasRole("Buyer") && a.status === "Active") {
      bidSection.classList.remove("hidden");
      const minimumBid = Number(a.currentPrice || a.startingPrice || 0) + 1;
      document.getElementById("min-bid-amount").innerText =
        Common.formatCurrency(minimumBid);
      document.getElementById("bid-amount").min = minimumBid;
    } else {
      bidSection.classList.add("hidden");
    }

    if (a.winnerUsername) {
      document.getElementById("winner-info").classList.remove("hidden");
      document.getElementById("winner-name").innerText = a.winnerUsername;
    }
  },

  renderBids(bids) {
    const container = document.getElementById("bids-history");
    document.getElementById("bids-count").innerText = bids.length;

    if (bids.length === 0) {
      container.innerHTML = `<p class="text-gray-400 text-sm py-4">Hələ heç bir təklif verilməyib.</p>`;
      return;
    }

    container.innerHTML = bids
      .map(
        (b) => `
            <div class="flex items-center justify-between py-3 border-b border-gray-100 last:border-0">
                <div class="flex items-center space-x-3">
                    <div class="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center text-xs font-bold text-gray-600">
                        ${b.buyerUsername ? b.buyerUsername[0].toUpperCase() : "B"}
                    </div>
                    <div>
                        <p class="font-semibold text-sm text-gray-800">${b.buyerUsername || "Alıcı"}</p>
                        <p class="text-xs text-gray-400">${new Date(b.bidTime || b.createdDate || b.createdAt || Date.now()).toLocaleString()}</p>
                    </div>
                </div>
                <span class="font-bold text-emerald-600">${Common.formatCurrency(b.amount)}</span>
            </div>
        `,
      )
      .join("");
  },

  async placeBid() {
    const amount = parseFloat(document.getElementById("bid-amount").value);
    if (!amount) return;

    try {
      await API.post(`/api/Auctions/${this.auctionId}/bids`, { amount });
      Common.showToast("Təklifiniz uğurla qəbul edildi!", "success");
      await this.loadAuction();
      await this.loadBids();
    } catch (err) {
      Common.showToast(err.errors);
    }
  },

  updateTimer() {
    if (!this.auction) return;
    const cd = Common.formatCountdown(this.auction.endTime);
    document.getElementById("detail-timer").innerText = cd.text;
  },
};
