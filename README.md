# Хранилище стандартов

## Документация API

### Методы пользователя

#### Вход в аккаунт
POST /api/accounts/login

Запрос:
```
{
  "login": "string",
  "password": "strings"
}
```

Ответ:
```
{
  "id": 1,
  "login": "login",
  "name": "name",
  "orgName": "orgName",
  "orgBranch": "orgBranch",
  "orgActivity": "orgActivity",
  "role": "Admin",
  "token": "ey...token"
}
```

#### Регистрация нового пользователя
POST /api/accounts/register

Запрос:
```
{
  "login": "string",
  "name": "string",
  "password": "strings",
  "orgName": "string",
  "orgBranch": "string",
  "orgActivity": "string"
}
```

Ответ:
```
{
  "id": 1,
  "login": "login",
  "name": "name",
  "orgName": "orgName",
  "orgBranch": "orgBranch",
  "orgActivity": "orgActivity",
  "role": "Admin",
  "token": "ey...token"
}
```

#### Смена пароля (для собственного аккаунта)
POST /api/accounts/change-password

Запрос:
```
{
  "login": "string",
  "new_password": "string",
  "old_password": "string"
}
```

Ответ:
200 OK - Успешно
400 Bad Request - Old password is wrong


#### Информация о текущем пользователе
GET /api/account/self-info

Ответ:
```
{
  "id": 1,
  "login": "string",
  "name": "string",
  "orgName": "string",
  "orgBranch": "string",
  "orgActivity": "string",
  "role": "User"
}
```



#### Обновление собственной информации
POST /api/account/self-edit

Запрос:
```
{
  "name": "string",
  "org_name": "string",
  "org_branch": "string",
  "org_activity": "string"
}
```

### Методы администратора

#### Восстановление пароля пользователя
POST /api/accounts/restore-password

Запрос:
```
{
  "login": "string",
  "new_password": "string"
}
```

Ответ:
200 OK - Успешно

#### Список пользователей
GET /api/accounts/list

Ответ:
```
[
  {
    "id": 1,
    "login": "string",
    "name": "string",
    "orgName": "string",
    "orgBranch": "string",
    "orgActivity": "string",
    "role": "User"
  },
  {
    "id": 2,
    "login": "string2",
    "name": "string",
    "orgName": "string",
    "orgBranch": "string",
    "orgActivity": "string",
    "role": "Admin"
  },
]
```

#### Информация о пользователе по id
GET /api/account/get-user-info

Ответ:
```
{
  "id": 1,
  "login": "string",
  "name": "string",
  "orgName": "string",
  "orgBranch": "string",
  "orgActivity": "string",
  "role": "User"
}
```

#### Обновление информации пользователя по login
POST /api/account/self-edit

Запрос:
```
{
  "login": "string",
  "name": "string",
  "org_name": "string",
  "org_branch": "string",
  "org_activity": "string"
}
```

#### Обновление статуса администратора по id
POST /api/account/make-admin

Запрос:
```
{
  "userId": 0,
  "isAdmin": true
}
```

