using System.IO;

namespace ImageUploader
{
	public class StreamHolder
	{
		private readonly Stream stream;
		private readonly string name;
		public Stream Stream { get { return stream;} }
		public string Name {get { return name;}}

		public StreamHolder(Stream stream, string name)
		{
			this.stream = stream;
			this.name = name;
		}
	}
}