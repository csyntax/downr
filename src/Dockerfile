FROM mcr.microsoft.com/dotnet/core/sdk:3.0-apline AS build

WORKDIR /src

COPY downr.csproj .
RUN dotnet restore downr.csproj
COPY . .

RUN dotnet publish -r linux-arm -c Release --self-contained

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-alpine AS runtime
WORKDIR /app
COPY --from=build /src/out .
EXPOSE 5000
ENTRYPOINT ["./downr"]
