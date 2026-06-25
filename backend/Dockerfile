FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/TalentHub.Domain/TalentHub.Domain.csproj src/TalentHub.Domain/
COPY src/TalentHub.Application/TalentHub.Application.csproj src/TalentHub.Application/
COPY src/TalentHub.Infrastructure/TalentHub.Infrastructure.csproj src/TalentHub.Infrastructure/
COPY src/TalentHub.Api/TalentHub.Api.csproj src/TalentHub.Api/
RUN dotnet restore src/TalentHub.Api/TalentHub.Api.csproj

COPY src/ src/
RUN dotnet publish src/TalentHub.Api/TalentHub.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "TalentHub.Api.dll"]
