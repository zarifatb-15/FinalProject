const Common = {
  // 0. CURRENCY FORMATTER
  formatCurrency(amount) {
    if (amount === null || amount === undefined) return "$0.00";
    return (
      "$" +
      Number(amount).toLocaleString("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      })
    );
  },

  // 0b. COUNTDOWN FORMATTER
  formatCountdown(endTime) {
    if (!endTime) return { text: "—", isEnded: true };
    const now = new Date();
    const end = new Date(endTime);
    const diff = end - now;

    if (diff <= 0) return { text: "Bitib", isEnded: true };

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((diff % (1000 * 60)) / 1000);

    if (days > 0) return { text: `${days}g ${hours}s`, isEnded: false };
    if (hours > 0) return { text: `${hours}s ${minutes}d`, isEnded: false };
    return { text: `${minutes}d ${seconds}s`, isEnded: false };
  },

  // 1. NAVBAR RENDERER
  async renderNavbar() {
    const navContainer = document.getElementById("navbar-container");
    if (!navContainer) return;

    let user = null;
    if (typeof Auth !== "undefined" && Auth.init) {
      user = await Auth.init();
    }

    const currentLang =
      typeof I18n !== "undefined"
        ? I18n.lang
        : localStorage.getItem("app_lang") || "az";

    // Əsas menyu linkləri
    let navLinks = `
            <a href="index.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_home">Ana Səhifə</a>
            <a href="auctions.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_auctions">Hərraclar</a>
        `;

    if (!user) {
      navLinks += `
                <a href="login.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_login">Daxil Ol</a>
                <a href="register.html" class="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-lg transition font-medium" data-i18n="nav_register">Qeydiyyat</a>
            `;
    } else {
      if (typeof Auth !== "undefined" && Auth.hasRole) {
        if (Auth.hasRole("Seller")) {
          navLinks += `<a href="seller-dashboard.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_seller_panel">Satıcı Paneli</a>`;
        }
        if (Auth.hasRole("Admin")) {
          navLinks += `<a href="admin-dashboard.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_admin_panel">Admin Paneli</a>`;
        }
        navLinks += `<a href="categories.html" class="hover:text-indigo-600 dark:hover:text-indigo-400 transition" data-i18n="nav_categories">Kategoriyalar</a>`;
      }

      const username = user.username || user.email || "İstifadəçi";
      const userInitial = username.charAt(0).toUpperCase();

      navLinks += `
                <a href="notifications.html" class="relative hover:text-indigo-600 dark:hover:text-indigo-400 transition">
                    <i class="fa-solid fa-bell text-xl"></i>
                    <span id="unread-count-badge" class="hidden absolute -top-2 -right-2 bg-red-500 text-white text-xs px-1.5 py-0.5 rounded-full font-bold">0</span>
                </a>
               <div class="relative group cursor-pointer py-2">
    <div class="flex items-center space-x-2 font-medium text-gray-700 dark:text-gray-200">
        <div class="w-8 h-8 rounded-full bg-indigo-100 dark:bg-indigo-900 text-indigo-600 dark:text-indigo-300 flex items-center justify-center font-bold">
            ${userInitial}
        </div>
        <span>${username}</span>
        <i class="fa-solid fa-chevron-down text-xs"></i>
    </div>

    <div class="absolute right-0 top-[calc(100%-8px)] pt-3 w-48 hidden group-hover:block z-50">
        <div class="bg-white dark:bg-slate-800 border border-gray-100 dark:border-slate-700 rounded-xl shadow-lg py-2">
            <button onclick="Auth.logout()" class="w-full text-left px-4 py-2 text-sm text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-slate-700 flex items-center space-x-2">
                <i class="fa-solid fa-right-from-bracket"></i>
                <span data-i18n="nav_logout">Çıxış Et</span>
            </button>
        </div>
    </div>
</div>
            `;
    }

    // Dark Mode və Dil Seçimi
    navLinks += `
            <div class="flex items-center space-x-3 ml-2 pl-3 border-l border-gray-200 dark:border-gray-700">
                <button onclick="Theme.toggle()" type="button" class="p-2 text-gray-600 dark:text-amber-400 hover:text-indigo-600 transition flex items-center justify-center">
                    <i class="fa-solid fa-moon text-lg dark:hidden"></i>
                    <i class="fa-solid fa-sun text-lg hidden dark:block"></i>
                </button>

                <select onchange="I18n.setLang(this.value)" class="bg-transparent border border-gray-200 dark:border-gray-700 text-gray-800 dark:text-gray-200 rounded-lg px-2 py-1 text-xs font-semibold focus:outline-none cursor-pointer">
                    <option value="az" class="bg-white dark:bg-slate-800 text-gray-800 dark:text-white" ${currentLang === "az" ? "selected" : ""}>AZ</option>
                    <option value="en" class="bg-white dark:bg-slate-800 text-gray-800 dark:text-white" ${currentLang === "en" ? "selected" : ""}>EN</option>
                    <option value="ru" class="bg-white dark:bg-slate-800 text-gray-800 dark:text-white" ${currentLang === "ru" ? "selected" : ""}>RU</option>
                </select>
            </div>
        `;

    // Navbar HTML - Həm Light, həm Dark Mode dəstəkli arxa fonla
    navContainer.innerHTML = `
            <header class="bg-white dark:bg-slate-900 border-b border-gray-100 dark:border-slate-800 text-gray-800 dark:text-gray-100 transition-colors duration-300">
                <nav class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
                    <a href="index.html" class="flex items-center space-x-2 group">
                        <div class="w-8 h-8 bg-indigo-600 rounded-lg flex items-center justify-center transform group-hover:scale-105 transition-transform shadow-md">
                            <svg class="w-5 h-5 text-white" viewBox="0 0 512 512" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M160 140H260C304.183 140 340 175.817 340 220C340 250.019 323.46 276.177 298.544 290.722C331.411 300.957 355 331.758 355 368C355 412.183 319.183 448 275 448H160V140Z" stroke="currentColor" stroke-width="48" stroke-linejoin="round"/>
                                <path d="M160 280H280" stroke="currentColor" stroke-width="48" stroke-linecap="round"/>
                                <circle cx="390" cy="140" r="48" fill="#10B981"/>
                            </svg>
                        </div>
                        <span class="text-xl font-black tracking-tight text-slate-900 dark:text-white group-hover:text-indigo-600 transition-colors">Bid<span class="text-indigo-600">Verse</span></span>
                    </a>
                    <div class="flex items-center space-x-6 text-sm font-medium">
                        ${navLinks}
                    </div>
                </nav>
            </header>
        `;

    if (typeof I18n !== "undefined") {
      I18n.apply();
    }
  },

  // 2. FOOTER RENDERER
  renderFooter() {
    const footerContainer = document.getElementById("footer-container");
    if (!footerContainer) return;

    const year = new Date().getFullYear();
    footerContainer.innerHTML = `
            <footer class="bg-white dark:bg-slate-900 border-t border-gray-100 dark:border-slate-800 py-8 mt-auto transition-colors duration-300">
                <div class="max-w-7xl mx-auto px-4 text-center text-sm text-gray-500 dark:text-gray-400">
                    <p>&copy; ${year} <span class="font-bold text-gray-800 dark:text-white">BidVerse</span>. Bütün hüquqlar qorunur.</p>
                </div>
            </footer>
        `;
  },

  // 3. TOAST BİLDİRİŞLƏRİ
  showToast(message, type = "info") {
    if (!message || message === "undefined") {
      message = "Əməliyyat zamanı xəta baş verdi.";
    }
    if (Array.isArray(message)) {
      message = message.join("<br>");
    }
    let toastBox = document.getElementById("toast-container");
    if (!toastBox) {
      toastBox = document.createElement("div");
      toastBox.id = "toast-container";
      toastBox.className =
        "fixed bottom-5 right-5 z-50 flex flex-col space-y-2";
      document.body.appendChild(toastBox);
    }

    const bgColors = {
      success: "bg-emerald-600 text-white",
      error: "bg-rose-600 text-white",
      info: "bg-indigo-600 text-white",
      warning: "bg-amber-500 text-white",
    };

    const toast = document.createElement("div");
    toast.className = `${bgColors[type] || bgColors.info} px-4 py-3 rounded-xl shadow-lg text-sm font-medium flex items-center space-x-2 transition-all duration-300 transform translate-y-2 opacity-0`;
    toast.innerHTML = `<span>${message}</span>`;

    toastBox.appendChild(toast);

    setTimeout(() => {
      toast.classList.remove("translate-y-2", "opacity-0");
    }, 10);

    setTimeout(() => {
      toast.classList.add("opacity-0", "translate-y-2");
      setTimeout(() => toast.remove(), 300);
    }, 3000);
  },
};

document.addEventListener("DOMContentLoaded", () => {
  Common.renderNavbar();
  Common.renderFooter();
});
