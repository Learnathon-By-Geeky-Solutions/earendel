# Stage 1: Build & Restore Dependencies
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy the solution file and all project files
COPY Aspire/Aspire.sln ./
COPY Api/Server/ Api/Server/
COPY Aspire/ Aspire/
COPY Api/Framework/Core/ Api/Framework/Core/
COPY Api/Framework/Infrastructure/ Api/Framework/Infrastructure/
COPY Api/Migrations/PGSql/ Api/Migrations/PGSql/
COPY Api/Module/Candidate/ Api/Module/Candidate/
COPY Api/Module/HRView/ Api/Module/HRView/
COPY Api/Module/InterviewerView/ Api/Module/InterviewerView/
COPY Api/Module/CandidateLogic/ Api/Module/CandidateLogic/
COPY Api/Module/Experties/ Api/Module/Experties/
COPY Api/Module/Interviews/ Api/Module/Interviews/
COPY Api/Module/Jobs/ Api/Module/Jobs/
COPY Api/Module/Notifications/ Api/Module/Notifications/
COPY Api/Module/Quizzes/ Api/Module/Quizzes/
COPY Api/Module/Evaluator/ Api/Module/Evaluator/
COPY Shared/ Shared/
COPY Test/ Test/

# ✅ Ensure `assets` directory is copied safely
RUN mkdir -p /app/assets
COPY Api/Server/assets/ /app/assets/
RUN chmod -R 755 /app/assets  # Set correct permissions

# Restore dependencies (cached for performance)
RUN dotnet restore Aspire/Aspire.sln

# ================================
# Stage 2: Build & Publish Application
# ================================
FROM build AS publish

# Build the application in Release mode
RUN dotnet build Aspire/Aspire.sln -c Release --no-restore

# Publish the application
RUN dotnet publish Aspire/Aspire.sln -c Release -o /app/publish --no-build

# ================================
# Stage 3: Runtime (Final Image)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set working directory inside the container
WORKDIR /app

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser:appuser /app  # ✅ Fix: Change `/App` to `/app`

USER appuser

# Copy the published files from the build stage
COPY --from=publish /app/publish .

# Ensure `assets` directory exists in final container
RUN mkdir -p /app/assets && chmod -R 755 /app/assets

# ✅ Expose ports needed for Aspire integration
EXPOSE 8000
EXPOSE 8001
EXPOSE 18888

# ✅ Define environment variable to enable Aspire telemetry
ENV DOTNET_DASHBOARD_URL="http://aspire:18888"

# Set the entrypoint command
ENTRYPOINT ["dotnet", "Server.dll"]