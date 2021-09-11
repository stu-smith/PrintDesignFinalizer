using PrintDesignFinalizer.Engine;
using PrintDesignFinalizer.Engine.Implementation;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class EngineExtensions
	{
		public static IServiceCollection AddPrintDesignFinalizerEngine(this IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddSingleton<IFileSystemOperations, FileSystemOperations>()
				.AddSingleton<IChooseNodeOperationVisitor, ChooseNodeOperationVisitor>();

			return serviceCollection;
		}
	}
}
