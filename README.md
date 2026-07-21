# Online Auction System

Online Auction System is a web-based auction application developed as a final course project.  
The system simulates the main flow of a real auction platform: sellers can create auction listings, buyers can place bids, users receive notifications, and expired auctions are closed automatically by the backend.

The backend is built with ASP.NET Core Web API and follows an Onion Architecture structure. The project includes role-based authentication, auction management, bid management, notifications, admin features, request validation, and API documentation with Scalar/OpenAPI.

---

## Project Status

The backend is feature-complete for the required course project features.

Implemented features include:

- User registration and login
- Buyer, Seller, and Admin roles
- Auction creation
- Auction image upload
- Category-based auction browsing
- Search and price filtering
- Real-time bid placement
- Bid history per auction
- Outbid notifications
- Automatic auction closing
- Winner determination
- Seller dashboard endpoints
- Seller auction update and cancel operations
- Admin dashboard
- Admin auction cancellation
- Standard API response format
- Global exception handling
- Request validation with FluentValidation

---

## Tech Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- JWT Authentication
- SignalR
- FluentValidation
- AutoMapper
- Scalar / OpenAPI

### Architecture

The project uses an Onion Architecture style:

```txt
src
├── Core
│   ├── OnlineAuctionApp.Domain
│   └── OnlineAuctionApp.Application
├── Infrastructure
│   ├── OnlineAuctionApp.Persistence
│   └── OnlineAuctionApp.Infrastructure
└── Presentation
    └── OnlineAuctionApp.WebAPI
```

### Layer Responsibilities

```txt
Domain
- Main entities
- Enums
- Base domain models

Application
- DTOs
- Service interfaces
- Validators
- Mapping profiles
- Response models
- Custom exceptions

Persistence
- AppDbContext
- Entity configurations
- EF Core migrations
- Database service implementations

Infrastructure
- JWT service
- File service
- Technical service implementations

WebAPI
- Controllers
- Middleware
- SignalR hubs
- API configuration
- Static frontend files
```

---

## Required Features

### F1 - User Registration with Seller and Buyer Roles

Users can register and log in with role-based access.

Supported roles:

- Buyer
- Seller
- Admin

Authentication is handled with JWT. The access token is stored in an HTTP-only cookie for browser-based usage.

---

### F2 - Auction Listing Creation

Sellers can create auction listings with:

- Title
- Description
- Starting price
- End time
- Category

Sellers can also upload images for their auction listings.

---

### F3 - Real-Time Bid Placement with Outbid Notifications

Buyers can place bids on active auctions.

The backend validates that:

- The auction exists
- The auction is active
- The auction has not expired
- The seller cannot bid on their own auction
- The new bid amount is greater than the current price

When a buyer is outbid, the system creates a notification and sends it through SignalR.

---

### F4 - Countdown Timer and Auto-Close on Expiry

The backend includes a background service that checks expired auctions and closes them automatically.

Manual auction closing is also available for Admin users.

---

### F5 - Winner Determination and Notification

When an auction closes, the system checks the highest bid and determines the winner.

Notifications are sent to:

- Seller
- Winner
- Losing bidders

If an auction ends without bids, the seller is notified.

---

### F6 - Seller Dashboard

Sellers can view and manage their own auctions.

Seller operations include:

- View all own auctions
- View active auctions
- View completed auctions
- View dashboard summary
- Update own active auction if it has no bids
- Cancel own active auction if it has no bids

---

### F7 - Bid History

Each auction has a public bid history endpoint.

Users can view the bid history of an auction, including buyer information, bid amount, and bid time.

---

### F8 - Category-Based Browsing and Search

Users can browse active auctions by category and search text.

Supported filters:

- Category
- Search text
- Minimum price
- Maximum price

---

## Extra Features

In addition to the required features, the project includes:

- Admin dashboard
- Admin user listing
- Admin auction listing
- Admin auction cancellation
- Standard response wrapper
- Global exception middleware
- Custom application exceptions
- FluentValidation request validation
- SignalR notification hub
- File upload validation
- HTTP-only cookie authentication
- Standardized 401 and 403 responses

---

## API Response Format

Most API responses follow the same structure.

Success response example:

```json
{
  "isSuccess": true,
  "statusCode": 200,
  "errors": null,
  "data": {}
}
```

Error response example:

```json
{
  "isSuccess": false,
  "statusCode": 400,
  "errors": [
    "Minimum price cannot be greater than maximum price."
  ],
  "data": null
}
```

This format makes the API easier to consume from the frontend because success and error responses follow a consistent structure.

---

## Main API Endpoints

### Auth

