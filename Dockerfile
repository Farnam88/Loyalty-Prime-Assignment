FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY LoyaltyPrime.WebApi/*.csproj ./
RUN dotnet restore
COPY ./LoyaltyPrime.WebApi ./
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","LoyaltyPrime.WebApi.dll", "--environment=Development"]