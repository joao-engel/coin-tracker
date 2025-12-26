FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Worker.Crypto/Worker.Crypto.csproj", "src/Worker.Crypto/"]
COPY ["src/Core.Lib/Core.Lib.csproj", "src/Core.Lib/"]

RUN dotnet restore "src/Worker.Crypto/Worker.Crypto.csproj"
COPY . .

WORKDIR "/src/src/Worker.Crypto"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Worker.Crypto.dll"]