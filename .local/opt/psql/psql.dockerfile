FROM docker.io/postgres:16.2-alpine

RUN apk add --no-cache pspg

RUN adduser -D -g appuser appuser
RUN chown -R appuser /home/appuser

COPY --chown=appuser:appuser pgpass /home/appuser/.pgpass
COPY --chown=appuser:appuser psqlrc /home/appuser/.psqlrc

USER appuser

WORKDIR /home/appuser

ENTRYPOINT psql