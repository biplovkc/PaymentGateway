FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /

COPY "PaymentGateway.sln" "PaymentGateway.sln"

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds

#Copy BuildingBlocks
COPY "src/BuildingBlocks/Core/Biplov.Common.Core/Biplov.Common.Core.csproj" "src/BuildingBlocks/Core/Biplov.Common.Core/Biplov.Common.Core.csproj"
COPY "src/BuildingBlocks/EventBus/Biplov.EventBus/Biplov.EventBus.csproj" "src/BuildingBlocks/EventBus/Biplov.EventBus/Biplov.EventBus.csproj"
COPY "src/BuildingBlocks/EventBus/Biplov.EventBus.RabbitMQ/Biplov.EventBus.RabbitMQ.csproj" "src/BuildingBlocks/EventBus/Biplov.EventBus.RabbitMQ/Biplov.EventBus.RabbitMQ.csproj"
COPY "src/BuildingBlocks/Bank/Biplov.BankService/Biplov.BankService.csproj" "src/BuildingBlocks/Bank/Biplov.BankService/Biplov.BankService.csproj"
COPY "src/BuildingBlocks/Bank/Biplov.MockBank/Biplov.MockBank.csproj" "src/BuildingBlocks/Bank/Biplov.MockBank/Biplov.MockBank.csproj"
COPY "src/BuildingBlocks/RiskAnalysis/Biplov.RiskAnalysis/Biplov.RiskAnalysis.csproj" "src/BuildingBlocks/RiskAnalysis/Biplov.RiskAnalysis/Biplov.RiskAnalysis.csproj"
COPY "src/BuildingBlocks/RiskAnalysis/Biplov.MockRiskAnalysis/Biplov.MockRiskAnalysis.csproj" "src/BuildingBlocks/RiskAnalysis/Biplov.MockRiskAnalysis/Biplov.MockRiskAnalysis.csproj"

#Copy MerchantIdentity
COPY "src/MerchantIdentity/Api/Biplov.MerchantIdentityApi/Biplov.MerchantIdentityApi.csproj" "src/MerchantIdentity/Api/Biplov.MerchantIdentityApi/Biplov.MerchantIdentityApi.csproj"
COPY "src/MerchantIdentity/Application/Biplov.MerchantIdentity.Application/Biplov.MerchantIdentity.Application.csproj" "src/MerchantIdentity/Application/Biplov.MerchantIdentity.Application/Biplov.MerchantIdentity.Application.csproj"
COPY "src/MerchantIdentity/Domain/Biplov.MerchantIdentity.Domain/Biplov.MerchantIdentity.Domain.csproj" "src/MerchantIdentity/Domain/Biplov.MerchantIdentity.Domain/Biplov.MerchantIdentity.Domain.csproj"
COPY "src/MerchantIdentity/Infrastructure/Biplov.MerchantIdentity.Infrastructure/Biplov.MerchantIdentity.Infrastructure.csproj" "src/MerchantIdentity/Infrastructure/Biplov.MerchantIdentity.Infrastructure/Biplov.MerchantIdentity.Infrastructure.csproj"

#Copy PaymentGateway
COPY "src/PaymentGateway/Api/Biplov.PaymentGatewayApi/Biplov.PaymentGatewayApi.csproj" "src/PaymentGateway/Api/Biplov.PaymentGatewayApi/Biplov.PaymentGatewayApi.csproj"
COPY "src/PaymentGateway/Application/Biplov.PaymentGateway.Application/Biplov.PaymentGateway.Application.csproj" "src/PaymentGateway/Application/Biplov.PaymentGateway.Application/Biplov.PaymentGateway.Application.csproj"
COPY "src/PaymentGateway/Domain/Biplov.PaymentGateway.Domain/Biplov.PaymentGateway.Domain.csproj" "src/PaymentGateway/Domain/Biplov.PaymentGateway.Domain/Biplov.PaymentGateway.Domain.csproj"
COPY "src/PaymentGateway/Infrastructure/Biplov.PaymentGateway.Infrastructure/Biplov.PaymentGateway.Infrastructure.csproj" "src/PaymentGateway/Infrastructure/Biplov.PaymentGateway.Infrastructure/Biplov.PaymentGateway.Infrastructure.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "src/NuGet.config" "src/NuGet.config"
RUN dotnet restore "PaymentGateway.sln"

COPY . .

WORKDIR "/src/MerchantIdentity/Api/Biplov.MerchantIdentityApi"
RUN dotnet publish --no-restore -c Release -o /app


FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/ .
ENTRYPOINT ["dotnet", "Biplov.MerchantIdentityApi.dll"]