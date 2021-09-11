using System.Collections.Generic;

namespace PrintDesignFinalizer.Engine
{
	public interface INode
	{
		IList<INode> ChildNodes { get; }

		string? FullPath { get; }

		INodeOperation? OperationToApply { get; set; }

		void Visit(INodeVisitor visitor);
	}
}
