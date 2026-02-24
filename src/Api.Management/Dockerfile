FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Api.Management/Api.Management.csproj", "src/Api.Management/"]
COPY ["src/Core.Lib/Core.Lib.csproj", "src/Core.Lib/"]
RUN dotnet restore "src/Api.Management/Api.Management.csproj"

COPY . .
WORKDIR "/src/src/Api.Management"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.Management.dll"]