# Versta Test

## Как запустить проект

1. Скачайте проект с GitHub:

```powershell
git clone <ссылка-на-репозиторий>
cd Versta_test
```

2. Создайте файлы окружения из примеров:

```powershell
copy backend\.env.example backend\.env
copy frontend\.env.example frontend\.env
```

3. Откройте созданные файлы `.env` и замените значения на свои, если нужно:

- `backend/.env` - настройки PostgreSQL;
- `frontend/.env` - адрес backend API.

4. Запустите Docker Desktop.

5. Соберите и запустите проект:

```powershell
docker compose up --build
```

После запуска:

- Frontend: `http://localhost:5173`
- Backend API: `http://localhost:5047`
- Swagger: `http://localhost:5047/swagger`

## Как остановить

```powershell
docker compose down
```
