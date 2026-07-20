const adminContent = document.getElementById("adminDashboardContent");
const adminAccessError = document.getElementById("adminAccessError");
const adminMessage = document.getElementById("adminMessage");
const adminSummaryGrid = document.getElementById("adminSummaryGrid");
const adminAuctionsTable = document.getElementById("adminAuctionsTable");
const adminUsersTable = document.getElementById("adminUsersTable");
const refreshAdminButton = document.getElementById("refreshAdminButton");
const adminName = document.getElementById("adminName");
const adminLogoutButton = document.getElementById("adminLogoutButton");

function escapeHtml(value) {
  return String(value ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

function formatAdminDisplayName(user) {
  const rawName = user?.username || user?.email?.split("@")[0] || "Admin";

  return rawName
    .replace(/([a-z])([A-Z])/g, "$1 $2")
    .replace(/[._-]+/g, " ")
    .replace(/\s+/g, " ")
    .trim()
    .replace(/\b\w/g, (letter) => letter.toUpperCase());
}

function getStatusClass(status) {
  const normalized = String(status).toLowerCase();

  if (normalized === "active") return "status-active";
  if (normalized === "completed") return "status-completed";
  if (normalized === "cancelled") return "status-cancelled";

  return "";
}

function renderSummary(summary) {
  const cards = [
    {
      label: "Total Users",
      value: summary.totalUsers,
      note: `${summary.buyerCount} buyers · ${summary.sellerCount} sellers · ${summary.adminCount} admin`,
    },
    {
      label: "Total Auctions",
      value: summary.totalAuctions,
      note: `${summary.activeAuctions} active · ${summary.completedAuctions} completed · ${summary.cancelledAuctions} cancelled`,
    },
    {
      label: "Total Bids",
      value: summary.totalBids,
      note: "Recorded bidding activity",
    },
    {
      label: "Categories",
      value: summary.totalCategories,
      note: "Marketplace groups",
    },
  ];

  adminSummaryGrid.innerHTML = cards
    .map(
      (card) => `
        <article class="admin-stat-card">
          <span>${escapeHtml(card.label)}</span>
          <strong>${escapeHtml(card.value)}</strong>
          <small>${escapeHtml(card.note)}</small>
        </article>
      `
    )
    .join("");
}

function renderUsers(users) {
  if (!users.length) {
    adminUsersTable.innerHTML = `<tr><td colspan="3">No users found.</td></tr>`;
    return;
  }

  adminUsersTable.innerHTML = users
    .map(
      (user) => `
        <tr>
          <td>
            <strong>${escapeHtml(user.fullName || user.username)}</strong>
            <small>${escapeHtml(user.username)}</small>
          </td>
          <td>${escapeHtml(user.email)}</td>
          <td>
            <span class="role-pill">${escapeHtml(user.roles.join(", "))}</span>
          </td>
        </tr>
      `
    )
    .join("");
}

function renderAuctions(auctions) {
  if (!auctions.length) {
    adminAuctionsTable.innerHTML = `<tr><td colspan="7">No auctions found.</td></tr>`;
    return;
  }

  adminAuctionsTable.innerHTML = auctions
    .map(
      (auction) => `
        <tr>
          <td>
            <strong>${escapeHtml(auction.title)}</strong>
            <small>
              ${escapeHtml(auction.categoryName)}
              ${
                auction.winnerUsername
                  ? ` · Winner: ${escapeHtml(auction.winnerUsername)}`
                  : ""
              }
            </small>
          </td>
          <td>${escapeHtml(auction.sellerUsername)}</td>
          <td>
            <span class="status-pill ${getStatusClass(auction.status)}">
              ${escapeHtml(auction.status)}
            </span>
          </td>
          <td>${formatPrice(auction.currentPrice)}</td>
          <td>${escapeHtml(auction.bidCount)}</td>
          <td>${formatShortDate(auction.endTime)}</td>
          <td>
            ${
              auction.status === "Active"
                ? `<button
                    type="button"
                    class="btn btn-small admin-danger-button admin-cancel-button"
                    data-auction-id="${escapeHtml(auction.id)}"
                    data-auction-title="${escapeHtml(auction.title)}"
                  >
                    Cancel
                  </button>`
                : `<a
                    href="/auction-details.html?id=${escapeHtml(auction.id)}"
                    class="btn btn-small btn-secondary"
                  >
                    View
                  </a>`
            }
          </td>
        </tr>
      `
    )
    .join("");

  document.querySelectorAll(".admin-cancel-button").forEach((button) => {
    button.addEventListener("click", async () => {
      const confirmed = confirm(
        `Are you sure you want to cancel "${button.dataset.auctionTitle}"?`
      );

      if (!confirmed) return;

      await cancelAuction(button.dataset.auctionId);
    });
  });
}

async function loadAdminDashboard() {
  adminMessage.textContent = "Loading admin dashboard...";
  adminMessage.className = "message";

  try {
    const dashboard = await apiRequest("/api/Admin/dashboard");
    const users = await apiRequest("/api/Admin/users");
    const auctions = await apiRequest("/api/Admin/auctions");

    renderSummary(dashboard.summary);
    renderUsers(users);
    renderAuctions(auctions);

    adminMessage.textContent = "";
  } catch (error) {
    adminMessage.textContent = error.message;
    adminMessage.className = "message error";
  }
}

async function cancelAuction(auctionId) {
  try {
    await apiRequest(`/api/Admin/auctions/${auctionId}/cancel`, {
      method: "PATCH",
    });

    adminMessage.textContent = "Auction cancelled successfully.";
    adminMessage.className = "message success";

    await loadAdminDashboard();
  } catch (error) {
    adminMessage.textContent = error.message;
    adminMessage.className = "message error";
  }
}

async function initializeAdminDashboard() {
  try {
    const user = await apiRequest("/api/Auth/me");

    if (!user || !user.roles.includes("Admin")) {
      adminAccessError.classList.remove("hidden");
      return;
    }

    adminName.textContent = formatAdminDisplayName(user);
    adminContent.classList.remove("hidden");

    await loadAdminDashboard();
  } catch {
    adminAccessError.classList.remove("hidden");
  }
}

refreshAdminButton?.addEventListener("click", loadAdminDashboard);

adminLogoutButton?.addEventListener("click", async () => {
  try {
    await apiRequest("/api/Auth/logout", {
      method: "POST",
    });
  } finally {
    window.location.href = "/login.html";
  }
});

initializeAdminDashboard();