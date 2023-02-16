// https://github.com/microsoft/monaco-editor/blob/main/samples/browser-amd-editor/index.html
const fs = require('node:fs')

function bootstrap(__CDNJS__, __MSBUILD_CONFIG__) {
    (async function (window) {
        const env = __MSBUILD_CONFIG__;

        console.log("Hello LINQPad!", { env });

        if (!window["MonacoEnvironment"]) {
            if (env === "Debug") {
                // This code is necessary to preserve CSS loaded dynamically from JS (dev only)

                let head = [];

                const document_open = document.open;
                document.open = function () {
                    head = [];
                    for (const el of document.head.childNodes) {
                        head.push(el);
                    }
                    return document_open.call(document);
                };

                const document_close = document.close;
                document.close = function () {
                    const ret = document_close.call(document);
                    let i = head.findIndex((el) => el.tagName === "SCRIPT");
                    for (; i < head.length; i++) {
                        const styleLink = head[i];
                        if (styleLink.tagName === "STYLE" || styleLink.tagName === "LINK") {
                            document.head.appendChild(styleLink);
                        }
                    }
                    return ret;
                };
            }

            /** @type {string} */
            const cdnjs = __CDNJS__;

            window.require.config({
                paths: {
                    vs: cdnjs + "/vs",
                },
            });

            const workerSnippet = [
                `self.MonacoEnvironment = { baseUrl: ${JSON.stringify(cdnjs)} }`,
                `importScripts(${JSON.stringify(
                    cdnjs + "/vs/base/worker/workerMain.min.js"
                )})`,
            ];

            window["MonacoEnvironment"] = {
                getWorkerUrl: () =>
                    `data:text/javascript;charset=utf-8,${encodeURIComponent(
                        workerSnippet.join(";")
                    )}`,
            };

            const editors = new WeakMap();

            function getMonacoEditor(/** @type {string} */ elementId) {
                const el = document.getElementById(elementId);
                if (el) {
                    return editors.get(el);
                }
                return undefined;
            }

            function getTextModel(/** @type {string} */ elementId) {
                const ed = getMonacoEditor(elementId);
                if (ed) {
                    return ed.getModel();
                }
                return undefined;
            }

            window["__monacoEditor_create"] = function (elementId, options) {
                window.require(["vs/editor/editor.main"], (monaco) => {
                    if (getMonacoEditor(elementId)) {
                        // Note sure if this really is a problem but it can't work any other way
                        console.warn("will not re-create monaco editor", { elementId });
                        return;
                    }
                    const el = document.getElementById(elementId);
                    const ed = monaco.editor.create(el, {
                        lineNumbers: false,
                        lineDecorationsWidth: 7,
                        automaticLayout: false, // don't use automatic layout; it is expensive
                        wordWrap: true,
                        padding: {
                            top: 30,
                        },
                        renderLineHighlight: false,
                        folding: false,
                        minimap: {
                            enabled: false,
                        },
                        theme: "vs-dry",
                        ...(JSON.parse(options) ?? {}),
                    });

                    let timeout
                    const obs = new ResizeObserver((e) => {
                        clearTimeout(timeout)
                        timeout = setTimeout(() => {
                            ed.layout({
                                width: e[0].contentRect.width,
                                height: e[0].contentRect.height,
                            });
                        },500)
                    });

                    obs.observe(el);

                    editors.set(el, ed);
                });
            };

            window["__monacoEditor_getText"] = function (elementId) {
                const m = getTextModel(elementId);
                if (m) {
                    return m.getValue();
                }
                return "";
            };

            window["__monacoEditor_setText"] = function (elementId, newValue) {
                const m = getTextModel(elementId);
                if (m) {
                    m.setValue(newValue);
                }
            };
        }
    })(window);
}

// build function with variable substitution

const s = bootstrap.toString();
const i = s.indexOf("{");
const j = s.lastIndexOf("}");

const { CDNJS_BASE, MSBUILD_CONFIG } = process.env

console.debug("build code editor", { CDNJS_BASE, MSBUILD_CONFIG })

fs.writeFileSync(process.argv.slice(2)[0],
    bootstrap
        .toString()
        .slice(i + 1, j)
        .replace("__CDNJS__", JSON.stringify(CDNJS_BASE))
        .replace("__MSBUILD_CONFIG__", JSON.stringify(MSBUILD_CONFIG))
)
