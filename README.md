# TFG

## Freeling

Docker build image

```
docker build -t freeling ./docker/freeling
docker run -it --rm -p 50005:50005 freeling analyze -f es.cfg --server -p 50005
```