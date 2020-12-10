
```
dotnet ef migrations add {MigrationName} --output-dir .\Migrations\ --startup-project .\TextualDatabaseApi.WebApi\ --project .\TextualDatabaseApi.Persistence\
dotnet ef database update --startup-project .\TextualDatabaseApi.WebApi\ --project .\TextualDatabaseApi.Persistence\


```