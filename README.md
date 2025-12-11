# Order Ingestion Microservice (ASP.NET Core)

This project is a robust **Order Ingestion Microservice** designed for a cloud-based ERP system. It handles orders from various e-commerce platforms with support for:
- Normalized SQL schema
- Payload validation
- Idempotency via RequestId
- High‑performance Dapper queries
- Async third‑party logistics integration
- RESTful API (`POST /api/v1/orders`)

---

## 🚀 Features

### ✅ **1. Normalized SQL Schema**
The system includes three tables:
- `Customers`
- `Orders`
- `OrderItems`

### ✅ **2. Request Validation**
- Ensures `quantity > 0`, `unitPrice > 0`, valid email.
- Returns meaningful error codes:
  - `1001` → Validation failures
  - `1002` → Duplicate (idempotent) request detected

### ✅ **3. Idempotency Support**
- Uses `RequestId` (GUID) to prevent duplicate order creation.

### ✅ **4. Performance Optimized**
- Uses **Dapper** for fast DB access.
- Transaction-based insertion for order items.

### ✅ **5. Async Processing**
- Simulated third‑party logistics gateway call (2 seconds).
- API remains responsive.

---

## 📦 Folder Structure
```
OrderIngestion/
│
├─ Controllers/
│   └─ OrdersController.cs
├─ Models/
│   ├─ OrderRequest.cs
│   ├─ CustomerDto.cs
│   └─ OrderItemDto.cs
├─ Repositories/
│   └─ OrderRepository.cs
├─ Services/
│   ├─ ILogisticsService.cs
│   └─ MockLogisticsService.cs
├─ Validators/
│   └─ OrderValidator.cs
├─ Program.cs
└─ appsettings.json
```

---

## 🗄️ SQL Schema
Run the following script on your SQL Server:

```sql
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    RequestId UNIQUEIDENTIFIER NOT NULL,
    OrderDate DATETIME2 DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId)
);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice > 0),

    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId)
);
```

---

## ⚙️ Configuration

### **appsettings.json**
Update your database connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=OrderDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
Replace `YOUR_SERVER` with your SQL Server instance.

---

## ▶️ Running the Application

### **Prerequisites**
- .NET 7 SDK or later
- SQL Server (local or remote)
- Visual Studio / VS Code

### **1. Clone the repository**
```bash
git clone https://github.com/yourrepo/order-ingestion-microservice.git
cd order-ingestion-microservice
```

### **2. Restore dependencies**
```bash
dotnet restore
```

### **3. Update connection string**
Open `appsettings.json` and update:
```json
"DefaultConnection": "your connection string"
```

### **4. Run the API**
```bash
dotnet run
```
The API will start at:
```
https://localhost:5001
```

### **5. Open Swagger**
Go to:
```
https://localhost:5001/swagger
```

---

## 📤 API Endpoint
### **POST /api/v1/orders**
Sends an order request to be stored.

### Sample Request
```json
{
  "requestId": "e3b0c442-98fc-1c14-9afb-000000000000",
  "customer": {
    "name": "John Doe",
    "email": "john@example.com"
  },
  "items": [
    {
      "productName": "Laptop",
      "quantity": 1,
      "unitPrice": 1200
    },
    {
      "productName": "Mouse",
      "quantity": 2,
      "unitPrice": 25
    }
  ]
}
```

### Sample Responses
#### **Success**
```json
{
  "orderId": 15,
  "message": "Order created successfully."
}
```

#### **Validation Error (1001)**
```json
{
  "errorCode": 1001,
  "errors": ["Quantity must be > 0"]
}
```

#### **Duplicate Request (1002)**
```json
{
  "errorCode": 1002,
  "message": "Duplicate order detected."
}
```

---

## 🔄 Asynchronous Logistics Integration
A simulated 2‑second call happens in the background:
- API response is not delayed
- The logistics task logs:
```
Order {id} sent to logistics.
```

---

## 🧪 Testing
Use Swagger, Postman, or cURL:
```bash
curl -X POST https://localhost:5001/api/v1/orders \  
-H "Content-Type: application/json" \  
-d "{ your JSON payload }"
```

---

## 📘 Summary
This microservice provides:
- Clean architecture
- High‑performance Dapper operations
- SQL‑normalized data storage
- Request validation
- Idempotency handling
- Async logistics processing
- Developer-friendly Swagger UI

You can extend it further with:
- Message Queue (RabbitMQ/Kafka)
- Authentication/Authorization
- Retry policies
- Outbox pattern

---

If you want, I can also generate a **Dockerfile** and full **deployment guide (Azure/AWS/Kubernetes)**.

