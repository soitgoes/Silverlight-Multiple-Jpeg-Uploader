using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Path = System.IO.Path;

namespace ImageUploader
{
	public class Controller
	{
		public IImageResizer ImageResizer { get; set; }
		public IZipper Zipper { get; set; }

		private byte[] buffer = new byte[4096];

		//for each image on the filesystem pipe through all the streams up to the server

		//Resize and produce all the images through the resizer and into memory streams

		//pass all the memory streams into the zipper

		//Pass the Zipper out put stream into teh request stream.

		public void ResizeAndZipToStream(Stream outputStream, params string[] filePaths)
		{
			var streams = new List<StreamHolder>();
			MemoryStream ms;
			foreach (var path in filePaths)
			{
				var fs = File.OpenRead(path);
				{
					ms = new MemoryStream();
					StreamUtils.Copy(fs, ms, buffer);

					string fileName = Path.GetFileName(path);
					string ext = Path.GetExtension(path);
					string prefix = Path.GetFileNameWithoutExtension(path);

					var streamHolder = new StreamHolder(ms, fileName);

					var sizes = new int[] { 100, 300, 500 };
					foreach (int size in sizes)
					{

						ms.Seek(0, SeekOrigin.Begin);
						streams.Add(ImageResizer.ResizeStream(ms, prefix + "_" + size.ToString() + ext, size));
					}


				
				}
			}
			Zipper.CreateZip(outputStream, streams.ToArray());
		}
	}
}
