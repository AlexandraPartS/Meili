# Meili

#### Как запустить проект (сейчас):
1. Установить пакеты NuGet из файла `Meili/backend/backend.csproj`:
    1) `Microsoft.EntityFrameworkCore.InMemory` версии 7.0.7
2. Для корректного отображения фронта установить папку <b>`node_modules`</b>:
    1) открыть терминал проекта, 
    2) перейти в директорию `..\Meili\frontend>`,
    3) набрать npm `npm install bootstrap@5.3.0-alpha3`.



#### Адреса страниц:

|Адрес запроса|Откроется файл|Расположение файла|
|:------|----|-----|
|`https://localhost:xxxx/`|`index.html`|`frontend/index.html`|
|`https://localhost:xxxx/index.html`|`index.html`|`frontend/index.html`|
|`https://localhost:xxxx/user/userc.html`|`userc.html`|`frontend/user/userc.html`|

Используемые настройки для переадресации загружаемых файлов:
  
В файле `Program.cs`:
* изменение пути к статическим файлам (`WebRootPath=`)
* поддержка страниц html по умолчанию (`UseDefaultFiles();`)


---
---

#### Для подробного тестирования API можно воспользоваться `Swagger`:
1. Установить пакет NuGet `Swashbuckle.AspNetCore` версии 6.5.0 в проект    `backend`.
2. Заменить содержимое файла `Program.cs` следующим кодом:

```
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(
            opt => opt.UseInMemoryDatabase("UsersList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
```