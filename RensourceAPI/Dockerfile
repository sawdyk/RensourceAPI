#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RensourceAPI/RensourceAPI.csproj", "RensourceAPI/"]
COPY ["RensourceDomain/RensourceDomain.csproj", "RensourceDomain/"]
COPY ["RensourcePersistence/RensourcePersistence.csproj", "RensourcePersistence/"]
RUN dotnet restore "RensourceAPI/RensourceAPI.csproj"
COPY . .
WORKDIR "/src/RensourceAPI"
RUN dotnet build "RensourceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RensourceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RensourceAPI.dll"]