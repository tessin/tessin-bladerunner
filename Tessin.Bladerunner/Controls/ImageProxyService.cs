using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
	public class ImageProxyService
	{
		public ImageProxyService(ImageProxySettings settings)
		{
			Endpoint = settings.Endpoint;
			FunctionKey = settings.FunctionKey;
        }

        public Uri Endpoint { get; set; }

		public string FunctionKey { get; set; }

		Uri Authorize(string url)
		{
			if (!(Endpoint.Host == "localhost"))
			{
				if (url.Contains('?'))
				{
					url += "&code=";
				}
				else
				{
					url += "?code=";
				}
				url += Uri.EscapeDataString(FunctionKey);
			}
			return new Uri(Endpoint, url);
		}

		public async Task<ImageProxyResponse> UploadImageFromFile(string path)
		{
			using (var http = new HttpClient())
			{
				var req = new HttpRequestMessage(HttpMethod.Post, Authorize("/api/UploadImage"));
				req.Content = new StreamContent(File.OpenRead(path));
				using (var res = await http.SendAsync(req))
				{
					return JsonConvert.DeserializeObject<ImageProxyResponse>(await res.Content.ReadAsStringAsync());
				}
			}
		}
	}
}

public class ImageProxyResponse
{
	public bool ok { get; set; }
	public string code { get; set; }
	public string message { get; set; }
	public UploadImageData data { get; set; }
}

public class UploadImageData
{
	public string hash { get; set; }
	public int width { get; set; }
	public int height { get; set; }
	public bool face { get; set; }
	public bool hint { get; set; }
	public string color { get; set; }
	public string href { get; set; }
}
