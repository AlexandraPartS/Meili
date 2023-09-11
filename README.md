# Meili

#### Как запустить проект:
1. Установить пакеты NuGet из файла `Meili/backend/backend.csproj`;
2. Для корректного отображения фронта установить папку <b>`node_modules`</b>:
    1) открыть терминал проекта, 
    2) перейти в директорию `..\Meili\frontend>`,
    3) набрать npm `npm install bootstrap@5.3.0-alpha3`.



#### Адреса страниц:

|Наименование страницы|Адрес запроса|Откроется файл|Расположение файла|
|:------|:------|----|-----|
|Главная|`https://localhost:xxxx/`|`index.html`|`frontend/index.html`|
|Главная|`https://localhost:xxxx/index.html`|`index.html`|`frontend/index.html`|
|Страница настроек пользователя|`https://localhost:xxxx/user/userdata.html`|`userc.html`|`frontend/user/userdata.html`|
|Страница списка пользователей|`https://localhost:xxxx/user/userslist.html`|`userslist.html`|`frontend/user/userslist.html`|