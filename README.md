# events-webapi

## Приложение

Чтобы запустить приложение, выполните следующие команды:

```sh
docker-compose up --build
```

Приложение будет доступно по следующим адресам:
Events: http://localhost:8080
PostgresDB:5432

## Тесты
Чтобы запустить тесты, выполните:
```sh
cd /event-webapi/Tests
dotnet test Tests.csproj
```
