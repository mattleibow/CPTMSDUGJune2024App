# Commands

## Docker Demo

```sh
# build docker image
docker build -t website-manual .

# run docker container
docker run -it --rm -p 8080:8080 website-manual

# start dev tunnel
devtunnel host --allow-anonymous --port-numbers 8080
```

## Aspire Demo

```sh
# start aspire app
dotnet run --project Exciting.AppHost --launch-profile https

# start dev tunnel
devtunnel delete exciting-tunnel --force
devtunnel create exciting-tunnel --allow-anonymous
# add the REST API port
devtunnel port create exciting-tunnel --port-number 7258 --protocol https
# add the Open Telemetry port (analytics and logs to the dashboard)
devtunnel port create exciting-tunnel --port-number 21031 --protocol https
# add the Aspire Dashboard
devtunnel port create exciting-tunnel --port-number 17048 --protocol https
devtunnel host exciting-tunnel
```

## Profiling Demo

```sh

```