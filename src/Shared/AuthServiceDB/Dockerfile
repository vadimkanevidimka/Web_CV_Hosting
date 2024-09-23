FROM postgres:latest
COPY init.sql /docker-entrypoint-initdb.d/

ENV POSTGRES_DB=identity
ENV POSTGRES_USER=admin
ENV POSTGRES_PASSWORD=admin_password

EXPOSE 5432

