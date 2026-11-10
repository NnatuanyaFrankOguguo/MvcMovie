# Frank's Movies

## Run the application

```powershell
dotnet restore
dotnet ef database update
dotnet run --launch-profile https
```

Open the HTTPS URL printed by the application. The app includes the HelloWorld tutorial controller and view, movie CRUD, SQLite persistence, search, genre filtering, year filtering, validation, and seeded movies.
