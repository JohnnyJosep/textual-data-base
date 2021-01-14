FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY TextualApi/ .
RUN dotnet restore "src/TextualApi.WebApi/TextualApi.WebApi.csproj"
RUN dotnet build "src/TextualApi.WebApi/TextualApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/TextualApi.WebApi/TextualApi.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TextualApi.WebApi.dll"]