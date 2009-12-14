using System.IO;

namespace ImageUploader
{
	public interface IImageResizer
	{
		StreamHolder ResizeStream(Stream inputStream, string name, int maxWidth, int maxHeight);
		StreamHolder ResizeStream(Stream inputStream, string name, int maxWidth);
	}
}