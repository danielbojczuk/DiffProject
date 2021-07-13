FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app/src
COPY ./ ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80/tcp
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DiffProject.WebAPI.dll"]
