##############################
# BUILD
##############################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /working
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore Exciting.Website
# Build and publish a release
RUN dotnet publish Exciting.Website -c Release -o out


##############################
# RUNTIME
##############################
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copy the published output from the sdk stage
COPY --from=build-env /working/out .
ENTRYPOINT ["dotnet", "Exciting.Website.dll"]
