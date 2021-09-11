using PrintDesignFinalizer.Engine;
using System.Collections.Generic;

namespace PrintDesignFinalizer.ConsoleApp
{
	public class Processor
	{
		public Processor(
			IConsoleOperations console,
			IFileSystemOperations fileSystemOperations,
			IChooseNodeOperationVisitor chooseNodeOperationVisitor
			)
		{
			_console = console;
			_fileSystemOperations = fileSystemOperations;
			_chooseNodeOperationVisitor = chooseNodeOperationVisitor;
		}

		public void Apply(string? directory)
		{
			if (string.IsNullOrWhiteSpace(directory))
			{
				_console.Fatal("Must specify --directory <directory-to-scan>");
			}

			_console.Info($"Scanning directory [{directory}]...");

			var rootNode = _fileSystemOperations.ReadDirectory(directory!);

			_console.Info("Determining which bitmaps to update...");

			rootNode.Visit(_chooseNodeOperationVisitor);

			var (ignoreFileCount, processFileCount, plan) = CountNodes(rootNode);

			_console.Info($"Found {processFileCount + ignoreFileCount} images total, {processFileCount} to update, {ignoreFileCount} to ignore.");

			foreach (var node in plan)
			{
				_console.Info($"   {node.OperationToApply!.GetType().Name} -> [{node.FullPath}]");
			}

			_console.Info("Starting processing...");

			foreach (var node in plan)
			{
				_console.Info($"   {node.OperationToApply!.GetType().Name} -> [{node.FullPath}]...");

				node.OperationToApply.Apply(node);
			}

			_console.Info("Done processing.");
		}

		private static (int IgnoreFileCount, int ProcessFileCount, INode[] plan) CountNodes(INode rootNode)
		{
			var ignoreFileCount = 0;
			var processFileCount = 0;
			var plan = new List<INode>();

			rootNode.Visit(new LambdaNodeVisitor(n =>
			{
				if (n.OperationToApply == null)
				{
					++ignoreFileCount;
				}
				else
				{
					++processFileCount;
					plan.Add(n);
				}
			}));

			return (ignoreFileCount, processFileCount, plan.ToArray());
		}

		private readonly IConsoleOperations _console;
		private readonly IFileSystemOperations _fileSystemOperations;
		private readonly IChooseNodeOperationVisitor _chooseNodeOperationVisitor;
	}
}
