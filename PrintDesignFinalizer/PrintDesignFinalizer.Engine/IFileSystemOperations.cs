namespace PrintDesignFinalizer.Engine
{
	public interface IFileSystemOperations
	{
		INode ReadDirectory(string directory);
	}
}
