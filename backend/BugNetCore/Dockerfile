FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7283


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BugNetCore.Api/BugNetCore.Api.csproj", "BugNetCore.Api/"]
COPY ["BugNetCore.Dal/BugNetCore.Dal.csproj", "BugNetCore.Dal/"]
COPY ["BugNetCore.Models/BugNetCore.Models.csproj", "BugNetCore.Models/"]
COPY ["BugNetCore.Services/BugNetCore.Services.csproj", "BugNetCore.Services/"]

RUN dotnet restore "BugNetCore.Api/BugNetCore.Api.csproj"
COPY . .
WORKDIR "/src/BugNetCore.Api"
RUN dotnet build "BugNetCore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BugNetCore.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "BugNetCore.Api.dll"]