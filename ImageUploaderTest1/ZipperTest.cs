using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ImageUploader;
using NUnit.Framework;

namespace ImageUploaderTest1
{
	[TestFixture]
	public class ZipperTest
	{
		private string directory;
		private const string zipName = "output.zip";
		private const string jpgName = "output.jpg";
		private byte[] buffer = new byte[4096];
		public string BasePath { get; set; }
		[SetUp]
		public void Setup()
		{
			directory = Environment.CurrentDirectory;
			BasePath= this.directory.Substring(0, this.directory.LastIndexOf("\\")) + "\\Files\\";
			Cleanup();
		}
		

		[TearDown]
		public void TearDown()
		{
			Cleanup();
		}

		private void Cleanup()
		{
			if (File.Exists(BasePath + zipName))
				File.Delete(BasePath + zipName);
			if (File.Exists(BasePath + jpgName))
				File.Delete(BasePath + jpgName);
		}

		[Test]
		public void CanResizeImageAsStream()
		{
			IImageResizer resizer = new JpgImageResizer();
			string path = Directory.GetFiles(BasePath)[0];
			string filename = Path.GetFileName(path);
			var holder = resizer.ResizeStream(File.OpenRead(path), filename,300);
			using (var imgFileOutput = File.OpenWrite(BasePath + jpgName))
			{
				StreamUtils.Copy(holder.Stream, imgFileOutput, buffer);	
			}
		}


		[Test]
		public void DoesSharpZipLibWorks()
		{
				var files = Directory.GetFiles(BasePath);
			using (var outputStream = File.Create(BasePath + zipName))
			{

				using (ZipOutputStream s = new ZipOutputStream(outputStream))
				{
					byte[] buffer = new byte[4096];
					s.SetLevel(9); // 0 - store only to 9 - means best compression
					foreach (var file in files)
					{
						using (var inputStream = File.OpenRead(file))
						{
							s.PutNextEntry(new ZipEntry(Path.GetFileName(file)));
							StreamUtils.Copy(inputStream, s, buffer);
							s.CloseEntry();
						}
					}
		
				}
			}
			//Verify that there are 4 files in the zip
			using (var inputStream  = new ZipInputStream(File.OpenRead(BasePath + zipName)))
			{
				ZipEntry entry;
				int count = 0;
				do
				{
					entry = inputStream.GetNextEntry();
					if (entry != null)
					{
						Assert.IsTrue(inputStream.CanDecompressEntry, "Can decompress the output file");
						count++;
					}
				} while (entry != null);
				Assert.IsTrue(count == 4, "There should be 4 entries in the archive");
			}
			
		}


		[Test]
		public void CanZipFromStream()
		{
			
			var paths = Directory.GetFiles(BasePath);
			var zipper = new Zipper();

			var streams = new List<StreamHolder>();
			foreach (var path in paths)
			{
				streams.Add(new StreamHolder ( File.OpenRead(path), Path.GetFileName(path) ));
			}
			Stream outputStream = File.OpenWrite(BasePath + zipName);
			zipper.CreateZip(outputStream, streams.ToArray());
		}
	}
}