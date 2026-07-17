function sellerDashboardPage() {
    return {
        loading: true,
        error: "",
        user: null,
        categories: [],
        summary: {
            activeAuctionCount: 0,
            completedAuctionCount: 0,
            totalAuctionCount: 0
        },
        activeAuctions: [],
        completedAuctions: [],
        activeTab: "active",
        showCreateForm: false,
        creating: false,
        selectedImage: null,
        successMessage: "",
        formError: "",
        createForm: {
            title: "",
            description: "",
            startingPrice: "",
            endTime: "",
            categoryId: ""
        },

        async init() {
            try {
                await this.loadCurrentUser();

                if (!this.user) {
                    this.error = "You need to login as a seller to access this page.";
                    return;
                }

                if (!this.user.roles.includes("Seller")) {
                    this.error = "Only seller accounts can access this dashboard.";
                    return;
                }

                await Promise.all([
                    this.loadCategories(),
                    this.loadDashboardData()
                ]);

                this.refreshCountdowns();
                setInterval(() => this.refreshCountdowns(), 30000);
            } catch (error) {
                this.error = error.message;
            } finally {
                this.loading = false;
            }
        },

        async loadCurrentUser() {
            this.user = await apiRequest("/api/Auth/me");
        },

        async loadCategories() {
            this.categories = await apiRequest("/api/Categories");
        },

        async loadDashboardData() {
            const [summary, activeAuctions, completedAuctions] = await Promise.all([
                apiRequest("/api/Auctions/seller/dashboard-summary"),
                apiRequest("/api/Auctions/seller/active"),
                apiRequest("/api/Auctions/seller/completed")
            ]);

            this.summary = summary;
            this.activeAuctions = activeAuctions;
            this.completedAuctions = completedAuctions;
        },

        handleImageChange(event) {
            this.selectedImage = event.target.files[0] || null;
        },

        async createAuction() {
            this.formError = "";
            this.successMessage = "";

            const startingPrice = Number(this.createForm.startingPrice);

            if (!Number.isFinite(startingPrice) || startingPrice <= 0) {
                this.formError = "Starting price must be greater than zero.";
                return;
            }

            if (!this.createForm.endTime) {
                this.formError = "End time is required.";
                return;
            }

            const endTime = new Date(this.createForm.endTime);

            if (endTime <= new Date()) {
                this.formError = "End time must be in the future.";
                return;
            }

            this.creating = true;

            try {
                const auction = await apiRequest("/api/Auctions", {
                    method: "POST",
                    body: JSON.stringify({
                        title: this.createForm.title.trim(),
                        description: this.createForm.description.trim(),
                        startingPrice,
                        endTime: endTime.toISOString(),
                        categoryId: this.createForm.categoryId
                    })
                });

                if (this.selectedImage) {
                    await this.uploadAuctionImage(auction.id, this.selectedImage);
                }

                this.resetCreateForm();
                await this.loadDashboardData();

                this.activeTab = "active";
                this.showCreateForm = false;
                this.successMessage = "Auction listing created successfully.";
            } catch (error) {
                this.formError = error.message;
            } finally {
                this.creating = false;
            }
        },

        async uploadAuctionImage(auctionId, imageFile) {
            const formData = new FormData();
            formData.append("image", imageFile);

            await apiRequest(`/api/Auctions/${auctionId}/images`, {
                method: "POST",
                body: formData,
                skipJsonContentType: true
            });
        },

        resetCreateForm() {
            this.createForm = {
                title: "",
                description: "",
                startingPrice: "",
                endTime: "",
                categoryId: ""
            };

            this.selectedImage = null;

            const fileInput = document.querySelector('input[type="file"]');
            if (fileInput) {
                fileInput.value = "";
            }
        },

        refreshCountdowns() {
            setTimeout(() => {
                document.querySelectorAll("[data-end-time]").forEach((element) => {
                    element.textContent = calculateCountdown(element.dataset.endTime);
                });
            }, 50);
        },

        formatPriceValue(value) {
            return formatPrice(value);
        }
    };
}