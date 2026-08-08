# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first, as its own layer, so code edits do not invalidate the
# (slow) package restore.
COPY Boggle/Boggle.csproj Boggle/
RUN dotnet restore Boggle/Boggle.csproj

COPY Boggle/ Boggle/
RUN dotnet publish Boggle/Boggle.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Most free hosts inject the listening port as $PORT; fall back to 8080 for
# plain "docker run". Shell form is required so the variable is expanded.
ENTRYPOINT ["/bin/sh", "-c", "exec dotnet Boggle.dll --urls http://+:${PORT:-8080}"]
