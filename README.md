# Тестовое задание

## Задача

Необходимо реализовать веб-сервис, основной задачей которого будет:

1. Принимать GET запрос
2. Формировать на его основе запрос к api Dadata
3. Возвращать модель

## Условия для работы

Необходимо иметь установленную среду .NET 7 на компьютере

## Запуск сервера

Сначала необходимо склонировать репозиторий:

    $ git clone https://github.com/RTDElap/QualityPointTask

Перейти в папку Source/QualityPointTask.WebApi:

    $ cd QualityPointTask/Source/QualityPointTask.WebApi

Скачать все зависимости:

    $ dotnet restore

Установить секреты через переменные окружения:

Windows:

    PS $env:DadataConfig:Token="ТОКЕН"

    PS $env:DadataConfig:Secret="СЕКРЕТ"

Запустить в промышленном окружении

    $ dotnet run -c release --launch-profile https

или в тестовом (с документацией swagger, доступной по /swagger)

    $ dotnet run -c release --launch-profile http

## Пример работы

:heavy_exclamation_mark: По-умолчанию сервер запускается на портах 8080 и 443

Запрос города

    GET /воронеж

    {
        "result": "г Воронеж",
        "country": "Россия",
        "region": "Воронежская",
        "area": null,
        "city": "Воронеж",
        "cityDistrict": null,
        "settlement": null,
        "street": null,
        "house": null,
        "block": null,
        "mailingQuality": 2
    }

Запроса дома

    GET /воронеж/генерала-лизюкова/15

    {
        "result": "г Воронеж, ул Генерала Лизюкова, д 15",
        "country": "Россия",
        "region": "Воронежская",
        "area": null,
        "city": "Воронеж",
        "cityDistrict": null,
        "settlement": null,
        "street": "Генерала Лизюкова",
        "house": "15",
        "block": null,
        "mailingQuality": 1
    }

Запрос с ошибкой

    GET /врн/vvv

    {
    "message": "Недостаточно данных для поиска."
    }
