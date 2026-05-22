FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["VPT_Movie_Box.csproj", "./"]

RUN dotnet restore "./VPT_Movie_Box.csproj"

COPY . .

RUN dotnet publish "VPT_Movie_Box.csproj" -c Release -o /app/publish



FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VPT_Movie_Box.dll"]