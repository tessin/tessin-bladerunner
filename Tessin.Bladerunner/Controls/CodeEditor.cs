using LINQPad;
using LINQPad.Controls;
using System;
using System.IO;
using System.Text.Json;

namespace Tessin.Bladerunner.Controls
{
    public class CodeEditorConstructionOptions
    {
        // Add to this class from here as needed
        // https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.IStandaloneEditorConstructionOptions.html

        // These override any defaults that have been set in the __monacoEditor_create function

        /// <summary>
        /// initial text value
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// language (i.e. syntax highlight; for example "markdown")
        /// </summary>
        public string language { get; set; }
    }

    public class CodeEditor : Control, ITextControl
    {
        private static readonly string MonacoEditorSnippet;

        static CodeEditor()
        {
            MonacoEditorSnippet = new StreamReader(typeof(CodeEditor).Assembly.GetManifestResourceStream("Tessin.Bladerunner.Controls.CodeEditor.min.js")).ReadToEnd();
        }

        public CodeEditor(CodeEditorConstructionOptions options = null)
            : base("div")
        {
            //value = value.Replace('\u00A0', ' '); //cleaning up all non-breaking spaces.
            //value = value.Replace("\r\n", "\n").Replace("\r", "\n");

            Util.HtmlHead.AddScriptFromUri(Cdnjs.Require_jsUrl);
            Util.HtmlHead.AddScript(MonacoEditorSnippet);

            this.Rendering += (_, _) =>
            {
                HtmlElement.SetAttribute("class", "code-editor");
                HtmlElement.InvokeScript(false, "__monacoEditor_create", HtmlElement.ID, JsonSerializer.Serialize(options));
            };
        }

        public string Text
        {
            get
            {
                return (string)HtmlElement.InvokeScript(true, "__monacoEditor_getText", HtmlElement.ID);
            }
            set
            {
                HtmlElement.InvokeScript(false, "__monacoEditor_setText", HtmlElement.ID, value);
            }
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public event EventHandler TextInput;

        /// <summary>
        /// Not supported
        /// </summary>
        public void SelectAll()
        {
        }
    }
}
