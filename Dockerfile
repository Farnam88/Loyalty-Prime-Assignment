FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build
WORKDIR /app
COPY LoyaltyPrime.WebApi/*.csproj ./
RUN dotnet restore
COPY ./LoyaltyPrime.WebApi ./
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","LoyaltyPrime.WebApi.dll", "--environment=Development"]