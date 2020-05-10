FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["WebAppCore22/WebAppCore22.csproj", "WebAppCore22/"]
RUN dotnet restore "WebAppCore22/WebAppCore22.csproj"
COPY . .
WORKDIR "/src/WebAppCore22"
RUN dotnet build "WebAppCore22.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAppCore22.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAppCore22.dll"]