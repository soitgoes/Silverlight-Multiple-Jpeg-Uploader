using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ImageUploader
{
	public class Zipper : IZipper
	{

		public void CreateZip(Stream outputStream, params StreamHolder[] streamsToZip)
		{
			using (var s = new ZipOutputStream(outputStream))
			{
				byte[] buffer = new byte[4096];
				s.SetLevel(9); // 0 - store only to 9 - means best compression

				foreach (var streamHolder in streamsToZip)
				{
					ZipEntry entry = new ZipEntry(streamHolder.Name);
					s.PutNextEntry(entry);
					StreamUtils.Copy(streamHolder.Stream, s, buffer);
					s.CloseEntry();
				}
			}
			//close and dispose of all streamsToZip
			foreach (var holders in streamsToZip)
			{
				holders.Stream.Close();
				holders.Stream.Dispose();
			}
		}
	}
}