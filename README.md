# 🚛 Logex API

**Logex** is a robust **Shipping & Logistics API** built with **ASP.NET Core 8**. It is designed to manage the complete lifecycle of shipments, including order creation, dynamic rate calculation based on zones, shipment tracking, and payment processing via Stripe.

---

## 📑 Table of Contents

- [Key Features](#-key-features)
- [Technology Stack](#-technology-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)

---

## 🚀 Key Features

* **📦 Smart Shipment Management**
    * Create and manage shipments with detailed sender/receiver data.
    * **Dynamic Pricing Engine**: Automatically calculates shipping costs based on weight, distance (zones), and selected shipment method (Standard, Express, etc.).
    * **Real-time Tracking**: Track shipment status via unique tracking numbers.

* **🔐 Advanced Security**
    * **JWT Authentication**: Secure access using Access Tokens and **Refresh Token Rotation**.
    * **Role-Based Access Control (RBAC)**: Granular permissions for different user roles (`Admin`, `Customer`).

* **💳 Payment Integration**
    * Seamless payment processing using the **Stripe API**.
    * Secure handling of payment intents and transaction records linked to shipments.
    * **Webhooks**: Automated handling of asynchronous payment confirmation events.
---

## 🛠 Technology Stack

* **Framework**: ASP.NET Core Web API (.NET 8)
* **Database**: SQL Server
* **ORM**: Entity Framework Core
* **Authentication**: ASP.NET Core Identity + JWT (Bearer)
* **Payment Gateway**: Stripe .NET SDK
* **Validation**: FluentValidation
---

## 🏗 Architecture

The solution follows a strict **Layered Architecture** to maintain a clean codebase:

* **Presentation Layer**: Controllers responsible for handling HTTP requests and returning DTOs.
* **Service Layer**: Contains business logic, validations, and integrations (e.g., StripeService).
* **Repository Layer**: Abstracts database operations using the **Repository Pattern**.
* **Data Layer**: Entity Framework Core configurations and migrations.

---

## ⚡ Getting Started

1.  **Clone the repository**
    ```bash
    git clone https://github.com/Ahmed-Saalah/Logex.git
    ```

2.  **Configure Environment**
    Update `appsettings.json` with your credentials:
    ```json
    {
      "ConnectionStrings": {
        "ConnectionString": "Your_SQL_Server_Connection_String"
      },
      "Stripe": {
        "SecretKey": "Your_Stripe_Secret_Key",
        "WebhookSecret": "Your_Stripe_Webhook_Secret"
      },
      "JWT": {
        "Key": "Your_Super_Secret_Key",
        "Issuer": "http://localhost:5000",
        "Audience": "http://localhost:5000"
      }
    }
    ```

3.  **Run Migrations & Seed Data**
    The application includes an auto-seeder. Simply apply the migrations:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Project**
    ```bash
    dotnet run
    ```

5.  **Explore the API**
    Navigate to `http://localhost:5000/swagger` to test the endpoints securely.

---

## 🔗 API Endpoints

### **👤 Authentication**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Auth/register` | Register a new user (Customer/Driver) |
| `POST` | `/api/Auth/login` | Authenticate and receive Access & Refresh Tokens |
| `POST` | `/api/Auth/refreshToken/{token}` | Revive an expired Access Token |

### **📦 Shipment**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Shipment` | Create a new shipment |
| `GET` | `/api/Shipment/{id}` | Retrieve shipment details by ID |
| `PUT` | `/api/Shipment/{id}` | Update shipment details |
| `DELETE` | `/api/Shipment/{id}` | Delete a shipment |
| `GET` | `/api/Shipment/tracking/{trackingNumber}` | Track a shipment by unique tracking number |
| `POST` | `/api/Shipment/calculate-rate` | Calculate shipping cost without creating an order |

### **💳 Payment**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Payment/checkout` | Process a payment for a shipment |
| `POST` | `/api/webhooks/stripe` | Stripe Webhook listener for payment confirmation |

### **🚚 Shipment Methods**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/ShipmentMethods` | Get all available shipment methods |
| `POST` | `/api/ShipmentMethods` | Create a new shipment method (Admin) |
| `GET` | `/api/ShipmentMethods/{id}` | Get shipment method by ID |
| `PUT` | `/api/ShipmentMethods/{id}` | Update shipment method |
| `PATCH` | `/api/ShipmentMethods/{id}/toggle-status` | Enable/Disable a shipment method |
| `GET` | `/api/ShipmentMethods/{id}/cost` | Get cost for a specific method |
| `GET` | `/api/ShipmentMethods/admin/all` | Retrieve all methods including disabled ones (Admin) |

### **🏙️ Cities & Zones**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Cities` | Get list of supported cities |
| `POST` | `/api/Cities` | Add a new city (Admin) |
| `GET` | `/api/Cities/{id}` | Get city details |
| `PUT` | `/api/Cities/{id}` | Update city information |
| `PATCH` | `/api/Cities/{id}/toggle-status` | Enable/Disable a city |
| `GET` | `/api/Zones` | Get all shipping zones |
| `POST` | `/api/Zones` | Create a new zone |
| `GET` | `/api/Zones/{id}` | Get zone details |
| `PUT` | `/api/Zones/{id}` | Update zone |
| `PATCH` | `/api/Zones/{id}/toggle-status` | Enable/Disable a zone |

### **💲 Zone Rates**
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/ZoneRates` | Get all zone rates |
| `POST` | `/api/ZoneRates` | Define a new rate for a zone |
| `GET` | `/api/ZoneRates/{id}` | Get specific rate details |
| `PUT` | `/api/ZoneRates/{id}` | Update a zone rate |
| `DELETE` | `/api/ZoneRates/{id}` | Remove a zone rate |
