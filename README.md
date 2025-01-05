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
POST /api/accounts/self-edit

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
GET /api/accounts/get-user-info

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
POST /api/accounts/self-edit

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
POST /api/accounts/make-admin

Запрос:
```
{
  "userId": 0,
  "isAdmin": true
}
```

### Поисковые запросы документов

#### Документ по id
GET /api/docs/{docId:long}

Ответ:
```
{
  "docId": 0,
  "primary": {
    "id": 0,
    "designation": "string",
    "fullName": "string",
    "codeOKS": "string",
    "activityField": "string",
    "acceptanceYear": 9999,
    "commissionYear": 9999,
    "author": "string",
    "acceptedFirstTimeOrReplaced": "string",
    "content": "string",
    "keyWords": "string",
    "applicationArea": "string",
    "adoptionLevel": 0,
    "documentText": "string",
    "changes": "string",
    "amendments": "string",
    "status": 0,
    "harmonization": 0,
    "isPrimary": true,
    "docId": 0,
    "lastEditTime": "2024-12-11T19:21:56.829Z"
  },
  "actual": {
    "id": 0,
    "designation": "string",
    "fullName": "string",
    "codeOKS": "string",
    "activityField": "string",
    "acceptanceYear": 9999,
    "commissionYear": 9999,
    "author": "string",
    "acceptedFirstTimeOrReplaced": "string",
    "content": "string",
    "keyWords": "string",
    "applicationArea": "string",
    "adoptionLevel": 0,
    "documentText": "string",
    "changes": "string",
    "amendments": "string",
    "status": 0,
    "harmonization": 0,
    "isPrimary": true,
    "docId": 0,
    "lastEditTime": "2024-12-11T19:21:56.829Z"
  },
  "references": [
    {
      "docId": 0,
      "designation": "string",
      "status": 0
    }
  ]
}
```

#### Поиск документов
GET /api/docs/search?{queryParams}

Список параметров:
- text: полнотекстовый поиск, ищет по всем полям в документе (если null, то выводятся все с сортировкой по коду ОКС)
- codeOks: фильтр с содержанием в поле
- acceptanceYear: фильтр с содержанием в поле
- commisionYear: фильтр с содержанием в поле
- author: фильтр с содержанием в поле
- acceptedFirstTimeOrReplaced: фильтр с содержанием в поле
- keyWords: фильтр с содержанием в поле
- adoptionLevel: уровень принятия International, Foreign, Regional, Organizational, National, Interstate
- status: статус документа Valid, Canceled, Replaced
- harmonization: гармонизация Unharmonized, Modified, Harmonized
- limit: количество
- offset: смещение

Ответ:
```
[
  {
    "id": 429,
    "codeOks": "01.040.35, 35.240",
    "designation": "ГОСТ 34.201-2020",
    "fullName": "ИТ Комплекс стандартов на автоматизированные системы. Виды, комплектность и обозначение документов при создании автоматизированных систем",
    "relevanceMark": 5
  },
  {
    "id": 4,
    "codeOks": "01.120",
    "designation": "ГОСТ 1.0-2015",
    "fullName": "Межгосударственная система стандартизации. Основные положения",
    "relevanceMark": 5
  }
]
```

#### Количество активных документов
GET /api/docs/count?{queryParams}

queryParams в "Поиск документов"

Ответ:
```
123
```


### Запросы по статистике

#### Количество просмотров
GET /api/stats/views?{queryParams}

Список параметров:
- designation: обозначение, например ГОСТ-1.0, точное совпадение
- codeOks: фильтр с содержанием в поле
- activityField: фильтр с содержанием в поле
- orgBranch: фильтр по количеству просмотра от конкретной организации
- startYear: начало диапазона поиска просмотров
- endYear: конец диапазона поиск просмотров

Ответ:
```
[
  {
    "docId": 1,
    "designation": "string",
    "fullName": "string",
    "views": 123,
  }
]
```


#### Действия с документами (обновление, создание)
GET /api/stats/actions?{queryParams}

Список параметров:
 - status: статус документов
 - count: количество действий
 - startDate: начало диапазона поиска
 - endDate: конец диапазона поиска


Ответ:
```
[
  {
    "docId": 1,
    "designation": "string",
    "fullName": "string",
    "action": "Create",
    "date": "2024-03-25T06:21:50.814318Z"
  }
]
```

### Действия с документами (только для администраторов)

#### Создание нового документа
POST /api/docs/add

Запрос:
```
{
  "designation": "string",
  "fullName": "string",
  "codeOKS": "string",
  "activityField": "string",
  "acceptanceYear": 9999,
  "commissionYear": 9999,
  "author": "string",
  "acceptedFirstTimeOrReplaced": "string",
  "content": "string",
  "keyWords": "string",
  "applicationArea": "string",
  "adoptionLevel": 0,
  "documentText": "string",
  "changes": "string",
  "amendments": "string",
  "status": 0,
  "harmonization": 0,
  "isPrimary": true,
  "references": [
    "string"
  ]
}
```

Ответ: id документа

#### Удаление документа
DELETE /api/docs/delete/{docId}

#### Обновление полей документа (первичные)
PUT /api/docs/update/{docId}

Запрос:
```
{
  "designation": "string",
  "fullName": "string",
  "codeOKS": "string",
  "activityField": "string",
  "acceptanceYear": 9999,
  "commissionYear": 9999,
  "author": "string",
  "acceptedFirstTimeOrReplaced": "string",
  "content": "string",
  "keyWords": "string",
  "applicationArea": "string",
  "adoptionLevel": 0,
  "documentText": "string",
  "changes": "string",
  "amendments": "string",
  "status": 0,
  "harmonization": 0,
  "isPrimary": true,
  "references": [
    "string"
  ]
}
```

#### Актуализация полей документа (актуализированные)
PUT /api/docs/actualize/{docId}

Запрос:
```
{
  "designation": "string",
  "fullName": "string",
  "codeOKS": "string",
  "activityField": "string",
  "acceptanceYear": 9999,
  "commissionYear": 9999,
  "author": "string",
  "acceptedFirstTimeOrReplaced": "string",
  "content": "string",
  "keyWords": "string",
  "applicationArea": "string",
  "adoptionLevel": 0,
  "documentText": "string",
  "changes": "string",
  "amendments": "string",
  "status": 0,
  "harmonization": 0,
  "isPrimary": true,
  "references": [
    "string"
  ]
}
```

#### Обновление статуса документа
PUT /api/docs/change-status

Запрос:
```
{
  "id": 0,
  "status": "Canceled"
}
```


#### Загрузка файла
POST /api/docs/{docId}/upload-file

"Content-Type": "multipart/form-data"

Запрос:
```
[
  "file": "content",
  "extension": "docx"
]
```


#### Реиндексация всех документов
POST /api/docs/index-all
