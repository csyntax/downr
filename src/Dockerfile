FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY downr.csproj src/
RUN dotnet restore src/downr.csproj
COPY . .
RUN dotnet build downr.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish downr.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "downr.dll"]