using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintDesignFinalizer.Engine
{
	public sealed class NodeRule
	{
		public NodeRule(IEnumerable<INodeCondition> conditions, INodeOperation operation)
		{
			Conditions = conditions.ToArray();
			Operation = operation;

			if(Conditions.Length == 0)
			{
				throw new InvalidOperationException();
			}
		}

		public INodeCondition [] Conditions { get; }
		public INodeOperation Operation { get; }

		public void TestNodeAndConditionallyAssignOperation(INode node)
		{
			if (node.OperationToApply != null)
			{
				throw new InvalidOperationException();
			}

			foreach (var condition in Conditions)
			{
				if (!condition.Test(node))
				{
					return;
				}
			}

			node.OperationToApply = Operation;
		}
	}
}
