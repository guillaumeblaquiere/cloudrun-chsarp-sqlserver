# Use Microsoft's official build .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-sdk/
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

# Install production dependencies.
# Copy csproj and restore as distinct layers.
COPY *.csproj ./
RUN dotnet restore

# Copy local code to the container image.
COPY . ./
WORKDIR /app

# Build a release artifact.
RUN dotnet publish -c Release -o out


# Use Microsoft's official runtime .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out ./
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

###### Solution 2: Cloud SQL proxy use #######
RUN wget https://dl.google.com/cloudsql/cloud_sql_proxy.linux.amd64 -O cloud_sql_proxy
RUN chmod +x cloud_sql_proxy

ENTRYPOINT (/app/cloud_sql_proxy -instances=gbl-imt-homerider-basguillaueb:us-central1:ms=tcp:1433 &) && dotnet helloworld-csharp.dll

###### Solution 3: Private Ip Use #######
#ENTRYPOINT ["dotnet", "helloworld-csharp.dll"]