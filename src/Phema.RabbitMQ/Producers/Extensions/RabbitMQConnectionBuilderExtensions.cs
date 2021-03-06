using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		private static IRabbitMQProducerBuilder<TPayload> AddProducer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQExchangeBuilderCore exchange)
		{
			var declaration = new RabbitMQProducerDeclaration(
				typeof(TPayload),
				connection.ConnectionDeclaration,
				exchange.ExchangeDeclaration);

			connection.Services
				.Configure<RabbitMQOptions>(
					options => options.ProducerDeclarations.Add(typeof(TPayload), declaration));

			return new RabbitMQProducerBuilder<TPayload>(declaration);
		}

		public static IRabbitMQProducerBuilder<TPayload> AddProducer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQExchangeBuilder exchange)
		{
			return connection.AddProducer<TPayload>((IRabbitMQExchangeBuilderCore)exchange);
		}
		
		public static IRabbitMQProducerBuilder<TPayload> AddProducer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQExchangeBuilder<TPayload> exchange)
		{
			return connection.AddProducer<TPayload>((IRabbitMQExchangeBuilderCore)exchange);
		}
	}
}