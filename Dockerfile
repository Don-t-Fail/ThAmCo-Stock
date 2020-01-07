FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim
COPY stock/ /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "ThAmCo.Stock.dll"]