# ─── BUILD STAGE ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj & restore only that, to leverage layer caching
COPY ["home-manager/HomeManager.csproj", "home-manager/"]
RUN dotnet restore "home-manager/HomeManager.csproj"

# copy the rest and publish
COPY . .
RUN dotnet publish "home-manager/HomeManager.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ─── RUNTIME STAGE ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# bring in published app
COPY --from=build /app/publish .

# expose the port Lightsail expects
EXPOSE 80

# tell ASP .NET to listen on 80
ENV ASPNETCORE_URLS=http://+:80

# (optional) leave empty so you can inject your real connection string at deploy time
ENV ConnectionStrings__DefaultConnection=""

ENTRYPOINT ["dotnet", "HomeManager.dll"]