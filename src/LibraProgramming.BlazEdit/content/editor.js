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
        var ranges = new Array();
        var item = {
            Start: null,
            End: null,
            StartOffset: -1,
            EndOffset: -1,
            Text: ""
        };
        ranges.push(item);
        this.callback.invokeMethodAsync("OnSelectionStart", ranges);
    };
    Editor.prototype.onSelectionChange = function (e) {
        var selection = this.document.getSelection();
        var ranges = new Array();
        if (0 < selection.rangeCount) {
            for (var index = 0; index < selection.rangeCount; index++) {
                var range = selection.getRangeAt(index);
                var item = {
                    Start: null,
                    End: null,
                    StartOffset: range.startOffset,
                    EndOffset: range.endOffset,
                    Text: range.toString()
                };
                var node = range.startContainer;
                while (null != node) {
                    item.Start = {
                        Name: node.nodeName,
                        NextNode: item.Start
                    };
                    node = node.parentNode;
                }
                node = range.endContainer;
                while (null != node) {
                    item.End = {
                        Name: node.nodeName,
                        NextNode: item.End
                    };
                    node = node.parentNode;
                }
                ranges.push(item);
            }
        }
        this.callback.invokeMethodAsync("OnSelectionChange", ranges);
    };
    return Editor;
}());
//
//
//
window.editor = function (elementId, callback) {
    var element = document.getElementById(elementId);
    //const temp = document.getElementById(element.id);
    //const host = element as HTMLIFrameElement;
    window.editor = new Editor(document, callback);
};
