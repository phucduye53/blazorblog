FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY blazorblog.csproj .
RUN dotnet restore "blazorblog.csproj"
COPY . .
RUN dotnet build "blazorblog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blazorblog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet blazorblog.dll