using Microsoft.Extensions.DependencyInjection;
using PrintDesignFinalizer.Engine;
using PrintDesignFinalizer.Engine.Implementation;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace PrintDesignFinalizer.ConsoleApp
{
	static class Program
	{
		static void Main(string? directory)
		{
			var serviceProvider = CreateServiceProvider();

			var processor = serviceProvider.GetRequiredService<Processor>();

			processor.Apply(directory);
		}

		static IServiceProvider CreateServiceProvider()
		{
			var serviceProvider = new ServiceCollection()
				.AddSingleton<Processor>()
				.AddSingleton<IConsoleOperations, ConsoleOperations>()
				.AddPrintDesignFinalizerEngine()
				.AddRules()
				.BuildServiceProvider();

			return serviceProvider;
		}

		static IServiceCollection AddRules(this IServiceCollection services)
		{
			var isTShirt = new NodePathMatchesRegexNodeCondition(new Regex(@"[\\/]TShirts[\\/]"));
			var isImage = new IsImageNodeCondition();
			var hasSemiTransparent = new ImageHasSemiTransparentPixelsNodeCondition();

			var makePixelsNonAlpha = new MakePixelsNonAlphaNodeOperation(GetBaseColor);

			services
				.AddSingleton(new NodeRule(new INodeCondition[] { isImage, isTShirt, hasSemiTransparent }, makePixelsNonAlpha));

			return services;
		}

		static Color GetBaseColor(INode node)
		{
			if (node.FullPath == null)
			{
				throw new InvalidOperationException();
			}

			var match = _baseColorRegex.Match(node.FullPath);

			if (match == null || !match.Success)
			{
				throw new InvalidOperationException($"Could not determine base color for file: {node.FullPath}");
			}

			var color = match.Groups["COLOR"].Value;

			return color switch
			{
				"Black" => Color.Black,
				"White" => Color.White,
				_ => throw new InvalidOperationException($"Could not determine base color '{color}' for file: {node.FullPath}")
			};
		}

		static Regex _baseColorRegex = new Regex(@"\-(?<COLOR>[A-Za-z]+)\.png", RegexOptions.Compiled);
	}
}
