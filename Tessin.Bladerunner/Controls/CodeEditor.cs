using LINQPad;
using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Controls
{
    public class CodeEditor : Control, ITextControl
    {

        public CodeEditor(string value, string language)
        {
            var container = new Div();
            container.SetClass("code-editor");
            this.VisualTree.Add(container);

            Util.HtmlHead.AddScriptFromUri("https://unpkg.com/monaco-editor@latest/min/vs/loader.js");

            Util.HtmlHead.AddScript(@$"
                require.config({{
                    paths: {{ vs: 'https://unpkg.com/monaco-editor@latest/min/vs' }}
                }});
                window.MonacoEnvironment = {{
                    getWorkerUrl: function (workerId, label) {{
                        return `data:text/javascript;charset=utf-8,${{encodeURIComponent(`
                            self.MonacoEnvironment = {{
                                baseUrl: 'https://unpkg.com/monaco-editor@latest/min/'
                            }};
                        importScripts('https://unpkg.com/monaco-editor@latest/min/vs/base/worker/workerMain.js');`)}}`;
                    }}
                }};
                setTimeout(function() {{
                    require(['vs/editor/editor.main'], function () {{
                        window.editor = monaco.editor.create(document.getElementById('{container.HtmlElement.ID}'), {{
                            value: `{value}`,
                            language: '{language}',
                            lineNumbers: false,
                            lineDecorationsWidth: 0,
                            minimap: {{
                                enabled: false        
                            }},
                            theme: 'vs-dry'
                        }});
                        window.s
                    }});
                }}, 100);
                CodeEditorGetValue = function() {{
                    return window.editor.getValue();    
                }}
            ");
        }

        public string Text
        {
            get
            {
                return (string)Util.InvokeScript(true, "CodeEditorGetValue");
            }
            set
            {
            }
        }

        public event EventHandler TextInput;

        public void SelectAll()
        {

        }
    }
}
