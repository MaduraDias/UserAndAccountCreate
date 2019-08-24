# ZipCo Test

## prerequisite to Run the code locally
1. Visual Studio 2019
2. Docker 19.03.1 and make sure to switch to Linux Containers


## Run API Integration Test

1. Use the following connection string in the appSettings.Development.json file in the ZipPay.Users.Api project, to run the Integration test, which creates an in-memory database.

```
"DefaultDBContext": "Server=(localdb)\\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0"
```

Note : After change the connection string you can run all the tests including Unit Tests

## Run the Application on Visual Studio with Docker-Compose

1. Make sure the docker-compose project is the Start Up project
2. Use the following connection string in the appSettings.Development.json file in the ZipPay.Users.Api project.

```
"DefaultDBContext": "Server=sql.data;Database=ZapUser;User=sa;Password=testUser@12345;"
```
3. Run the application.
4. You can use Postman to test the Api

