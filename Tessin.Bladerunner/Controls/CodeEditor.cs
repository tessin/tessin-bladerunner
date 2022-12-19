using LINQPad;
using LINQPad.Controls;
using System;

namespace Tessin.Bladerunner.Controls
{
    public class CodeEditor : Control, ITextControl
    {

        public CodeEditor(string value, string language)
        {
            value = value.Replace('\u00A0', ' '); //cleaning up all non-breaking spaces.
            value = value.Replace("\r\n", "\n").Replace("\r","\n");
            
            var container = new Div();
            container.SetClass("code-editor");
            this.VisualTree.Add(container);
            
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
                            value: '{value.Replace("\n",@"\n").Replace("'", @"\'")}',
                            language: '{language}',
                            lineNumbers: false,
                            lineDecorationsWidth: 7,
                            automaticLayout: true,
                            wordWrap: true,  
                            padding : {{
                                top: 30,
                            }},
                            renderLineHighlight : false,
                            folding: false,
                            minimap: {{
                                enabled: false        
                            }},
                            theme: 'vs-dry'
                        }});
                        window.s
                    }});
                }}, 100);
                CodeEditorGetValue = function() {{
                    if(!!window.editor) {{
                        return window.editor.getValue();  
                    }}
                    else
                    {{
                        return 'ERROR';
                    }}  
                }}
            ");
        }

        public string Text
        {
            get => (string)Util.InvokeScript(true, "CodeEditorGetValue");
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
