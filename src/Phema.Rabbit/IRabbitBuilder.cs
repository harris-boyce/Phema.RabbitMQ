﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Phema.Rabbit.Consumers")]
[assembly: InternalsVisibleTo("Phema.Rabbit.Producers")]

namespace Phema.Rabbit
{
	/// <summary>
	/// Used for configuring rabbit services
	/// </summary>
	public interface IRabbitBuilder
	{
		IServiceCollection Services { get; }
	}
	
	internal class RabbitBuilder : IRabbitBuilder
	{
		public RabbitBuilder(IServiceCollection services)
		{
			Services = services;
		}
		
		public IServiceCollection Services { get; }
	}
}