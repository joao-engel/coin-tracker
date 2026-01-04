FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Api.Analytics/Api.Analytics.csproj", "src/Api.Analytics/"]
COPY ["src/Core.Lib/Core.Lib.csproj", "src/Core.Lib/"]
RUN dotnet restore "src/Api.Analytics/Api.Analytics.csproj"

COPY . .
WORKDIR "/src/src/Api.Analytics"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.Analytics.dll"]