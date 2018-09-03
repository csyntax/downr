FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY downr.csproj .
RUN dotnet restore downr.csproj
COPY . .
RUN dotnet publish downr.csproj -c Release -r linux-x64 -o /app --self-contained

FROM microsoft/dotnet:2.1-runtime-deps
ENV ASPNETCORE_URLS http://+:80
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENTRYPOINT ["./downr"]