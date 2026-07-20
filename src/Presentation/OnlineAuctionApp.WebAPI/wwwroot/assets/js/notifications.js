const notificationState = {
  user: null,
  notifications: [],
  connection: null,
};

function normalizeNotification(notification) {
  if (typeof notification === "string") {
    return {
      id: `local-${Date.now()}`,
      message: notification,
      isRead: false,
      createdDate: new Date().toISOString(),
    };
  }

  return {
    id: notification.id || notification.Id || `local-${Date.now()}`,
    message:
      notification.message ||
      notification.Message ||
      "You have a new auction notification.",
    isRead: notification.isRead ?? notification.IsRead ?? false,
    createdDate:
      notification.createdDate ||
      notification.CreatedDate ||
      new Date().toISOString(),
  };
}

function createNotificationUi() {
  const navLinks = document.querySelector(".nav-links");

  if (!navLinks || document.getElementById("notificationButton")) {
    return;
  }

  const wrapper = document.createElement("div");
  wrapper.className = "notification-wrapper";

  wrapper.innerHTML = `
    <button type="button" id="notificationButton" class="notification-button">
      Notifications
      <span id="notificationBadge" class="notification-badge hidden">0</span>
    </button>

    <div id="notificationPanel" class="notification-panel hidden">
      <div class="notification-panel-header">
        <strong>Notifications</strong>
        <span id="notificationCountText">No unread notifications</span>
      </div>

      <div id="notificationList" class="notification-list">
        <p class="notification-empty">No notifications yet.</p>
      </div>
    </div>
  `;

  const logoutButton = document.getElementById("logoutButton");
  navLinks.insertBefore(wrapper, logoutButton || null);

  document.getElementById("notificationButton").addEventListener("click", () => {
    document.getElementById("notificationPanel").classList.toggle("hidden");
  });

  document.addEventListener("click", (event) => {
    if (!event.target.closest(".notification-wrapper")) {
      document.getElementById("notificationPanel")?.classList.add("hidden");
    }
  });

  if (!document.querySelector(".toast-container")) {
    const toastContainer = document.createElement("div");
    toastContainer.className = "toast-container";
    document.body.appendChild(toastContainer);
  }
}

function renderNotifications() {
  const badge = document.getElementById("notificationBadge");
  const countText = document.getElementById("notificationCountText");
  const list = document.getElementById("notificationList");

  if (!badge || !countText || !list) {
    return;
  }

  const unreadCount = notificationState.notifications.filter(
    (item) => !item.isRead
  ).length;

  if (unreadCount > 0) {
    badge.textContent = unreadCount;
    badge.classList.remove("hidden");
    countText.textContent = `${unreadCount} unread`;
  } else {
    badge.classList.add("hidden");
    countText.textContent = "No unread notifications";
  }

  if (notificationState.notifications.length === 0) {
    list.innerHTML = `<p class="notification-empty">No notifications yet.</p>`;
    return;
  }

  list.innerHTML = notificationState.notifications
    .slice(0, 8)
    .map(
      (item) => `
        <button
          type="button"
          class="notification-item ${item.isRead ? "" : "unread"}"
          data-notification-id="${escapeHtml(item.id)}"
        >
          <span>${escapeHtml(item.message)}</span>
          <small>${formatShortDate(item.createdDate)}</small>
        </button>
      `
    )
    .join("");

  list.querySelectorAll(".notification-item").forEach((button) => {
    button.addEventListener("click", async () => {
      const notificationId = button.dataset.notificationId;
      await markNotificationAsRead(notificationId);
    });
  });
}

async function loadNotifications() {
  try {
    const notifications = await apiRequest("/api/Notifications/my");

    notificationState.notifications = notifications
      .map(normalizeNotification)
      .sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));

    renderNotifications();
  } catch {
    notificationState.notifications = [];
    renderNotifications();
  }
}

async function markNotificationAsRead(notificationId) {
  const notification = notificationState.notifications.find(
    (item) => item.id === notificationId
  );

  if (!notification || notification.isRead) {
    return;
  }

  try {
    await apiRequest(`/api/Notifications/${notificationId}/read`, {
      method: "PATCH",
    });

    notification.isRead = true;
    renderNotifications();
  } catch {
    notification.isRead = true;
    renderNotifications();
  }
}

function showNotificationToast(notification) {
  const toastContainer = document.querySelector(".toast-container");

  if (!toastContainer) {
    return;
  }

  const toast = document.createElement("div");
  toast.className = "notification-toast";
  toast.innerHTML = `
    <strong>New notification</strong>
    <p>${escapeHtml(notification.message)}</p>
  `;

  toastContainer.appendChild(toast);

  setTimeout(() => {
    toast.remove();
  }, 5000);
}

async function connectNotificationHub() {
  if (!window.signalR) {
    return;
  }

  notificationState.connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications")
    .withAutomaticReconnect()
    .build();

  notificationState.connection.on("ReceiveNotification", (notification) => {
    const item = normalizeNotification(notification);

    notificationState.notifications.unshift(item);
    renderNotifications();
    showNotificationToast(item);
  });

  try {
    await notificationState.connection.start();
  } catch {
    // If SignalR connection fails, stored notifications still work from API.
  }
}

async function initializeNotifications() {
  try {
    notificationState.user = await apiRequest("/api/Auth/me");
  } catch {
    return;
  }

  if (!notificationState.user) {
    return;
  }

  createNotificationUi();
  await loadNotifications();
  await connectNotificationHub();
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initializeNotifications);
} else {
  initializeNotifications();
}
