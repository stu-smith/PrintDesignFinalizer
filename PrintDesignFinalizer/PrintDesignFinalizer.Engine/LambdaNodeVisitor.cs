using System;

namespace PrintDesignFinalizer.Engine
{
	public class LambdaNodeVisitor : INodeVisitor
	{
		public LambdaNodeVisitor(Action<INode> nodeAction)
		{
			_nodeAction = nodeAction;
		}

		public void OnVisitNode(INode node)
		{
			_nodeAction(node);
		}

		private readonly Action<INode> _nodeAction;
	}
}
