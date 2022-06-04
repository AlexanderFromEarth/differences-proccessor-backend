FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["XlsFilterService/XlsFilterService.csproj", "XlsFilterService/"]
RUN dotnet restore "XlsFilterService/XlsFilterService.csproj"
COPY . .
WORKDIR "/src/XlsFilterService"
RUN dotnet build "XlsFilterService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XlsFilterService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XlsFilterService.dll"]
