using System.IO;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class FileSystemOperations : IFileSystemOperations
	{
		public INode ReadDirectory(string directory)
		{
			var node = new Node(directory);

			var subDirectories = Directory.GetDirectories(directory);

			foreach (var subDirectory in subDirectories)
			{
				var subDirectoryNode = ReadDirectory(subDirectory);

				node.ChildNodes.Add(subDirectoryNode);
			}

			var files = Directory.GetFiles(directory);

			foreach (var file in files)
			{
				var fileNode = new Node(file);

				node.ChildNodes.Add(fileNode);
			}

			return node;
		}
	}
}
