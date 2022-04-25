using LINQPad.Controls;
using System;
using System.Threading.Tasks;

namespace Tessin.Bladerunner.Controls
{
    public class ImageProxyBox : LINQPad.Controls.Div, ITextControl
    {
        public bool IsUploading { get; set; }

        private readonly LINQPad.Controls.TextBox _textBox;

        private readonly ImageProxySettings _settings;

        private readonly LINQPad.DumpContainer _imageContainer;

        public ImageProxyBox(ImageProxySettings settings, string initialValue = null, string initialCatalog = null, string width = "-webkit-fill-available",
            Action<LINQPad.Controls.TextBox> onTextInput = null)
        {
            _settings = settings;
            initialCatalog = initialCatalog ?? System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            this.SetClass("file-box image-proxy-box");

            _textBox = new LINQPad.Controls.TextBox(initialValue, width, onTextInput);

            _textBox.TextInput += (sender, args) =>
            {
                TextInput?.Invoke(sender, args);
                UpdateImageContainer(_textBox.Text);
            };

            this.VisualTree.Add(_textBox);

            var btnSelectFile = new IconButton(Icons.FolderOpen, async (_) =>
            {
                var path = ShowOpenFileDialog(initialCatalog);

                if (!string.IsNullOrEmpty(path))
                {
                    _imageContainer.Content = "Uploading...";
                    IsUploading = true;
                    _textBox.Text = await UploadFile(path);
                    IsUploading = false;
                    TextInput?.Invoke(_textBox, null!);
                    UpdateImageContainer(_textBox.Text);
                }
            });

            _imageContainer = new LINQPad.DumpContainer();

            this.VisualTree.Add(btnSelectFile);
            this.VisualTree.Add(_imageContainer);
        }

        public void UpdateImageContainer(string url)
        {
            var image = ImageProxyFile.Parse(url);

            if (image == null)
            {
                _imageContainer.Content = "";
            } 
            else
            {
                image.Width = 230;
                image.Height = 129;
                _imageContainer.Content = LINQPad.Util.Image(image.ToString()); 
            }
        }

        public async Task<string> UploadFile(string path)
        {
            await Task.Delay(10000);

            var service = new ImageProxyService(_settings);
            var result = await service.UploadImageFromFile(path);
            return result.data.href;
        }

        public string ShowOpenFileDialog(string initialCatalog)
        {
            // You can't hide from me Joe!
            Type tBridge = typeof(LINQPad.Util).Assembly.GetType("LINQPad.ExecutionModel.UIBridge");
            Type tService = typeof(LINQPad.Util).Assembly.GetType("LINQPad.ExecutionModel.IRuntimeUIServices");
            var prop = tBridge.GetProperty("UIServices");
            var oService = prop.GetValue(null, null);
            if (oService != null)
            {
                var method = tService.GetMethod("ShowOpenFileDialog");
                var result = method.Invoke(oService, new[] { initialCatalog });
                return result?.ToString();
            }
            return null;
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public string Text
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        public event EventHandler TextInput;

        public LINQPad.Controls.TextBox TextBox => _textBox;
    }
}
