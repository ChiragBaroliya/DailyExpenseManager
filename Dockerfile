FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DailyExpenseManager.API/DailyExpenseManager.API.csproj", "DailyExpenseManager.API/"]
COPY ["DailyExpenseManager.Application/DailyExpenseManager.Application.csproj", "DailyExpenseManager.Application/"]
COPY ["DailyExpenseManager.Domain/DailyExpenseManager.Domain.csproj", "DailyExpenseManager.Domain/"]
COPY ["DailyExpenseManager.Infrastructure/DailyExpenseManager.Infrastructure.csproj", "DailyExpenseManager.Infrastructure/"]

RUN dotnet restore "DailyExpenseManager.API/DailyExpenseManager.API.csproj"

COPY . .
WORKDIR "/src/DailyExpenseManager.API"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DailyExpenseManager.API.dll"]
