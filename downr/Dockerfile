FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY downr.csproj downr/
RUN dotnet restore downr/downr.csproj
COPY . .
WORKDIR /src/downr
RUN dotnet build downr.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish downr.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "downr.dll"]
