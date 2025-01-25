# Order Processing System

A .NET 9 solution demonstrating a job queue architecture with AWS SQS, PostgreSQL, and background processing.

## ✨ Features

- **Order Management**
  - Create orders with validation
  - Track order status transitions
  - Filter orders by status
- **Job Queue System**
  - AWS SQS integration
  - Automatic retry mechanism
  - Job status tracking (Queued → Processing → Completed/Failed)
- **Background Processing**
  - Worker service with .NET `BackgroundService`
  - Batch job processing
- **Database**
  - PostgreSQL relational storage
  - Entity Framework Core migrations
- **API**
  - Minimal API endpoints
  - FluentValidation integration
  - DTO-based request/response models

## 🛠 Tech Stack

| Component           | Technologies Used                     |
|---------------------|---------------------------------------|
| **Backend**         | .NET 9, C# 13                         |
| **Database**        | PostgreSQL, Entity Framework Core 8   |
| **Messaging**       | AWS SQS                               |
| **Architecture**    | Clean Architecture |
| **Validation**      | FluentValidation, FluentResults       |
| **Containerization**| Docker, Docker Compose                |

## 📂 Solution Structure

```
OrderProcessing/
├── OrderProcessing.Api/          # Web API Layer
├── OrderProcessing.Domain/       # Domain Models, Interfaces and Errors
├── OrderProcessing.Infrastructure/ # Infrastructure Services
└── OrderProcessing.Worker/       # Background Processing
```

### Key Projects
- **Domain**
  - Models: `Order`, `Job`
  - Interfaces: `IOrderRepository`, `IQueueService`
  - Errors: Domain-specific error types
- **Infrastructure**
  - Data: EF Core DbContext, Repository
  - Messaging: AWS SQS implementation
- **API**
  - Minimal API endpoints
  - Request/Response DTOs
  - Validation handlers
- **Worker**
  - Background job processing
  - Queue polling implementation

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Docker Desktop
- AWS CLI (for local development)

### Setup
```bash
# Start dependencies
docker-compose -f docker-compose.yml up -d

# Apply database migrations
dotnet ef database update --project OrderProcessing.Api
```

### Running
```bash
# Start API
dotnet run --project OrderProcessing.Api

# Start Worker
dotnet run --project OrderProcessing.Worker
```

## 📚 API Reference

| Endpoint                | Method | Description                     |
|-------------------------|--------|---------------------------------|
| `POST /api/orders`      | POST    | Create new order               |
| `GET /api/orders/{id}`  | GET     | Get order details              |
| `GET /api/orders`       | GET     | List all orders                |
| `GET /api/orders/status/{status}` | GET | Filter orders by status |

**Sample Request:**
```http
POST /api/orders
Content-Type: application/json

{
  "customerName": "John Doe",
  "items": [
    {
      "productName": "Sample Product",
      "quantity": 1,
      "unitPrice": 29.99
    }
  ]
}
```

## 🔧 Key Implementation Details

1. **Domain-Centric Design**
   - Business logic isolated in Domain project
   - Explicit error handling with FluentResults
   - Entity validation in domain models

2. **Queue Patterns**
   - SQS message visibility timeouts
   - Dead-letter queue support
   - Batch message processing

3. **Worker Service**
   - Exponential backoff retries
   - Transactional database updates
   - Graceful shutdown handling

## 🚧 Future Improvements

- [ ] Add authentication/authorization
- [ ] Implement OpenAPI documentation
- [ ] Add performance metrics
- [ ] Containerize entire solution
- [ ] Load testing scenarios



## 🚀 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development)

### Run the Full System
1. Clone the repository:
   ```bash
   git clone https://github.com/aenvis/OrderProcessing.git
   cd OrderProcessing
   ```

2. Start all services (PostgreSQL, API, and Worker):
   ```bash
   docker-compose up --build -d
   ```

3. Apply database migrations (in a new terminal):
   ```bash
   docker-compose exec api dotnet ef database update --project OrderProcessing.Infrastructure --startup-project ../OrderProcessing.Api
   ```

4. The system is now ready:
   - **API**: http://localhost:8080
   - **PostgreSQL**: Available at `localhost:5432` (user: postgres, password: postgres)
   - **Worker**: Running in background processing jobs

### Test the System
Create a test order:
```bash
curl -X POST http://localhost:8080/api/orders \
  -H "Content-Type: application/json" \
  -d '{"customerName":"Test Customer","items":[{"productName":"Sample","quantity":1,"unitPrice":29.99}]}'
```

### Stop the Services
```bash
docker-compose down
```

## Development Workflow
For local development without Docker:
```bash
# Start dependencies
docker-compose up -d postgres

# Run API
dotnet run --project OrderProcessing.Api

# Run Worker
dotnet run --project OrderProcessing.Worker
```
