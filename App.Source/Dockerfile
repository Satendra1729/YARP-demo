FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /s


# copy everything else and build app

COPY /. ./
WORKDIR /s
RUN dotnet publish -c release -o /app  --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
COPY ./app.source.entrypoint.sh ./
ENTRYPOINT ["bash","app.source.entrypoint.sh"]