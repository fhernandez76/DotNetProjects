FROM mcr.microsoft.com/dotnet/core/runtime:3.0-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS build
WORKDIR /src
COPY ["TestWorkerServiceRabbit/TestWorkerServiceRabbit.csproj", "TestWorkerServiceRabbit/"]
RUN dotnet restore "TestWorkerServiceRabbit/TestWorkerServiceRabbit.csproj"
COPY . .
WORKDIR "/src/TestWorkerServiceRabbit"
RUN dotnet build "TestWorkerServiceRabbit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestWorkerServiceRabbit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestWorkerServiceRabbit.dll"]