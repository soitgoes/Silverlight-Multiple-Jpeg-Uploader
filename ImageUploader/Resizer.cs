using System.IO;
using FluxJpeg.Core;
using FluxJpeg.Core.Decoder;
using FluxJpeg.Core.Encoder;
using FluxJpeg.Core.Filtering;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ImageUploader
{
	public class JpgImageResizer : IImageResizer
	{

		public StreamHolder ResizeStream(Stream inputStream, string name, int maxWidth, int maxHeight)
		{

			// Decode
			DecodedJpeg jpegIn = new JpegDecoder(inputStream).Decode();

			if (!ImageResizer.ResizeNeeded(jpegIn.Image, maxWidth))
			{
				return new StreamHolder(inputStream, name);
			}
			else
			{
				// Resize
				DecodedJpeg jpegOut = new DecodedJpeg(
					new ImageResizer(jpegIn.Image)
						.Resize(maxWidth, maxHeight, ResamplingFilters.NearestNeighbor),
					jpegIn.MetaHeaders); // Retain EXIF details

				// Encode
				var outputStream = new MemoryStream();
				new JpegEncoder(jpegOut, 90, outputStream).Encode();
				// Display 
				outputStream.Seek(0, SeekOrigin.Begin);
				return new StreamHolder(outputStream, name);
			}


		}

		public StreamHolder ResizeStream(Stream inputStream, string name, int maxWidth)
		{

			// Decode
			DecodedJpeg jpegIn = new JpegDecoder(inputStream).Decode();

			if (!ImageResizer.ResizeNeeded(jpegIn.Image, maxWidth))
			{
				return new StreamHolder(inputStream, name);
			}
			else
			{
				// Resize
				DecodedJpeg jpegOut = new DecodedJpeg(
					new ImageResizer(jpegIn.Image)
						.Resize(maxWidth, ResamplingFilters.NearestNeighbor),
					jpegIn.MetaHeaders); // Retain EXIF details

				// Encode
				var outputStream = new MemoryStream();
				new JpegEncoder(jpegOut, 90, outputStream).Encode();
				// Display 
				outputStream.Seek(0, SeekOrigin.Begin);
				return new StreamHolder(outputStream, name);
			}
		}
	}
}
