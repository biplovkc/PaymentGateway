#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MockBank/MockBankApi/MockBankApi.csproj", "src/MockBank/MockBankApi/"]
RUN dotnet restore "src/MockBank/MockBankApi/MockBankApi.csproj"
COPY . .
WORKDIR "/src/src/MockBank/MockBankApi"
RUN dotnet build "MockBankApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockBankApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockBankApi.dll"]