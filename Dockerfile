FROM mcr.microsoft.com/dotnet/sdk:7.0 as sdk
WORKDIR /carmanage

COPY ./Data/*.csproj ./Data/
COPY ./DapperAccess/*.csproj ./DapperAccess/
COPY ./Web/*.csproj ./Web/
RUN dotnet restore ./Web/Web.csproj

COPY ./Data/. ./Data/
COPY ./DapperAccess/. ./DapperAccess/
COPY ./Web/. ./Web/

WORKDIR /carmanage/Web
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /carmanage
COPY --from=sdk carmanage/Web/publish ./

ENTRYPOINT [ "dotnet", "Web.dll" ]
