FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Orbus/Orbus.csproj", "Orbus/"]
RUN dotnet restore "Orbus/Orbus.csproj"
COPY . .
WORKDIR "/src/Orbus"
RUN dotnet build "Orbus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orbus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orbus.dll"]
