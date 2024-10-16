#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BaGet/BaGet.csproj", "BaGet/"]
COPY ["src/BaGet.Core/BaGet.Core.csproj", "BaGet.Core/"]
COPY ["src/BaGet.Protocol/BaGet.Protocol.csproj", "BaGet.Protocol/"]
COPY ["src/BaGet.Database.Sqlite/BaGet.Database.Sqlite.csproj", "BaGet.Database.Sqlite/"]
RUN dotnet restore "BaGet/BaGet.csproj"
COPY ./src/ .
WORKDIR "/src/BaGet"
RUN dotnet publish "BaGet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BaGet.dll"]