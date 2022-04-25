using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tessin.Bladerunner.Controls
{
	public enum ImageType
	{
		/// <summary>
		/// As source, i.e. no processing.
		/// </summary>
		None,
		Jpg,
		Png,
		Webp,
	}

	[Flags]
	public enum ImageCrop
	{
		None = 0,
		Face = 1 << 0, // smart, face detection
		Hint = 1 << 1, // smart, crop hint (more general)
		Rect = 1 << 2, // manual
	}

	public class ImageProxyFile
	{
		static readonly Regex Pattern = new Regex(@"(?<endpoint>.*)(?<hash>[a-z0-9-[/]]+)/(?<width>\d+)?x(?<height>\d+)?(?:-crop-(?<crop>face|hint))*(?:-color-(?<color>[0-9a-f]{6}))*?\.(?<type>jpg|png|webp)$", RegexOptions.RightToLeft);

		private const int EndpointIndex = 1;
		private const int HashIndex = 2;
		private const int WidthIndex = 3;
		private const int HeightIndex = 4;
		private const int CropIndex = 5;
		private const int ColorIndex = 6;
		private const int TypeIndex = 7;

		public string Hash { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public ImageCrop Crop { get; set; }
		public string Color { get; set; }
		public ImageType OutputType { get; set; }
		public string Endpoint { get; set; }

		public static ImageProxyFile Parse(string url)
		{
			var image = new ImageProxyFile();

			var match = Pattern.Match(url);
			if (!match.Success) return null;

			image.Endpoint = match.Groups[EndpointIndex].Value;
			image.Hash = match.Groups[HashIndex].Value;
			var width = match.Groups[WidthIndex];
			var height = match.Groups[HeightIndex];
			var crop = match.Groups[CropIndex];
			var color = match.Groups[ColorIndex];
			var type = match.Groups[TypeIndex];

			if (width.Success)
			{
				image.Width = Convert.ToInt32(width.Value, CultureInfo.InvariantCulture);
			}

			if (height.Success)
			{
				image.Height = Convert.ToInt32(height.Value, CultureInfo.InvariantCulture);
			}

			if (crop.Success)
			{
				var c = crop.Captures;
				for (int i = 0; i < c.Count; i++)
				{
					switch (c[i].Value)
					{
						case "face":
							{
								image.Crop |= ImageCrop.Face;
								break;
							}

						case "hint":
							{
								image.Crop |= ImageCrop.Hint;
								break;
							}

						default:
							throw new InvalidOperationException("unhandled crop parameter");
					}
				}
			}

			if (color.Success)
			{
				image.Color = color.Value;
			}

			// required

			switch (type.Value)
			{
				case "jpg":
					{
						image.OutputType = ImageType.Jpg;
						break;
					}
				case "png":
					{
						image.OutputType = ImageType.Png;
						break;
					}
				case "webp":
					{
						image.OutputType = ImageType.Webp;
						break;
					}
				default:
					{
						throw new InvalidOperationException("unhandled type parameter");
					}
			}

			return image;
		}

		private ImageProxyFile()
		{
		}

		//https://www.tessin.dev/api/images/bb438d68613bde4a2aef9efebb376e3a17da335dabb3a93190c505316cf18ec8/3261x2170-crop-hint-color-d04720.jpg

		public override string ToString()
		{
			return $"{Endpoint}{Hash}/{Width}x{Height}-crop-{Crop.ToString().ToLower()}-color-{Color}.{this.OutputType.ToString().ToLower()}";
		}
	}
}
