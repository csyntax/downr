FROM microsoft/dotnet:sdk AS build
WORKDIR /src
COPY downr.csproj .
RUN dotnet restore downr.csproj
COPY . .
RUN dotnet publish downr.csproj -c Release -r linux-arm -o /app --self-contained

FROM microsoft/dotnet:aspnetcore-runtime-deps
ENV ASPNETCORE_URLS http://+:5000
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["./downr"]