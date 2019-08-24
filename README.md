# ZipCo Test

# Pre-Requistes to Run the code locally
1. Visual Studio 2019
2. Docker 19.03.1 and make sure to switch to Linux Containers

#Run API Integration Test

1. Uncomment the following connection string in the appSettings.Development.json
'''
"DefaultDBContext": "Server=(localdb)\\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0"
'''
ZipPay.Users.Api.IntegrationTest

