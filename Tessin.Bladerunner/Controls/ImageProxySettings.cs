using System;
using System.Collections.Generic;
using System.Text;

//https://github.com/tessin/image-proxy/blob/feature/smart/README.md

namespace Tessin.Bladerunner.Controls
{
    public class ImageProxySettings
    {
        public ImageProxySettings(Uri endpoint, string functionKey)
        {
            Endpoint = endpoint;
            FunctionKey = functionKey;
        }

        public Uri Endpoint { get; set; }

        public string FunctionKey { get; set; }
    }
}
