using System;
using System.Drawing;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class MakePixelsNonAlphaNodeOperation : INodeOperation
	{
		public MakePixelsNonAlphaNodeOperation(Func<INode, Color> pickBaseColor)
		{
			_pickBaseColor = pickBaseColor;
		}

		public void Apply(INode node)
		{
			if (node.FullPath == null)
			{
				throw new InvalidOperationException();
			}

			var baseColor = _pickBaseColor(node);

			var bitmap = BitmapReaderWriter.LoadBitmapFromFile(node.FullPath);

			using (var bmp = BitmapReaderWriter.LockReadWrite(bitmap))
			{
				for (var y = 0; y < bmp.Height; ++y)
				{
					for (var x = 0; x < bmp.Width; ++x)
					{
						var color = bmp.GetPixel(x, y);

						if (color.A == 0 || color.A == 255)
						{
							continue;
						}

						var ca = color.A / 255.0;
						var ba = 1.0 - ca;

						var r = Clamp((color.R * ca) + (baseColor.R * ba));
						var g = Clamp((color.G * ca) + (baseColor.G * ba));
						var b = Clamp((color.B * ca) + (baseColor.B * ba));

						bmp.SetPixel(x, y, Color.FromArgb(255, r, g, b));
					}
				}
			}

			bitmap.Save(node.FullPath);
		}

		private static byte Clamp(double v)
		{
			if (v < 0.0)
			{
				return 0;
			}
			if (v > 255)
			{
				return 255;
			}

			return (byte)v;
		}

		private readonly Func<INode, Color> _pickBaseColor;
	}
}
