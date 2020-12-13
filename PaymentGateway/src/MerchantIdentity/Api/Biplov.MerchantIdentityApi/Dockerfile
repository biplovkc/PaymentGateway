FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

COPY "PaymentGateway.sln" "PaymentGateway.sln"

WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds

#Copy BuildingBlocks
COPY "BuildingBlocks/Core/Biplov.Common.Core/Biplov.Common.Core.csproj" "BuildingBlocks/Core/Biplov.Common.Core/Biplov.Common.Core.csproj"
COPY "BuildingBlocks/EventBus/Biplov.EventBus/Biplov.EventBus.csproj" "BuildingBlocks/EventBus/Biplov.EventBus/Biplov.EventBus.csproj"
COPY "BuildingBlocks/EventBus/Biplov.EventBus.RabbitMQ/Biplov.EventBus.RabbitMQ.csproj" "BuildingBlocks/EventBus/Biplov.EventBus.RabbitMQ/Biplov.EventBus.RabbitMQ.csproj"
COPY "BuildingBlocks/Bank/Biplov.BankService/Biplov.BankService.csproj" "BuildingBlocks/Bank/Biplov.BankService/Biplov.BankService.csproj"
COPY "BuildingBlocks/Bank/Biplov.MockBank/Biplov.MockBank.csproj" "BuildingBlocks/Bank/Biplov.MockBank/Biplov.MockBank.csproj"
COPY "BuildingBlocks/RiskAnalysis/Biplov.RiskAnalysis/Biplov.RiskAnalysis.csproj" "BuildingBlocks/RiskAnalysis/Biplov.RiskAnalysis/Biplov.RiskAnalysis.csproj"
COPY "BuildingBlocks/RiskAnalysis/Biplov.MockRiskAnalysis/Biplov.MockRiskAnalysis.csproj" "BuildingBlocks/RiskAnalysis/Biplov.MockRiskAnalysis/Biplov.MockRiskAnalysis.csproj"

#Copy MerchantIdentity
COPY "MerchantIdentity/Api/Biplov.MerchantIdentityApi/Biplov.MerchantIdentityApi.csproj" "MerchantIdentity/Api/Biplov.MerchantIdentityApi/Biplov.MerchantIdentityApi.csproj"
COPY "MerchantIdentity/Application/Biplov.MerchantIdentity.Application/Biplov.MerchantIdentity.Application.csproj" "MerchantIdentity/Application/Biplov.MerchantIdentity.Application/Biplov.MerchantIdentity.Application.csproj"
COPY "MerchantIdentity/Domain/Biplov.MerchantIdentity.Domain/Biplov.MerchantIdentity.Domain.csproj" "MerchantIdentity/Domain/Biplov.MerchantIdentity.Domain/Biplov.MerchantIdentity.Domain.csproj"
COPY "MerchantIdentity/Infrastructure/Biplov.MerchantIdentity.Infrastructure/Biplov.MerchantIdentity.Infrastructure.csproj" "MerchantIdentity/Infrastructure/Biplov.MerchantIdentity.Infrastructure/Biplov.MerchantIdentity.Infrastructure.csproj"

#Copy PaymentGateway
COPY "PaymentGateway/Api/Biplov.PaymentGatewayApi/Biplov.PaymentGatewayApi.csproj" "PaymentGateway/Api/Biplov.PaymentGatewayApi/Biplov.PaymentGatewayApi.csproj"
COPY "PaymentGateway/Application/Biplov.PaymentGateway.Application/Biplov.PaymentGateway.Application.csproj" "PaymentGateway/Application/Biplov.PaymentGateway.Application/Biplov.PaymentGateway.Application.csproj"
COPY "PaymentGateway/Domain/Biplov.PaymentGateway.Domain/Biplov.PaymentGateway.Domain.csproj" "PaymentGateway/Domain/Biplov.PaymentGateway.Domain/Biplov.PaymentGateway.Domain.csproj"
COPY "PaymentGateway/Infrastructure/Biplov.PaymentGateway.Infrastructure/Biplov.PaymentGateway.Infrastructure.csproj" "PaymentGateway/Infrastructure/Biplov.PaymentGateway.Infrastructure/Biplov.PaymentGateway.Infrastructure.csproj"


COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "PaymentGateway.sln"

COPY . .

WORKDIR "/src/MerchantIdentity/Api/Biplov.MerchantIdentityApi"
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Biplov.MerchantIdentityApi.dll"]