```txt
POST /api/Auth/register
POST /api/Auth/login
GET  /api/Auth/me
POST /api/Auth/logout
```

---

### Categories

```txt
GET    /api/Categories
GET    /api/Categories/{id}
POST   /api/Categories
PUT    /api/Categories/{id}
DELETE /api/Categories/{id}
```

---

### Auctions

```txt
GET   /api/Auctions
GET   /api/Auctions/{id}
POST  /api/Auctions
PUT   /api/Auctions/{auctionId}
PATCH /api/Auctions/{auctionId}/cancel
POST  /api/Auctions/{auctionId}/images
```

---

### Bids

```txt
POST /api/Auctions/{auctionId}/bids
GET  /api/Auctions/{auctionId}/bids
```

---

### Seller Dashboard

```txt
GET /api/Auctions/seller/my
GET /api/Auctions/seller/active
GET /api/Auctions/seller/completed
GET /api/Auctions/seller/dashboard-summary
```

---

### Auction Closing

```txt
POST /api/Auctions/close-expired
POST /api/Auctions/{auctionId}/close
```

Manual auction closing endpoints are restricted to Admin users.

---

### Notifications

```txt
GET   /api/Notifications/my
PATCH /api/Notifications/{notificationId}/read
```

---

### Admin

```txt
GET   /api/Admin/dashboard
GET   /api/Admin/users
GET   /api/Admin/auctions
PATCH /api/Admin/auctions/{auctionId}/cancel
```

---

## How to Run the Project

### 1. Clone the repository

```bash
git clone https://github.com/zarifatb-15/FinalProject.git
cd FinalProject
```

---

### 2. Configure the database

Update the connection string in:

```txt
src/Presentation/OnlineAuctionApp.WebAPI/appsettings.Development.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=OnlineAuctionDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

---

### 3. Apply migrations

```bash
dotnet ef database update \
  --project src/Infrastructure/OnlineAuctionApp.Persistence \
  --startup-project src/Presentation/OnlineAuctionApp.WebAPI
```

---

### 4. Run the API

```bash
dotnet run --project src/Presentation/OnlineAuctionApp.WebAPI
```

The API runs locally on:

```txt
http://localhost:5003
```

---

### 5. Open API documentation

Scalar API documentation:

```txt
http://localhost:5003/scalar/v1
```

OpenAPI JSON:

```txt
http://localhost:5003/openapi/v1.json
```

---

## Default Test Users

The project seeds the required roles and a default admin user.

### Admin

```txt
Email: admin@example.com
Username: adminUser
Password: Password123
Role: Admin
```

Example users used during local development and demo testing:

### Seller

```txt
Email: sara@example.com
Password: Password123
Role: Seller
```

### Buyer

```txt
Email: ali@example.com
Password: Password123
Role: Buyer
```

---

## Demo Flow

A suggested demo flow:

1. Login as Admin
2. Open Admin dashboard
3. View users and auctions
4. Login as Seller
5. Create a new auction
6. Upload an auction image
7. View seller dashboard
8. Update an active auction before it receives bids
9. Login as Buyer
10. Place a bid
11. View bid history
12. Login as another Buyer and place a higher bid
13. Check outbid notification
14. Wait for auto-close or close an expired auction as Admin
15. Show winner information
16. Show completed auction in seller dashboard

---

## Business Rules

Important business rules implemented in the backend:

- Only sellers can create auctions
- Sellers cannot bid on their own auctions
- Buyers can only bid on active auctions
- Expired auctions cannot receive bids
- Bid amount must be greater than the current price
- Sellers can update their own active auctions only if there are no bids
- Sellers can cancel their own active auctions only if there are no bids
- Completed and cancelled auctions cannot be changed by sellers
- Admin users can cancel active auctions
- Public auction list only shows active auctions
- Winner is determined based on the highest bid when an auction closes

---

## Git Workflow

The project was developed using separate feature and chore branches.

Important branches include:

```txt
feat/user-registration
feat/category-management
feat/auction-management
feat/auction-images
feat/bid-management
feat/auction-closing
feat/seller-dashboard
feat/auction-search-filter
feat/admin-panel
chore/backend-final-hardening
chore/auth-response-polish
feat/seller-auction-management
docs/readme
```

Pull requests were used to merge completed work into the `main` branch while preserving commit history.

---

## Notes

This project is developed for educational purposes as a final course project.

The current backend focuses on:

- Clean API structure
- Role-based access control
- Auction business rules
- Validation
- Real-time notifications
- Consistent API responses
- A complete auction flow from listing creation to winner determination

Frontend pages are included for demo purposes and can be improved or replaced with a more polished UI.