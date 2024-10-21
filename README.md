"ConnectionStrings": {
  "DefaultConnection": "Server=Sundetkali;Database=test;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
}

dotnet ef migrations add InitialCreate

dotnet ef migrations add AddProductGroups

dotnet ef migrations add AddProducts

dotnet ef database update
