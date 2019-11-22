/**
 * @class Editor
 *
 */
var Editor = /** @class */ (function () {
    /**
     *
     * @param {Document} document
     * @param {any} instance
     */
    function Editor(document, callback) {
        this.document = document;
        this.callback = callback;
        var onSelectionStart = this.onSelectionStart.bind(this);
        var onSelectionChange = this.onSelectionChange.bind(this);
        this.document.addEventListener("selectstart", onSelectionStart);
        this.document.addEventListener("selectionchange", onSelectionChange);
        this.document.body.setAttribute("contenteditable", "true");
    }
    Object.defineProperty(Editor.prototype, "content", {
        /**
         * @prop {string} [content] gets or sets content for editing.
         * @returns {string}
         */
        get: function () {
            return this.document.body.innerHTML;
        },
        set: function (value) {
            this.document.body.innerHTML = value;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * Wraps current selection with htmlTag specified.
     * @param {ISelectionFormat} format
     */
    Editor.prototype.formatSelection = function (format) {
        var selection = this.document.getSelection();
        if (0 < selection.rangeCount) {
            for (var index = 0; index < selection.rangeCount; index++) {
                var range = selection.getRangeAt(index);
                var element = this.document.createElement(format.elementName);
                range.surroundContents(element);
            }
        }
        else {
            ;
        }
    };
    /**
     * @param text
     *
     */
    Editor.prototype.setContent = function (text) {
        this.content = text;
    };
    /**
     *
     */
    Editor.prototype.getContent = function () {
        return this.content;
    };
    Editor.prototype.onSelectionStart = function (e) {
        this.callback.invokeMethodAsync("OnSelectionStart", e);
    };
    Editor.prototype.onSelectionChange = function (e) {
        var selection = this.document.getSelection();
        var text = "";
        if (0 < selection.rangeCount) {
            for (var index = 0; index < selection.rangeCount; index++) {
                var range = selection.getRangeAt(index);
                text = range.toString();
                var node = range.startContainer;
                var path = new Array();
                while (null !== node) {
                    path.unshift(node.nodeName);
                    node = node.parentNode;
                }
            }
        }
        this.callback.invokeMethodAsync("OnSelectionChange", text);
    };
    return Editor;
}());
//
//
//
window.editor = function (elementId, callback) {
    var element = document.getElementById(elementId);
    if (!element) {
        return;
    }
    var doc = element.contentDocument || element.contentWindow.document;
    window.editor = new Editor(doc, callback);
};
