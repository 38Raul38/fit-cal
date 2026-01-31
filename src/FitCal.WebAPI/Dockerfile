FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FitCal.WebAPI/FitCal.WebAPI.csproj", "src/FitCal.WebAPI/"]
COPY ["src/FitCal.Application/FitCal.Application.csproj", "src/FitCal.Application/"]
COPY ["src/FitCal.Persistence/FitCal.Persistence.csproj", "src/FitCal.Persistence/"]
COPY ["src/FitCal.Core/FitCal.Core.csproj", "src/FitCal.Core/"]
RUN dotnet restore "src/FitCal.WebAPI/FitCal.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/FitCal.WebAPI"
RUN dotnet build "./FitCal.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FitCal.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitCal.WebAPI.dll"]
