# ZipCo Test

## Prerequisites to Run the code locally
1. Visual Studio 2019
2. Docker 19.03.1 and make sure to switch to Linux Containers


## Run API Integration Test

1. Following connection string should be used in the ZipPay.Users.Api project's appSettings.Development.json file,
  which creates an in-memory database.

```
"DefaultDBContext": "Server=(localdb)\\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0"
```

Note : After change the connection string you can run all the tests including Unit Tests

## Run the Application on Visual Studio with Docker-Compose

1. Make sure to set the docker-compose project as the Start Up project.
2. Following connection string should be used in the ZipPay.Users.Api project's appSettings.Development.json file.

```
"DefaultDBContext": "Server=sql.data;Database=ZapUser;User=sa;Password=testUser@12345;"
```
3. Run the application.
4. You will see the Swagger UI.

