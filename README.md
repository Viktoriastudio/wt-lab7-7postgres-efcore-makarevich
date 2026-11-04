# WT-6 Minimal API — Управление IT-проектами

REST API на ASP.NET Core Minimal API.

## API Endpoints
| Метод | URL | Назначение |
|---|---|---|
| GET | `/` | Проверить работу приложения |
| GET | `/api/info` | Информация о приложении |
| GET | `/api/tasks` | Получить список задач |
| GET | `/api/tasks/{id}` | Получить задачу по ID |
| GET | `/api/tasks?search=...` | Поиск задач по названию |
| POST | `/api/tasks` | Создать задачу |
| PUT | `/api/tasks/{id}` | Изменить задачу |
| DELETE | `/api/tasks/{id}` | Удалить задачу |

## Пример POST-запроса
```json
{
  "title": "Код-ревью",
  "project": "CRM для банка",
  "assignee": "Виктория",
  "estimatedHours": 5,
  "isCompleted": false
}