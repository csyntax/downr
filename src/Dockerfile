FROM microsoft/dotnet:sdk AS build
WORKDIR /src
COPY downr.csproj .
RUN dotnet restore downr.csproj
COPY . .
RUN dotnet publish downr.csproj -c Release -o /app

FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 7000
ENTRYPOINT ["dotnet", "./downr.dll"]
