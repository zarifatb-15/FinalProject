# Online Auction System

Online Auction System is a full-stack web application developed as a final course project.  
The system recreates the main flow of a real online auction platform where sellers can publish auction listings, buyers can place competitive bids, users receive notifications, and the backend automatically closes expired auctions and determines winners.

The project is implemented with **ASP.NET Core Web API** on the backend and a static **HTML/CSS/JavaScript frontend** served from the WebAPI project.  
The backend follows an **Onion Architecture** approach and includes authentication, authorization, auction management, bid management, category management, notifications, admin features, validation, global exception handling, and API documentation.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Implemented Features](#implemented-features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Required Course Features](#required-course-features)
- [Additional Features](#additional-features)
- [Frontend Pages](#frontend-pages)
- [API Response Format](#api-response-format)
- [Main API Endpoints](#main-api-endpoints)
- [Business Rules](#business-rules)
- [How to Run the Project](#how-to-run-the-project)
- [Default Test Users](#default-test-users)
- [Demo Flow](#demo-flow)
- [Git Workflow](#git-workflow)
- [Notes](#notes)

---

## Project Overview

The goal of this project is to build an online auction platform that simulates the excitement and structure of a live auction in a web-based environment.

Sellers can create auction listings by providing auction details such as title, description, starting price, category, end time, and images. Buyers can browse active auctions, filter listings, view auction details, place bids, and follow bid history. The system validates every bid, notifies outbid users, automatically closes expired auctions, and determines the winner based on the highest bid.

The project also includes seller dashboard functionality and admin management features for monitoring platform activity.

---

## Implemented Features

The application includes the following implemented features:

- User registration and login
- Buyer, Seller, and Admin roles
- JWT authentication with HTTP-only cookie support
- Role-based authorization
- Auction listing creation
- Auction image upload
- Auction browsing
- Category-based filtering
- Search and price filtering
- Real-time bid placement
- Bid validation
- Bid history per auction
- Outbid notifications
- SignalR notification hub
- Automatic auction closing
- Winner determination
- Seller dashboard
- Seller auction update
- Seller auction cancellation
- Admin dashboard
- Admin users overview
- Admin auctions overview
- Admin auction cancellation
- Category management
- Standard API response wrapper
- Global exception handling
- FluentValidation request validation
- Scalar/OpenAPI documentation
- Static frontend integrated with backend APIs
- Dark/light mode support
- Language switcher
- Role-based frontend navigation

---

## Technology Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- JWT Authentication
- HTTP-only cookie authentication
- SignalR
- FluentValidation
- AutoMapper
- Scalar / OpenAPI

### Frontend

- HTML
- CSS
- JavaScript
- Fetch API
- Static files served from `wwwroot`
- Responsive UI pages
- Dark/light mode
- Language switcher
- Role-based navigation

### Development Tools

- .NET SDK
- Entity Framework Core CLI
- SQL Server
- Git
- GitHub
- Visual Studio Code

---

## Architecture

The project follows an Onion Architecture structure.  
This keeps domain logic, application logic, infrastructure concerns, persistence logic, and presentation layer separated.

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

---

## Layer Responsibilities

### Domain Layer

The Domain layer contains the main business models and core domain definitions.

Responsibilities:

- Domain entities
- Base entity model
- Enums
- Core business object structure

Main examples:

- Auction
- Bid
- Category
- AuctionImage
- Notification
- AppUser
- AppRole
- AuctionStatus

---

### Application Layer

The Application layer contains contracts, DTOs, validation rules, mappings, response models, and application-level abstractions.

Responsibilities:

- DTO definitions
- Service interfaces
- Validation rules
- Mapping profiles
- Response wrapper models
- Custom application exceptions

Main examples:

- Auth DTOs
- Auction DTOs
- Bid DTOs
- Category DTOs
- Notification DTOs
- FluentValidation validators
- AutoMapper profile
- ResponseModel
- ResponseModelHelper

---

### Persistence Layer

The Persistence layer handles database access and Entity Framework Core configuration.

Responsibilities:

- AppDbContext
- Entity configurations
- EF Core migrations
- Repository/database service implementations
- Identity database integration

Main examples:

- AppDbContext
- Auction configuration
- Bid configuration
- Category configuration
- Identity tables
- Database migrations

---

### Infrastructure Layer

The Infrastructure layer contains technical services that support the application.

Responsibilities:

- JWT token generation
- File upload service
- Technical service implementations
- External infrastructure-related services

Main examples:

- JwtService
- FileService

---

### WebAPI Layer

The WebAPI layer is the entry point of the application.

Responsibilities:

- API controllers
- Middleware
- Authentication/authorization configuration
- SignalR hub configuration
- Static frontend hosting
- Scalar/OpenAPI setup
- Global exception middleware

Main examples:

- AuthController
- AuctionsController
- CategoriesController
- NotificationsController
- AdminController
- NotificationHub
- GlobalExceptionMiddleware
- Static frontend files under `wwwroot`

---

## Required Course Features

### F1 - User Registration with Seller and Buyer Roles

The system supports user registration and login with role-based access control.

Supported roles:

- Buyer
- Seller
- Admin

Authentication is handled with JWT.  
For browser usage, the token is stored in an HTTP-only cookie, which improves security by avoiding token storage in localStorage.

---

### F2 - Auction Listing Creation

Sellers can create auction listings with the required auction information.

Auction creation includes:

- Title
- Description
- Starting price
- End time
- Category

Sellers can also upload images for auction listings.

---

### F3 - Real-Time Bid Placement with Outbid Notifications

Buyers can place bids on active auctions.

The backend validates that:

- The auction exists
- The auction is active
- The auction has not expired
- The bidder is a Buyer
- The seller cannot bid on their own auction
- The bid amount is greater than the current price

When a higher bid is placed, the previous highest bidder receives an outbid notification.  
SignalR is used for real-time notification delivery.

---

### F4 - Countdown Timer and Auto-Close on Expiry

Each auction has an end time and countdown behavior on the frontend.

The backend includes a background service that checks expired auctions and closes them automatically.

Admin users can also trigger auction closing manually where applicable.

---

### F5 - Winner Determination and Notification

When an auction closes, the system checks the highest bid and determines the winner.

Notifications are created for:

- Seller
- Winner
- Losing bidders

If an auction ends without any bids, the seller is notified.

---

### F6 - Seller Dashboard

Sellers can manage their own auctions from the seller dashboard.

Seller dashboard functionality includes:

- View all own auctions
- View active auctions
- View completed auctions
- View dashboard summary
- Create auction
- Upload auction image
- Update own active auction if it has no bids
- Cancel own active auction if it has no bids

---

### F7 - Bid History

Each auction has a public bid history endpoint.

Users can view:

- Bidder information
- Bid amount
- Bid time

This improves transparency and makes the auction process auditable.

---

### F8 - Category-Based Browsing and Search

Users can browse and search active auctions.

Supported filters include:

- Category
- Search text
- Minimum price
- Maximum price

The public auction list only displays active auctions.

---

## Additional Features

In addition to the required course features, the project includes several extra improvements:

- Admin dashboard
- Admin users overview
- Admin auctions overview
- Admin auction cancellation
- Category management
- Standard response wrapper
- Global exception handling
- Custom application exceptions
- FluentValidation request validation
- SignalR notification hub
- File upload validation
- HTTP-only cookie authentication
- Standardized 401 and 403 responses
- Static frontend integrated with backend APIs
- Dark/light mode
- Language switcher
- Role-based navigation

---

## Frontend Pages

The frontend is served directly from the WebAPI project under:

```txt
src/Presentation/OnlineAuctionApp.WebAPI/wwwroot
```

Main frontend pages:

```txt
index.html
auctions.html
auction-details.html
login.html
register.html
seller-dashboard.html
admin-dashboard.html
categories.html
notifications.html
unauthorized.html
not-found.html
```

The frontend communicates with backend endpoints through the Fetch API.

Authenticated requests use cookie-based authentication:

```txt
credentials: "include"
```

This allows the frontend to work with HTTP-only JWT cookies without storing tokens in localStorage.

---

## API Response Format

Most API responses follow a consistent response model.

### Success Response

```json
{
  "isSuccess": true,
  "statusCode": 200,
  "errors": null,
  "data": {}
}
```

### Error Response

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

Using a consistent response model makes frontend integration easier and improves API reliability.

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

### SignalR

```txt
/hubs/notifications
```

SignalR is used for real-time notification delivery.

---

## Business Rules

The backend contains business rules to protect the auction process and keep data consistent.

Important rules:

- Only sellers can create auctions
- Only buyers can place bids
- Sellers cannot bid on their own auctions
- Buyers can only bid on active auctions
- Expired auctions cannot receive bids
- Bid amount must be greater than the current price
- Sellers can update their own active auctions only if there are no bids
- Sellers can cancel their own active auctions only if there are no bids
- Completed auctions cannot be modified by sellers
- Cancelled auctions cannot receive bids
- Admin users can cancel active auctions
- Public auction list only shows active auctions
- Winner is determined from the highest bid when an auction closes
- Notifications are created for important auction events

---

## How to Run the Project

### 1. Clone the Repository

```bash
git clone https://github.com/zarifatb-15/FinalProject.git
cd FinalProject
```

---

### 2. Configure the Database

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

### 3. Apply Migrations

```bash
dotnet ef database update \
  --project src/Infrastructure/OnlineAuctionApp.Persistence \
  --startup-project src/Presentation/OnlineAuctionApp.WebAPI
```

---

### 4. Run the Application

```bash
dotnet run --project src/Presentation/OnlineAuctionApp.WebAPI
```

The application runs locally at:

```txt
http://localhost:5003
```

---

### 5. Open API Documentation

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

The project seeds roles and a default admin user.

### Admin

```txt
Email: admin@example.com
Username: adminUser
Password: Password123
Role: Admin
```

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

### Second Buyer

```txt
Email: leyla@example.com
Password: Password123
Role: Buyer
```

---

## Demo Flow

A recommended demo flow for presentation:

1. Open the home page
2. Browse active auctions
3. Use category/search/price filters
4. Login as Seller
5. Open Seller Dashboard
6. Create a new auction
7. Upload an auction image
8. View seller auction list
9. Logout
10. Login as Buyer
11. Open auction details
12. Place a bid
13. View bid history
14. Login as another Buyer
15. Place a higher bid
16. Show outbid notification
17. Login as Admin
18. Open Admin Dashboard
19. View platform statistics
20. View users and auctions
21. Cancel an active auction if needed
22. Show category management

---

## Security and Validation

The project includes several security and validation measures:

- Role-based endpoint protection
- HTTP-only cookie authentication
- Server-side validation with FluentValidation
- Global exception handling
- Standardized error responses
- File upload validation
- Business rule checks before bid placement
- Authorization checks for seller-owned auction operations
- Admin-only management endpoints

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
feat/frontend-marketplace-ui
```

Pull requests and branch-based development were used to preserve project history.

---

## Notes

This project is developed for educational purposes as a final course project.

The system demonstrates:

- Clean architecture structure
- Role-based authentication and authorization
- Complete auction flow
- Seller and buyer workflows
- Real-time notifications
- Automatic auction closing
- Winner determination
- Admin monitoring features
- Category management
- Frontend and backend integration
- Consistent API response handling

Email notifications are not implemented as a separate SMTP provider.  
The project currently uses real-time in-app notifications with SignalR. An email provider can be added later as an infrastructure service.

---

## Author

Final course project by Zarifa Babayeva.
