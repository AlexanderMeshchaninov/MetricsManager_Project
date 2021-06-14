# MetricsManager_Project
  Проект микросервиса по сбору метрик (MetricsManager) создан в REST стиле и состоит из двух API клиентов и одного приложения:
  1. Агент сбора метрик (Metrics Agent) - устанавливается и запускается на локальных машинах где раз в 5 секунд (с помощью библиотеки Quartz) проиcходит мониторинг локальной машины:
  - ЦП (CPU), загруженность процессора;
  - ОПЕРАТИВНАЯ ПАМЯТЬ (RAM), загруженность оперативной памяти;
  - ФИЗИЧЕСКИЙ ДИСК (HDD), свободное место;
  - ДОТНЕТ (DotNET), показывает количество байт во всех кучах;
  - Сеть (Network), загруженность сети.
  Метрики поступают в локальную базу данных (основанную на SQLite), где хранятся для последующих запросов. Также регистрация метрик отмечается датой и временем получения.   Просмотр метрик возможен, через Open API - Swagger, по локальному адресу посредством браузера.
  2. Менеджер сбора метрик (Metrics Manager) - имеет возможность запуска на другой локальной машине для последующего получения метрик с Агентов сбора метрик. Все запрошенные метрики с Агентов попадают в локальную базу данных Менеджера (основанную так же на SQLite).
  Просмотр метрик также возможен, через Open API - Swagger, по локальному адресу посредством браузера.
  3. Клиент для сбора метрик (MetricsManagerClient) приложение имеющее минимальный функционал для отображения метрик полученных с Менеджера сбора метрик (графики загруженности по-умолчанию: CPU, HDD, RAM).
  На Агента сбора метрик и Менеджера сбора метрик написаны ЮнитТесты с использованием библиотеки Moq. Все три приложения имеют логгирование. Архитектура проекта позволяет расширять функциональность всего проекта.
  При создании клиента, который обращается к контроллерам Агента использовался автосгенерированный класс (с помощью NSwag Studio).
  При создании проекта использовались nuGet пакеты:
- AutoMapper.Extensions.Microsoft.DependencyInjection;
- Dapper;
- FluentMigrator;
- FluentMigrator.Runner;
- FluentMigrator.Renner.SQLite;
- Microsoft.Extensions.DependencyInjection;
- Moq;
- NLog;
- NLog.Web.AspNetCore;
- Quartz;
- Swashbuckle.AspNetCore;
- Newtonsoft.Json;
- System.Data.SQLite.Core;
- System.Diagnostics.PerfomanceCounter.

"MetricsManagerClient"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/MetricsManagerPrj-1.png "MetricsManagerClient")
"Agent Manager"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/MetricsManagerPrj-2.png "Agent Manager")
"Metrics Manager"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/MetricsManagerPrj-3.png "Metrics Manager")
"Пример получения метрик CPU из браузера"
![alt tag](https://github.com/AlexanderMeshchaninov/Screenshots/blob/main/MetricsManagerPrj-4.png "Пример получения метрик CPU из браузера")
