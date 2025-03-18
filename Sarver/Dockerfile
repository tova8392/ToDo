# שלב בסיסי להרצת האפליקציה
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

# שלב הבנייה
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG configuration=Release
WORKDIR /src

# העתקת קובץ ה-proj
COPY Sarver/TodoApi.csproj Sarver/
RUN dotnet restore "Sarver/TodoApi.csproj"

# העתקת כל הקבצים
COPY . .

WORKDIR "/src/Sarver"
RUN dotnet build "TodoApi.csproj" -c $configuration -o /app/build

# שלב הפרסום
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TodoApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# שלב ההרצה
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
