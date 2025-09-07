# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Mottu.Api/Mottu.Api.csproj"
RUN dotnet build "Mottu.Api/Mottu.Api.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "Mottu.Api/Mottu.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# runtime final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Mottu.Api.dll"]
