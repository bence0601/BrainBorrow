#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BrainBorrowAPI/BrainBorrowAPI.csproj", "BrainBorrowAPI/"]
RUN dotnet restore "./BrainBorrowAPI/./BrainBorrowAPI.csproj"
COPY . .
WORKDIR "/src/BrainBorrowAPI"
RUN dotnet build "./BrainBorrowAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Add test project
COPY ["BrainBorrowApiTest/BrainBorrowApiTest.csproj", "BrainBorrowApiTest/"]
RUN dotnet restore "./BrainBorrowApiTest/BrainBorrowApiTest.csproj"
COPY . .
WORKDIR "/src/BrainBorrowApiTest"
RUN dotnet build "./BrainBorrowApiTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BrainBorrowAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Add test project publish
RUN dotnet publish "./BrainBorrowApiTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrainBorrowAPI.dll"]
