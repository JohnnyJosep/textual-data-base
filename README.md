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

### Freeling api
```
docker build -t freeling-api -f freelingapi.dockerfile .\docker
```