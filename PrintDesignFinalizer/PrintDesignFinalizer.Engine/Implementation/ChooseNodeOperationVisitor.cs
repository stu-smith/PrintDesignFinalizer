using System.Collections.Generic;
using System.Linq;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class ChooseNodeOperationVisitor : IChooseNodeOperationVisitor
	{
		public ChooseNodeOperationVisitor(IEnumerable<NodeRule> rules)
		{
			_rules = rules.ToArray();
		}

		public void OnVisitNode(INode node)
		{
			foreach (var rule in _rules)
			{
				rule.TestNodeAndConditionallyAssignOperation(node);
			}
		}

		private readonly NodeRule[] _rules;
	}
}
