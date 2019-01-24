FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["desafio-.NET/desafio-.NET.csproj", "desafio-.NET/"]
RUN dotnet restore "desafio-.NET/desafio-.NET.csproj"
COPY . .
WORKDIR "/src/desafio-.NET"
RUN dotnet build "desafio-.NET.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "desafio-.NET.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "desafio-.NET.dll"]