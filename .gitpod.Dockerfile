FROM gitpod/workspace-full:2024-12-19-16-24-32

# Install .NET 8 SDK
RUN apt-get update && apt-get install -y \
    dotnet-sdk-8.0 \
    dotnet-runtime-8.0 \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Install additional development tools
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    jq \
    git-lfs \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Set up .NET telemetry
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# Verify installation
RUN dotnet --version

# Pre-warm dotnet by creating a hello world project
RUN mkdir -p /tmp/warmup && cd /tmp/warmup && \
    dotnet new console -f net8.0 -n warmup && \
    cd warmup && \
    dotnet build && \
    cd / && rm -rf /tmp/warmup
