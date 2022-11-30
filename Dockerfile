FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Task.Mvc/Task.Mvc.csproj", "Task.Mvc/"]
COPY ["Task.Application/Task.Application.csproj", "Task.Application/"]
COPY ["Task.Domain/Task.Domain.csproj", "Task.Domain/"]
COPY ["Task.Persistence/Task.Persistence.csproj", "Task.Persistence/"]
RUN dotnet restore "Task.Mvc/Task.Mvc.csproj"
RUN dotnet restore "Task.Application/Task.Application.csproj"
RUN dotnet restore "Task.Domain/Task.Domain.csproj"
RUN dotnet restore "Task.Persistence/Task.Persistence.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Task.Mvc/Task.Mvc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Task.Mvc/Task.Mvc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD dotnet Task.Mvc.dll