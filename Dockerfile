# ---------- Etapa 1: compilar y publicar ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SaltinasMvc.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish SaltinasMvc.csproj -c Release -o /app/publish --no-restore

# ---------- Etapa 2: imagen liviana para ejecutar ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Program.cs lee la variable PORT que entrega el hosting (si no existe, usa 8080)
ENTRYPOINT ["dotnet", "SaltinasMvc.dll"]
