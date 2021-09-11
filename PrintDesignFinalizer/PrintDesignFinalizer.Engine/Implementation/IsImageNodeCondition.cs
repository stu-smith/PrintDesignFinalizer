using System.IO;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class IsImageNodeCondition : INodeCondition
	{
		public bool Test(INode node)
		{
			if (node.FullPath == null)
			{
				return false;
			}

			var extension = Path.GetExtension(node.FullPath);

			switch (extension)
			{
				case ".png":
					return true;
				default:
					return false;
			}
		}
	}
}
