FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source


# copy everything else and build app
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app

COPY /. ./
WORKDIR /source
RUN dotnet publish -c release -o /app  --no-restore --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "YARP-demo.dll"]