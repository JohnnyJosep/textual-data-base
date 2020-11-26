# TFG

## Freeling

Docker build image

```
docker build -t freeling ./docker/freeling
docker run -it --rm --name freeling -p 50005:50005 freeling analyze -f es.cfg --server -p 50005
```

## RabbitMq

```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

## Postgre SQL

https://hub.docker.com/_/postgres
```
docker build -t postgresql ./docker/postgresql
docker run -it --rm --name postgresql -p 5432:5432 -v D:/Sources/TFG/data/postgresql:/var/lib/postgresql/data postgresql
```


## Mongo

https://hub.docker.com/_/mongo