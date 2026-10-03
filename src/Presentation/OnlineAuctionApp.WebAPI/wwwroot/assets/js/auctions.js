const AuctionsPage = {
  items: [],

  async init() {
    await this.loadCategories();
    await this.loadAuctions();
    setInterval(() => this.updateTimers(), 1000);
  },

  async loadCategories() {
    try {
      const categories = await API.get("/api/Categories");
      const select = document.getElementById("category-filter");
      if (!select) return;
      categories.forEach((c) => {
        const opt = document.createElement("option");
        opt.value = c.id;
        opt.textContent = c.name;
        select.appendChild(opt);
      });
    } catch (e) {
      console.error(e);
    }
  },

  async loadAuctions() {
    const search = document.getElementById("search-input")?.value || "";
    const categoryId = document.getElementById("category-filter")?.value || "";
    const minPrice = document.getElementById("min-price")?.value || "";
    const maxPrice = document.getElementById("max-price")?.value || "";

    let query = [];
    if (search) query.push(`Search=${encodeURIComponent(search)}`);
    if (categoryId) query.push(`CategoryId=${categoryId}`);
    if (minPrice) query.push(`MinPrice=${minPrice}`);
    if (maxPrice) query.push(`MaxPrice=${maxPrice}`);

    const queryString = query.length > 0 ? "?" + query.join("&") : "";

    try {
      this.items = await API.get("/api/Auctions" + queryString);
      this.render();
    } catch (err) {
      Common.showToast(err.errors);
    }
  },

  render() {
    const grid = document.getElementById("auctions-grid");
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

    grid.innerHTML = this.items
      .map((a) => {
        const img = (() => {
          const title = (a.title || "").toLowerCase();
          const category = (
            a.categoryName ||
            a.category?.name ||
            ""
          ).toLowerCase();
          const u = (id) =>
            `https://images.unsplash.com/${id}?w=600&auto=format&fit=crop`;
          if (a.images && a.images.length > 0) {
            const latestImage = a.images[a.images.length - 1];
            return latestImage.imageUrl;
          }

          // Order is important: "headphone" contains "phone", so headphones must be checked first.
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

        return `
                <div class="bg-white rounded-2xl border border-gray-100 shadow-sm hover:shadow-xl transition-all duration-300 flex flex-col overflow-hidden group">
                    <div class="relative h-48 bg-gray-100 overflow-hidden">
                        <img src="${img}" alt="${a.title}" class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500">
                        <span class="absolute top-3 left-3 bg-white/90 backdrop-blur-md text-gray-800 text-xs px-3 py-1 rounded-full font-semibold shadow-sm">
                            ${a.categoryName || "General"}
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
      })
      .join("");

    this.updateTimers();
  },

  updateTimers() {
    document.querySelectorAll(".auction-timer").forEach((el) => {
      const end = el.getAttribute("data-endtime");
      const cd = Common.formatCountdown(end);
      el.textContent = cd.text;
      if (cd.isEnded) {
        el.className =
          "auction-timer text-xs font-bold text-red-600 bg-red-50 px-2 py-1 rounded-md";
      }
    });
  },
};
