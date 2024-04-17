FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TestAuthApplication/TestAuthApplication.csproj", "TestAuthApplication/"]
RUN dotnet restore "TestAuthApplication/TestAuthApplication.csproj"

COPY . .
WORKDIR "/src/TestAuthApplication/"
RUN dotnet build "TestAuthApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAuthApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM  base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY TestAuthApplication/nginx.conf /etc/nginx/nginx.conf

