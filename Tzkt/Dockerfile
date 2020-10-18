FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY . .
RUN cd Tzkt && dotnet publish -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
COPY --from=build /app/Tzkt/output ./
ENTRYPOINT ["dotnet", "Tzkt.dll"]
