FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Dashboard.Web.API/Dashboard.Demo.API.csproj", "src/Dashboard.Web.API/"]
COPY ["src/Dashboard.SharedKernel/Dashboard.SharedKernel.csproj", "src/Dashboard.SharedKernel/"]
COPY ["src/Dashboard.Core/Dashboard.Core.csproj", "src/Dashboard.Core/"]
COPY ["src/Dashboard.Infrastructure/Dashboard.Infrastructure.csproj", "src/Dashboard.Infrastructure/"]
COPY ["src/Dashboard.API.DataServices/Dashboard.API.DataServices.csproj", "src/Dashboard.API.DataServices/"]
RUN dotnet restore "src/Dashboard.Web.API/Dashboard.Demo.API.csproj"
COPY . .
WORKDIR "/src/src/Dashboard.Web.API"
RUN dotnet build "Dashboard.Demo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dashboard.Demo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard.Demo.API.dll"]
