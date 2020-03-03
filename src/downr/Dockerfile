FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY downr.csproj .
RUN dotnet restore

COPY . /src
RUN dotnet publish -r linux-x64 -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /src/out ./

ENTRYPOINT ["dotnet", "downr.dll"]
