FROM liquibase/liquibase:latest

# Установка драйвера PostgreSQL
USER root
RUN mkdir -p /liquibase/lib && \
    wget -q https://jdbc.postgresql.org/download/postgresql-42.7.1.jar -O /liquibase/lib/postgresql.jar && \
    chmod 644 /liquibase/lib/postgresql.jar
USER liquibase

COPY change-sets /liquibase/changelog/change-sets
COPY master.xml /liquibase/changelog/master.xml

ENTRYPOINT ["/liquibase/liquibase"]
CMD ["--changeLogFile=master.xml", "--searchPath=changelog", "--url=jdbc:postgresql://db:5432/access_control_db", "--username=test_db", "--password=test_db", "--driver=org.postgresql.Driver", "update"]

