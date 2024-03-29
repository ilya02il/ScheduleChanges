#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 666

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["src/GrpcService/GrpcAPI.csproj", "./GrpcService/"]
COPY ["src/Mapping/Mapping.csproj", "./Mapping/"]
COPY ["src/Domain/Domain.csproj", "./Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "./Infrastructure/"]
COPY ["src/Application/Application.csproj", "./Application/"]
RUN dotnet restore "./GrpcService/GrpcAPI.csproj"
COPY . .
WORKDIR ./src/GrpcService
RUN dotnet build "./GrpcAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./GrpcAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcAPI.dll"]