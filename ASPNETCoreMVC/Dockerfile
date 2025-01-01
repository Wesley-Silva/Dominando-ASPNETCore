#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ASPNETCoreMVC.csproj", "ASPNETCoreMVC/"]
RUN dotnet restore "ASPNETCoreMVC/ASPNETCoreMVC.csproj"
WORKDIR "ASPNETCoreMVC"
COPY . .

RUN dotnet build "ASPNETCoreMVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ASPNETCoreMVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Docker"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASPNETCoreMVC.dll"]



#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["ASPNETCoreMVC.csproj", "ASPNETCoreMVC/"]
#RUN dotnet restore "./ASPNETCoreMVC/ASPNETCoreMVC.csproj"
#WORKDIR "/src/ASPNETCoreMVC"
#COPY . .
#
#RUN dotnet build "ASPNETCoreMVC.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./ASPNETCoreMVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ASPNETCoreMVC.dll"]