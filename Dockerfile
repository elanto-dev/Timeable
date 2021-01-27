FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Directory.Build.props .

COPY Contracts.BLL.App/*.csproj ./Contracts.BLL.App/
COPY Contracts.BLL.Base/*.csproj ./Contracts.BLL.Base/	
COPY Contracts.DAL.App/*.csproj ./Contracts.DAL.App/
COPY Contracts.DAL.Base/*.csproj ./Contracts.DAL.Base/
COPY Domain/*.csproj ./Domain/
COPY Resources/*.csproj ./Resources/
COPY BLL.App/*.csproj ./BLL.App/
COPY DAL.App/*.csproj ./DAL.App/
COPY BLL.Base/*.csproj ./BLL.Base/
COPY DAL.App.EF/*.csproj ./DAL.App.EF/
COPY BLL.DTO/*.csproj ./BLL.DTO/
COPY DAL.Base/*.csproj ./DAL.Base/
COPY DAL.DTO/*.csproj ./DAL.DTO/
COPY HTMLParser/*.csproj ./HTMLParser/
COPY TimeableAppWeb/*.csproj ./TimeableAppWeb/

RUN dotnet restore


# copy everything else and build app
COPY Contracts.BLL.App/. ./Contracts.BLL.App/
COPY Contracts.BLL.Base/. ./Contracts.BLL.Base/	
COPY Contracts.DAL.App/. ./Contracts.DAL.App/
COPY Contracts.DAL.Base/. ./Contracts.DAL.Base/
COPY Domain/. ./Domain/
COPY Resources/. ./Resources/
COPY BLL.App/. ./BLL.App/
COPY DAL.App/. ./DAL.App/
COPY BLL.Base/. ./BLL.Base/
COPY DAL.App.EF/. ./DAL.App.EF/
COPY BLL.DTO/. ./BLL.DTO/
COPY DAL.Base/. ./DAL.Base/
COPY DAL.DTO/. ./DAL.DTO/
COPY HTMLParser/. ./HTMLParser/
COPY TimeableAppWeb/. ./TimeableAppWeb/

WORKDIR /source/TimeableAppWeb
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:3.1

# set timezone
# https://serverfault.com/questions/683605/docker-container-time-timezone-will-not-reflect-changes
ENV TZ 'Europe/Tallinn'
RUN echo $TZ > /etc/timezone && \
    apt-get update && apt-get install -y tzdata && \
    rm /etc/localtime && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get clean

WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "TimeableAppWeb.dll"]
