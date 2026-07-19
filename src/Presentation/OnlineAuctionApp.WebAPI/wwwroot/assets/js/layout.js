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

function initializeLayout() {
  renderTopStrip();
  renderSiteFooter();
  markActiveNavigation();
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initializeLayout);
} else {
  initializeLayout();
}