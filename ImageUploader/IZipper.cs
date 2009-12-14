using System.IO;

namespace ImageUploader
{
	public interface IZipper
	{
		void CreateZip(Stream outputStream, StreamHolder[] streamsToZip);
	}
}