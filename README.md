# SolarBatteryMeter API

The **SolarBatteryMeter API** is a .NET-based application designed to provide calculation, monitoring, and data access services for solar battery systems.  
It follows a **Clean Architecture** approach, ensuring separation of concerns, testability, and scalability.

---

## 📂 Project Structure

The solution follows a **Clean Architecture** pattern:

- **API (`SolarBatteryMeterApi`)**  
  The entry point of the application, exposing endpoints for external interaction.

- **Application (`Calculator`)**  
  Contains business logic and use cases.

- **Domain (`Calculator.Abstractions`)**  
  Core domain models and abstractions, independent of any frameworks.

- **Infrastructure (`Logger`)**  
  Provides implementations for logging and persistence.

- **Presentation**  
  Two WinForms applications for user interaction:  
  - `BatteryFarmCalculator`  
  - `SolarBatteryMeter`

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/SolarBatteryMeterApi.git
   cd SolarBatteryMeterApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run the API:
   ```bash
   cd SolarBatteryMeterApi
   dotnet run
   ```

---

## ⚡ Running with Docker

The API includes a `Dockerfile` for containerized deployment.

```bash
# Build the image
docker build -t solarbatterymeter-api .

# Run the container
docker run -p 5000:5000 solarbatterymeter-api
```

API will be available at:  
👉 http://localhost:5000

---

## 🛠 Configuration

Configuration settings (ports, logging, connection strings, etc.) are managed via:

- `appsettings.json`
- Environment variables (for Docker/Kubernetes deployments)

---

## 📖 API Documentation

After starting the API, Swagger UI will be available at:

👉 http://localhost:5000/swagger

---

## 📝 Logging & Error Handling

The solution uses the **Logger** project for centralized logging and exception handling.  
All API requests and errors are logged consistently across layers.

---

## 🤝 Contributing

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Commit changes (`git commit -m 'Add feature'`)
4. Push branch (`git push origin feature/my-feature`)
5. Open a Pull Request

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).
