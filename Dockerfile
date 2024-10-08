﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN groupadd -g 1000 appgroup && \
    useradd -r -u 1000 -g appgroup appuser
WORKDIR /app
RUN mkdir data
RUN chown -R appuser:appgroup /app/data
USER appuser
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NafanyaVPN/NafanyaVPN.csproj", "NafanyaVPN.csproj"]
RUN dotnet restore "NafanyaVPN.csproj"
COPY . .
WORKDIR "/src/NafanyaVPN"
RUN dotnet build "NafanyaVPN.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NafanyaVPN.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NafanyaVPN.dll"]
