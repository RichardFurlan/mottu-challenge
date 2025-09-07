# Mottu Backend Challenge
## Como rodar (local)
1. `docker-compose up -d db rabbit minio`
2. Aguarde o Postgres iniciar (`docker logs -f <db-container>` ou `docker-compose logs -f db`)
3. Criar migrations:
    - `dotnet ef migrations add Initial --project Mottu.Infrastructure --startup-project Mottu.Api`
    - `dotnet ef database update --project Mottu.Infrastructure --startup-project Mottu.Api`
4. Rodar API:
    - `dotnet run --project Mottu.Api`
5. Verificar: `http://localhost:5000/health`

## Rodar com Docker (tudo)
`docker-compose up --build`

