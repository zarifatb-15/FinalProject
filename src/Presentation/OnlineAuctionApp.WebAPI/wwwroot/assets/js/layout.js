function renderTopStrip() {
  const navbar = document.querySelector(".navbar");

  if (!navbar || document.querySelector(".top-strip")) {
    return;
  }

  const topStrip = document.createElement("div");
  topStrip.className = "top-strip";
  topStrip.innerHTML = `
    <div class="top-strip-shell">
      <span>Online Auction Support Center</span>
      <span>Active listings are updated automatically</span>
      <span>Need help? support@onlineauction.com</span>
    </div>
  `;

  navbar.parentNode.insertBefore(topStrip, navbar);
}

function renderSiteFooter() {
  let footerContainer = document.getElementById("siteFooter");

  if (!footerContainer) {
    footerContainer = document.createElement("div");
    footerContainer.id = "siteFooter";
    document.body.appendChild(footerContainer);
  }

footerContainer.innerHTML = `
  <footer class="site-footer">
    <div class="footer-grid">
      <div class="footer-brand">
        <a href="/index.html" class="brand footer-logo">
          <span class="brand-mark">A</span>
          <span>
            <strong>Online Auction</strong>
            <small>Digital auction marketplace</small>
          </span>
        </a>

        <p>
          Online Auction connects sellers and buyers through a transparent
          bidding process. Sellers can publish auction listings, while buyers
          follow deadlines, place bids and review auction activity in one place.
        </p>
      </div>

      <div class="footer-column">
        <h3>Marketplace</h3>
        <a href="/auctions.html">Active auctions</a>
        <a href="/auctions.html">Browse by category</a>
        <a href="/auctions.html">Search listings</a>
        <a href="/auctions.html">Price filter</a>
      </div>

      <div class="footer-column">
        <h3>Account</h3>
        <a href="/login.html">Login</a>
        <a href="/register.html">Create account</a>
        <a href="/seller-dashboard.html">Seller dashboard</a>
        <p>Buyer and seller role support</p>
      </div>

      <div class="footer-column">
        <h3>Contact</h3>
        <p>Online Auction Support Center</p>
        <p>Baku, Azerbaijan</p>
        <p>+994 50 000 00 00</p>
        <p>support@onlineauction.com</p>
      </div>
    </div>

    <div class="footer-bottom">
      <span>© 2026 Online Auction System. All rights reserved.</span>
      <span>By Babayeva Zarifa.</span>
    </div>
  </footer>
`;
}

function markActiveNavigation() {
  const currentPath = window.location.pathname;
  const navLinks = document.querySelectorAll(".nav-links a");

  navLinks.forEach((link) => {
    const href = link.getAttribute("href");

    if (!href) {
      return;
    }

    if (currentPath === href || currentPath.endsWith(href)) {
      link.classList.add("active-nav-link");
    }
  });
}
function formatAccountName(user) {
  const rawName = user?.username || user?.email?.split("@")[0] || "Account";

  return rawName
    .replace(/([a-z])([A-Z])/g, "$1 $2")
    .replace(/[._-]+/g, " ")
    .replace(/\s+/g, " ")
    .trim()
    .replace(/\b\w/g, (letter) => letter.toUpperCase());
}

function getUserInitial(user) {
  const name = formatAccountName(user);
  return name.charAt(0).toUpperCase();
}

function getPrimaryRole(user) {
  if (!user?.roles || user.roles.length === 0) {
    return "User";
  }

  return user.roles[0];
}

async function renderAccountPreview() {
  const navLinks = document.querySelector(".nav-links");

  if (!navLinks || typeof apiRequest !== "function") {
    return;
  }

  document.getElementById("accountPreview")?.remove();

  let user;

  try {
    user = await apiRequest("/api/Auth/me");
  } catch {
    return;
  }

  if (!user) {
    return;
  }
  if (user.roles && user.roles.includes("Admin")) {
  if (!document.getElementById("adminDashboardLink")) {
    const adminLink = document.createElement("a");
    adminLink.href = "/admin-dashboard.html";
    adminLink.id = "adminDashboardLink";
    adminLink.textContent = "Admin Dashboard";

    const auctionsLink = navLinks.querySelector('a[href="/auctions.html"]');
    navLinks.insertBefore(adminLink, auctionsLink || navLinks.firstChild);
  }

  markActiveNavigation();
}

  const accountPreview = document.createElement("div");
  accountPreview.id = "accountPreview";
  accountPreview.className = "account-preview";

  accountPreview.innerHTML = `
    <button type="button" class="account-chip" id="accountChipButton">
      <span class="account-avatar" id="accountAvatar"></span>
      <span class="account-chip-text">
        <strong id="accountDisplayName"></strong>
        <small id="accountRoleName"></small>
      </span>
    </button>

    <div class="account-dropdown hidden" id="accountDropdown">
      <div class="account-dropdown-header">
        <span class="account-avatar large" id="accountDropdownAvatar"></span>
        <div>
          <strong id="accountDropdownName"></strong>
          <small id="accountDropdownEmail"></small>
        </div>
      </div>

      <div class="account-dropdown-meta">
        <span id="accountDropdownRole"></span>
        <span>Signed in</span>
      </div>
    </div>
  `;

  const logoutButton = document.getElementById("logoutButton");
  navLinks.insertBefore(accountPreview, logoutButton || null);

  const displayName = formatAccountName(user);
  const roleName = getPrimaryRole(user);
  const initial = getUserInitial(user);

  document.getElementById("accountAvatar").textContent = initial;
  document.getElementById("accountDropdownAvatar").textContent = initial;
  document.getElementById("accountDisplayName").textContent = displayName;
  document.getElementById("accountRoleName").textContent = `${roleName} account`;
  document.getElementById("accountDropdownName").textContent = displayName;
  document.getElementById("accountDropdownEmail").textContent =
    user.email || "No email available";
  document.getElementById("accountDropdownRole").textContent = roleName;

  document.getElementById("accountChipButton").addEventListener("click", () => {
    document.getElementById("accountDropdown").classList.toggle("hidden");
  });

  document.addEventListener("click", (event) => {
    if (!event.target.closest(".account-preview")) {
      document.getElementById("accountDropdown")?.classList.add("hidden");
    }
  });
}

function initializeLayout() {
  renderTopStrip();
  renderSiteFooter();
  markActiveNavigation();
  renderAccountPreview();
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initializeLayout);
} else {
  initializeLayout();
}