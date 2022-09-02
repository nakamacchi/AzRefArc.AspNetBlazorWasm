# ソリューションのディレクトリでビルドする

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . /src
RUN dotnet build . -c Release -o /app/build

FROM build AS publish
RUN dotnet publish . -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT Development
ENV DOTNET_ENVIRONMENT Development

CMD ["dotnet", "AzRefArc.AspNetBlazorWasm.Server.dll"]

# Docker ビルド
# sudo docker build -t app .
# コンテナ内ポート 80 をローカルポート 8080 へ接続
# sudo docker run -p:8080:80 app
# イメージデバッグ (コンテナ内を参照)
# sudo docker run --rm -i -t -p:8080:80 app /bin/bash
# 環境変数設定つきで実行
# docker run --env "CONNECTIONSTRINGS__PUBSENTITIES=Server=tcp:xxxxx.database.windows.net,1433;Initial Catalog=pubs;Persist Security Info=False;User ID=xxxxx;Password=xxxxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" -p:8080:80 app

