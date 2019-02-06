FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY downr.csproj .
RUN dotnet restore downr.csproj
COPY . .
RUN dotnet publish downr.csproj -c Release -o /app

FROM microsoft/dotnet:2.2-runtime-deps
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["./downr"]
