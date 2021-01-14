#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ThiTracNghiem_BackEndAPI/ThiTracNghiem_BackEndAPI.csproj", "ThiTracNghiem_BackEndAPI/"]
COPY ["ThiTracNghiem.ViewModel/ThiTracNghiem_ViewModel.csproj", "ThiTracNghiem.ViewModel/"]
RUN dotnet restore "ThiTracNghiem_BackEndAPI/ThiTracNghiem_BackEndAPI.csproj"
COPY . .
WORKDIR "/src/ThiTracNghiem_BackEndAPI"
RUN dotnet build "ThiTracNghiem_BackEndAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThiTracNghiem_BackEndAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThiTracNghiem_BackEndAPI.dll"]