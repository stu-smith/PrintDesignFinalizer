using System;
using System.Drawing;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class ImageHasSemiTransparentPixelsNodeCondition : INodeCondition
	{
		public bool Test(INode node)
		{
			if (node.FullPath == null)
			{
				throw new InvalidOperationException();
			}

			var bitmap = BitmapReaderWriter.LoadBitmapFromFile(node.FullPath);

			using (var bmp = BitmapReaderWriter.LockReadOnly(bitmap))
			{
				for (var y = 0; y < bmp.Height; ++y)
				{
					for (var x = 0; x < bmp.Width; ++x)
					{
						var alpha = bmp.GetAlpha(x, y);

						if (alpha > 0 && alpha < 255)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
