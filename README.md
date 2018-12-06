# Phema.Rabbit
AspNetCore Rabbit integration

```csharp

public class TestPayload
{
  public string Name { get; set; }
}

public class TestModelConsumer : RabbitConsumer<TestPayload>
{
  protected override string Tag => "TestModelConsumer";

  protected override async Task Consume(TestPayload payload)
  {
    await Console.Out.WriteLineAsync(payload.Name);
  }
}

public class TestPayloadProducer : RabbitProducer<TestPayload>
{
  public void Send(TestPayload testPayload)
  {
    Produce(testPayload);
  }
}

public class TestModelQueue : RabbitQueue<TestPayload>
{
  protected override string Name => "TestModelQueue";
}

public class TestModelExchange : DirectRabbitExchange
{
  public override string Name => "TestModelExchange";
}

// Startup.cs
services.AddRabbit(options => { /*...*/ })
  .AddConsumers(consumers =>
    consumers.AddConsumer<TestPayload, TestModelConsumer, TestModelQueue>())
  .AddProducers<TestModelExchange>(producers =>
    producers.AddProducer<TestPayload, TestPayloadProducer, TestModelQueue>());
```

- Библиотека разделена на три части: 
  - Базовая с определениями и экстеншеном для конфигурирования
  - Консьюмеры и `IHostedService` к ним
  - Продьюсеры и там же находятся `Exchange`
- Такой подход нужен в случаях, когда инстанс только обрабатывает очереди или только кладет в них
  
- Чтобы создать:
  - Продьюсера нужно отнаследоваться от `RabbitProducer<TPayload>`
  - Консьюмера - `RabbitConsumer<TPayload>`
  - Очередь - `RabbitQueue<TPayload>`
  - Об `Exchange` пунктом ниже. И по неоходимости переопределить нужные параметры
  
- Существует 4 предопределенных Exchange - `DirectRabbitExchange`, `FanoutRabbitExchange`, `HeadersRabbitExchange`, `TopicRabbitExchange
- Каждый инстанс создает одно соединение с реббитом как для консьюмеров, так и для продьюсеров
- Количество консьюмеров на инстансе определяется свойством Parallelism. Все консьюмеры будут находиться в одном канале с тегами `Название_Порядковый номер`
- Все продьюсеры - `Scoped-сервисы` и создают новый канал (не соединение!) на время отправки, что позволяет добавлять в очередь сообщения вне зависимости от количества запросов и обеспечивать потокобезопасность
- Для консьюмеров используется `IHostedService`, после завершения заботы AspNetCore хоста, они не смогут больше обрабатывать сообщения, даже если приложение продолжит свою работу
- Чтобы задать название инстанса в реббите определите `options.InstanceName`
- Если хотите работать с реббитом напрямую - в DI добавлен `IConnection` текущего инстанса
- Все консьюмеры используют `AsyncEventingBasicConsumer` для обработки сообщений
- Можно не добавлять `RabbitOptions` через `AddRabbit(options => ...)`, если используются сторонние механизмы конфигурации
- Через `RabbitOptions` можно определить кодировку, по-умолчанию - `utf-8`
- Для сериализации используется `Newtonsoft.Json` без возможности замены

