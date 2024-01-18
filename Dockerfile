FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build

ARG TARGETARCH
ARG TARGETOS

RUN arch=$TARGETARCH \
    && if [ "$arch" = "amd64" ]; then arch="x64"; fi && echo $TARGETOS-$arch > /tmp/rid
    
WORKDIR /app
EXPOSE 80
EXPOSE 3000

COPY *.csproj ./
RUN dotnet restore 

COPY . ./
RUN dotnet publish -c Release -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o out

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS final

ARG TARGETARCH
ARG TARGETOS

RUN arch=$TARGETARCH \
    && if [ "$arch" = "amd64" ]; then arch="x64"; fi && echo $TARGETOS-$arch > /tmp/rid
    
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "startup-trial.dll"]
