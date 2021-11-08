FROM mcr.microsoft.com/dotnet/core/sdk:2.2.203 AS build
WORKDIR /app
COPY ./src .
RUN dotnet publish "./ZipPay.API/ZipPay.API.csproj" -c Release -o /dist

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
COPY --from=build /dist .
ENTRYPOINT ["dotnet", "ZipPay.API.dll"]
