FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["home-manager/HomeManager.csproj", "home-manager/"]
RUN dotnet restore "home-manager/HomeManager.csproj"
COPY . .
RUN dotnet build "home-manager/HomeManager.csproj" -c Release -o /app/build
RUN dotnet publish "home-manager/HomeManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__DefaultConnection=""
ENTRYPOINT ["dotnet", "HomeManager.dll"]