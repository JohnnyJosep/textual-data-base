# TFG - Joan Josep Escandell Riera

```
pip freeze > requirements.txt
```

## DOCKER 

### Freeling

```
docker build -t freeling ./docker/freeling
docker run -it --rm --name freeling -p 50005:50005 freeling analyze -f es.cfg --server -p 50005
```

### Postgre SQL

```
docker build -t postgresql ./docker/postgresql
docker run -it --rm --name postgresql -p 5432:5432 -v D:/Sources/TFG/data/postgresql:/var/lib/postgresql/data postgresql
```
