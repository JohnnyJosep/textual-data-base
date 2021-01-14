FROM postgres:alpine

ENV POSTGRES_USER dbUser
ENV POSTGRES_PASSWORD dbPass
ENV POSTGRES_DB TextDb

COPY init.sql /docker-entrypoint-initdb.d/