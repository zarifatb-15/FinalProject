async function getCurrentUser() {
    try {
        return await apiRequest("/api/Auth/me");
    } catch {
        return null;
    }
}

async function updateNavigation() {
    const user = await getCurrentUser();

    const loginLink = document.getElementById("loginLink");
    const registerLink = document.getElementById("registerLink");
    const logoutButton = document.getElementById("logoutButton");
    const sellerDashboardLink = document.getElementById("sellerDashboardLink");

    if (user) {
        loginLink?.classList.add("hidden");
        registerLink?.classList.add("hidden");
        logoutButton?.classList.remove("hidden");

        if (sellerDashboardLink && user.roles.includes("Seller")) {
            sellerDashboardLink.classList.remove("hidden");
        }
    } else {
        loginLink?.classList.remove("hidden");
        registerLink?.classList.remove("hidden");
        logoutButton?.classList.add("hidden");
        sellerDashboardLink?.classList.add("hidden");
    }
}

async function logout() {
    try {
        await apiRequest("/api/Auth/logout", {
            method: "POST"
        });
    } finally {
        window.location.href = "/login.html";
    }
}

const loginForm = document.getElementById("loginForm");

if (loginForm) {
    loginForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const messageElement = document.getElementById("loginMessage");

        const payload = {
            usernameOrEmail: document.getElementById("usernameOrEmail").value.trim(),
            password: document.getElementById("password").value
        };

        try {
            await apiRequest("/api/Auth/login", {
                method: "POST",
                body: JSON.stringify(payload)
            });

            messageElement.textContent = "Logged in successfully.";
            messageElement.className = "message success";

            setTimeout(() => {
                window.location.href = "/auctions.html";
            }, 600);
        } catch (error) {
            messageElement.textContent = error.message;
            messageElement.className = "message error";
        }
    });
}

const registerForm = document.getElementById("registerForm");

if (registerForm) {
    registerForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const messageElement = document.getElementById("registerMessage");

        const payload = {
            firstName: document.getElementById("firstName").value.trim(),
            lastName: document.getElementById("lastName").value.trim(),
            username: document.getElementById("username").value.trim(),
            email: document.getElementById("email").value.trim(),
            role: document.getElementById("role").value,
            password: document.getElementById("registerPassword").value,
            confirmPassword: document.getElementById("confirmPassword").value
        };

        try {
            await apiRequest("/api/Auth/register", {
                method: "POST",
                body: JSON.stringify(payload)
            });

            messageElement.textContent = "Account created successfully.";
            messageElement.className = "message success";

            setTimeout(() => {
                window.location.href = "/auctions.html";
            }, 700);
        } catch (error) {
            messageElement.textContent = error.message;
            messageElement.className = "message error";
        }
    });
}

const logoutButton = document.getElementById("logoutButton");

if (logoutButton) {
    logoutButton.addEventListener("click", logout);
}

updateNavigation();

document.querySelectorAll("[data-toggle-password]").forEach((button) => {
    button.addEventListener("click", () => {
        const inputId = button.dataset.togglePassword;
        const input = document.getElementById(inputId);

        if (!input) {
            return;
        }

        const isPassword = input.type === "password";

        input.type = isPassword ? "text" : "password";
        button.classList.toggle("is-visible", isPassword);
        button.setAttribute(
            "aria-label",
            isPassword ? "Hide password" : "Show password"
        );
    });
});