const auctionGrid = document.getElementById("auctionGrid");
const auctionMessage = document.getElementById("auctionMessage");
const auctionFilterForm = document.getElementById("auctionFilterForm");
const clearFiltersButton = document.getElementById("clearFiltersButton");
const categorySelect = document.getElementById("categorySelect");

async function loadCategories() {
  if (!categorySelect) {
    return;
  }

  try {
    const categories = await apiRequest("/api/Categories");

    categories.forEach((category) => {
      const option = document.createElement("option");
      option.value = category.id;
      option.textContent = category.name;
      categorySelect.appendChild(option);
    });
  } catch {
    // Category filter remains usable as "All categories".
  }
}

function buildAuctionQuery() {
  const params = new URLSearchParams();

  const search = document.getElementById("searchInput").value.trim();
  const categoryId = document.getElementById("categorySelect").value;
  const minPrice = document.getElementById("minPriceInput").value;
  const maxPrice = document.getElementById("maxPriceInput").value;

  if (search) {
    params.append("search", search);
  }

  if (categoryId) {
    params.append("categoryId", categoryId);
  }

  if (minPrice) {
    params.append("minPrice", minPrice);
  }

  if (maxPrice) {
    params.append("maxPrice", maxPrice);
  }

  const query = params.toString();
  return query ? `/api/Auctions?${query}` : "/api/Auctions";
}

function renderAuctionCard(auction) {
  const imageUrl = getImageUrl(auction);
  const description =
    auction.description.length > 100
      ? `${auction.description.slice(0, 100)}...`
      : auction.description;

  return `
<article class="auction-card clickable-card" data-auction-id="${auction.id}">       
     <div class="auction-image">
                ${
                  imageUrl
                    ? `<img src="${imageUrl}" alt="${auction.title}">`
                    : `<span>No image uploaded</span>`
                }
            </div>

            <div class="auction-body">
                <div class="auction-meta">
                    <span>${auction.categoryName}</span>
                    <span class="countdown" data-end-time="${auction.endTime}">
                        ${calculateCountdown(auction.endTime)}
                    </span>
                </div>

                <h3>${auction.title}</h3>
                <p>${description}</p>

                <div class="auction-footer">
                    <div>
                        <span class="muted">Current Bid</span>
                        <div class="current-price">${formatPrice(auction.currentPrice)}</div>
                    </div>

                    <a href="/auction-details.html?id=${auction.id}" class="btn btn-secondary">
                        View Details
                    </a>
                </div>
            </div>
        </article>
    `;
}

async function loadAuctions() {
  if (!auctionGrid) {
    return;
  }

  auctionMessage.textContent = "Loading auctions...";
  auctionMessage.className = "message";

  try {
    const auctions = await apiRequest(buildAuctionQuery());

    if (auctions.length === 0) {
      auctionGrid.innerHTML = "";
      auctionMessage.textContent = "No auctions found for your filters.";
      auctionMessage.className = "message";
      return;
    }

    auctionGrid.innerHTML = auctions.map(renderAuctionCard).join("");
    auctionMessage.textContent = "";
  } catch (error) {
    auctionGrid.innerHTML = "";
    auctionMessage.textContent = error.message;
    auctionMessage.className = "message error";
  }
}

function refreshCountdowns() {
  document.querySelectorAll("[data-end-time]").forEach((element) => {
    element.textContent = calculateCountdown(element.dataset.endTime);
  });
}

if (auctionFilterForm) {
  auctionFilterForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    await loadAuctions();
  });
}

if (clearFiltersButton) {
  clearFiltersButton.addEventListener("click", async () => {
    auctionFilterForm.reset();
    await loadAuctions();
  });
}

loadCategories();
loadAuctions();
if (auctionGrid) {
  auctionGrid.addEventListener("click", (event) => {
    const clickedAction = event.target.closest("a, button");

    if (clickedAction) {
      return;
    }

    const card = event.target.closest(".clickable-card[data-auction-id]");

    if (!card) {
      return;
    }

    window.location.href = `/auction-details.html?id=${card.dataset.auctionId}`;
  });
}
setInterval(refreshCountdowns, 30000);
