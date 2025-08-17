FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["BankApp.API2/BankApp.API2.csproj", "BankApp.API2/"]
RUN dotnet restore "BankApp.API2/BankApp.API2.csproj"

COPY . .

WORKDIR "/src/BankApp.API2"

RUN dotnet build "BankApp.API2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankApp.API2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankApp.API2.dll"]

