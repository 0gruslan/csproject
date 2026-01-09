# Система контроля и управления доступом

Простой CRUD сервис для управления доступом, написанный на ASP.NET Core.

## Технологии

- ASP.NET Core 8.0
- PostgreSQL
- Redis (для кэширования)
- Docker + docker-compose
- Liquibase (миграции БД)
- Entity Framework Core
- Dapper (для many-to-many связей)
- FluentValidation
- Swagger
- JWT авторизация

## Запуск проекта

1. Убедитесь, что установлен Docker и docker-compose
2. Запустите проект:
```bash
docker-compose up -d --build
```

3. API будет доступен по адресу: http://localhost:5000
4. Swagger UI: http://localhost:5000/swagger

## Первый вход

**Логин:** admin  
**Пароль:** admin123

## Основные функции

- Управление пользователями (CRUD)
- Управление точками доступа (CRUD)
- Управление картами доступа (CRUD)
- Просмотр журнала доступа
- Пагинация и поиск
- Авторизация через JWT
- Роли: Admin, Manager, User

## Роли и права

- **Admin** - полный доступ ко всем операциям
- **Manager** - может создавать и обновлять, но не удалять
- **User** - только чтение

## Тестирование

Запуск unit-тестов:
```bash
dotnet test WebApplication1.Tests/WebApplication1.Tests.csproj
```

## Структура проекта

```
WebApplication1/
├── Controllers/     # API контроллеры
├── Services/        # Бизнес-логика
├── Repositories/    # Работа с БД
├── Models/          # Модели данных
├── Middleware/      # Middleware для обработки ошибок и логирования
└── Validators/      # Валидация входных данных
```

## База данных

База данных создается автоматически при первом запуске через Liquibase миграции.

Схема БД:
- users - пользователи
- roles - роли
- permissions - разрешения
- access_points - точки доступа
- access_cards - карты доступа
- access_logs - журнал доступа
- user_roles - связь пользователей и ролей (many-to-many)
- access_card_access_points - связь карт и точек доступа (many-to-many)
