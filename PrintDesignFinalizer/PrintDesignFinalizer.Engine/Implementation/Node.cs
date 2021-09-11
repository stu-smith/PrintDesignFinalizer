using System;
using System.Collections.Generic;
using System.IO;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class Node : INode
	{
		public Node(string fullPath)
		{
			FullPath = fullPath;
		}

		public IList<INode> ChildNodes { get; } = new List<INode>();

		public string? FullPath { get; }

		public INodeOperation? OperationToApply
		{
			get => _operationToApply;
			set => _operationToApply = value;
		}

		public void Visit(INodeVisitor visitor)
		{
			visitor.OnVisitNode(this);

			foreach (var childNode in ChildNodes)
			{
				childNode.Visit(visitor);
			}
		}

		private INodeOperation? _operationToApply;
	}
}
