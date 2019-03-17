using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerBuilderExtensions
	{
		/// <summary>
		///   Sets consumer tag used in queue consumers
		/// </summary>
		public static IRabbitMQConsumerBuilder WithTag(
			this IRabbitMQConsumerBuilder builder,
			string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));

			builder.Metadata.Tag = consumerTag;

			return builder;
		}

		/// <summary>
		///   Sets message count prefetch
		/// </summary>
		public static IRabbitMQConsumerBuilder WithPrefetch(
			this IRabbitMQConsumerBuilder builder,
			ushort prefetch)
		{
			builder.Metadata.Prefetch = prefetch;

			return builder;
		}

		/// <summary>
		///   Sets count parallel consumers
		/// </summary>
		public static IRabbitMQConsumerBuilder WithCount(
			this IRabbitMQConsumerBuilder builder,
			int count)
		{
			builder.Metadata.Count = count;

			return builder;
		}

		/// <summary>
		///   Sets exclusive consumer for queue
		/// </summary>
		public static IRabbitMQConsumerBuilder Exclusive(this IRabbitMQConsumerBuilder builder)
		{
			builder.Metadata.Exclusive = true;
			return builder;
		}

		/// <summary>
		///   Sets no-local flag. If true, rabbitmq will not send messages to the connection that published them
		/// </summary>
		public static IRabbitMQConsumerBuilder NoLocal(this IRabbitMQConsumerBuilder builder)
		{
			builder.Metadata.NoLocal = true;

			return builder;
		}

		/// <summary>
		///   Sets auto-ack flag. If true, consumer will ack messages when received
		/// </summary>
		public static IRabbitMQConsumerBuilder AutoAck(this IRabbitMQConsumerBuilder builder)
		{
			builder.Metadata.AutoAck = true;

			return builder;
		}

		/// <summary>
		///   Requeue message when fail to consume.
		/// </summary>
		public static IRabbitMQConsumerBuilder Requeue(
			this IRabbitMQConsumerBuilder builder,
			bool multiple = false)
		{
			builder.Metadata.Requeue = true;
			builder.Metadata.Multiple = multiple;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQConsumerBuilder WithArgument<TValue>(
			this IRabbitMQConsumerBuilder builder,
			string argument,
			TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));

			if (builder.Metadata.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));

			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}

		public static IRabbitMQConsumerBuilder WithPriority(
			this IRabbitMQConsumerBuilder configuration,
			byte priority)
		{
			return configuration.WithArgument("x-priority", priority);
		}
	}
}