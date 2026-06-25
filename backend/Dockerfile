FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TalentHub.sln .
COPY src/TalentHub.Domain/TalentHub.Domain.csproj src/TalentHub.Domain/
COPY src/TalentHub.Application/TalentHub.Application.csproj src/TalentHub.Application/
COPY src/TalentHub.Infrastructure/TalentHub.Infrastructure.csproj src/TalentHub.Infrastructure/
COPY src/TalentHub.Api/TalentHub.Api.csproj src/TalentHub.Api/
RUN dotnet restore

COPY src/TalentHub.Domain/ src/TalentHub.Domain/
COPY src/TalentHub.Application/ src/TalentHub.Application/
COPY src/TalentHub.Infrastructure/ src/TalentHub.Infrastructure/
COPY src/TalentHub.Api/ src/TalentHub.Api/
RUN dotnet publish src/TalentHub.Api/TalentHub.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TalentHub.Api.dll"]
