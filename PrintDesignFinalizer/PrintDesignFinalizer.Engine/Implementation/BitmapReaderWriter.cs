using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PrintDesignFinalizer.Engine.Implementation
{
	public class BitmapReaderWriter : IDisposable
	{
		public static Bitmap LoadBitmapFromFile(string filename)
		{
			using var filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);

			return new Bitmap(filestream);
		}

		public static BitmapReaderWriter LockReadOnly(Bitmap bitmap)
		{
			return new BitmapReaderWriter(bitmap, ImageLockMode.ReadOnly);
		}

		public static BitmapReaderWriter LockReadWrite(Bitmap bitmap)
		{
			return new BitmapReaderWriter(bitmap, ImageLockMode.ReadWrite);
		}

		private BitmapReaderWriter(Bitmap bitmap, ImageLockMode lockMode)
		{
			if(bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			{
				throw new InvalidOperationException();
			}

			_bitmap = bitmap;
			_imageLockMode = lockMode;

			var rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);

			_bitmapData = _bitmap.LockBits(rect, lockMode, _bitmap.PixelFormat);

			var numBytes = _bitmapData.Stride * _bitmap.Height;

			_bytes = new byte[numBytes];

			System.Runtime.InteropServices.Marshal.Copy(_bitmapData.Scan0, _bytes, 0, numBytes);
		}

		public void Dispose()
		{
			if(_bitmap != null)
			{
				if (_imageLockMode == ImageLockMode.ReadWrite)
				{
					var numBytes = Math.Abs(_bitmapData!.Stride) * _bitmap.Height;

					System.Runtime.InteropServices.Marshal.Copy(_bytes!, 0, _bitmapData.Scan0, numBytes);
				}

				_bitmap.UnlockBits(_bitmapData!);

				_bitmap = null;
				_bitmapData = null;
				_bytes = null;
			}
		}

		public int Width => CheckDisposed(_bitmap, b => b.Width);

		public int Height => CheckDisposed(_bitmap, b => b.Height);

		public byte GetAlpha(int x, int y)
		{
			if (_bitmap == null || _bitmapData == null || _bytes == null)
			{
				throw new ObjectDisposedException(nameof(BitmapReaderWriter));
			}

			var offset = (y * _bitmapData.Stride) + (x * 4);

			var a = _bytes[offset + 3];

			return a;
		}

		public Color GetPixel(int x, int y)
		{
			if(_bitmap == null || _bitmapData == null || _bytes == null)
			{
				throw new ObjectDisposedException(nameof(BitmapReaderWriter));
			}

			var offset = (y * _bitmapData.Stride) + (x * 4);

			var a = _bytes[offset + 3];
			var r = _bytes[offset + 2];
			var g = _bytes[offset + 1];
			var b = _bytes[offset + 0];

			return Color.FromArgb(a, r, g, b);
		}

		public void SetPixel(int x, int y, Color c)
		{
			if (_bitmap == null || _bitmapData == null || _bytes == null)
			{
				throw new ObjectDisposedException(nameof(BitmapReaderWriter));
			}

			if (_imageLockMode != ImageLockMode.ReadWrite)
			{
				throw new InvalidOperationException();
			}

			var offset = (y * _bitmapData.Stride) + (x * 4);

			_bytes[offset + 3] = c.A;
			_bytes[offset + 2] = c.R;
			_bytes[offset + 1] = c.G;
			_bytes[offset + 0] = c.B;
		}

		private static V CheckDisposed<T, V>(T? t, Func<T, V> get)
		{
			if(t == null)
			{
				throw new ObjectDisposedException(nameof(BitmapReaderWriter));
			}

			return get(t);
		}

		private Bitmap? _bitmap;
		private BitmapData? _bitmapData;
		private byte[]? _bytes;
		private readonly ImageLockMode _imageLockMode;
	}
}
