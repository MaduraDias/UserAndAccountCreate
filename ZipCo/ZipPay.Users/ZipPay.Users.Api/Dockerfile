FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["ZipPay.Users.Api/ZipPay.Users.Api.csproj", "ZipPay.Users.Api/"]
COPY ["ZipPay.Users.Entities/ZipPay.Users.Entities.csproj", "ZipPay.Users.Entities/"]
COPY ["ZipPay.Users.BusinessService/ZipPay.Users.BusinessService.csproj", "ZipPay.Users.BusinessService/"]
COPY ["ZipPay.Users.DataServices/ZipPay.Users.DataServices.csproj", "ZipPay.Users.DataServices/"]
RUN dotnet restore "ZipPay.Users.Api/ZipPay.Users.Api.csproj"
COPY . .
WORKDIR "/src/ZipPay.Users.Api"
RUN dotnet build "ZipPay.Users.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ZipPay.Users.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ZipPay.Users.Api.dll"]
