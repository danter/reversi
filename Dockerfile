FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /app

# NOTE: this is not very efficient, you should only copy the files needed for the build step and not all files or the cache invalidation will trigger too often.
COPY . .
RUN cd reversi_dotnet && dotnet build /p:targetframework=netcoreapp2.0 && dotnet publish -f netcoreapp2.0 -c Release -o /app/out

FROM microsoft/dotnet:2.1-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "reversi.dll"]
