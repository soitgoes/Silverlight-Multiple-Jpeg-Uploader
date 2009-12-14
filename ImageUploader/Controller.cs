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
using ICSharpCode.SharpZipLib.Zip;
using Path = System.IO.Path;

namespace ImageUploader
{
	public class Controller
	{
		public IImageResizer ImageResizer { get; set; }
		public IZipper Zipper { get; set; }
        
		//for each image on the filesystem pipe through all the streams up to the server

		//Resize and produce all the images through the resizer and into memory streams

		//pass all the memory streams into the zipper

		//Pass the Zipper out put stream into teh request stream.

		public void ResizeAndZipToStream(Stream responseStream, params string[] filePaths)
		{
			var streams = new List<StreamHolder>();
			foreach (var path in filePaths)
			{
				using (var fs = File.OpenRead(path))
				{
					string fileName = Path.GetFileName(path);
					string ext = Path.GetExtension(path);
					string prefix = Path.GetFileNameWithoutExtension(path);

					var streamHolder = new StreamHolder(fs, fileName);

					var sizes = new int[100, 300, 500];
					foreach (int size in sizes)
					{
						streams.Add(ImageResizer.ResizeStream(fs, prefix + "_" + size.ToString() + ext, size));
					}
					Zipper.CreateZip(responseStream, streams.ToArray());
				}
			}
		}

	}
}
