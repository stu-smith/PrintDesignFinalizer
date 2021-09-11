using System.Text.RegularExpressions;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class NodePathMatchesRegexNodeCondition : INodeCondition
	{
		public NodePathMatchesRegexNodeCondition(Regex regex)
		{
			_regex = regex;
		}

		public bool Test(INode node)
		{
			if(node.FullPath==null)
			{
				return false;
			}

			return _regex.IsMatch(node.FullPath);
		}

		private readonly Regex _regex;
	}
}
