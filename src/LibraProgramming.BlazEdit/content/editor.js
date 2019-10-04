//
//
//
var Editor = /** @class */ (function () {
    function Editor(document) {
        this.document = document;
    }
    Editor.prototype.getContent = function () {
        return this.document.body.innerHTML;
    };
    Editor.prototype.setContent = function (content) {
        this.document.body.setAttribute("contenteditable", "true");
        this.document.body.addEventListener("change", this.onChange);
        this.document.body.innerHTML = content;
    };
    Editor.prototype.onChange = function () {
        console.log("[Editor.ts] Editor.onChange");
    };
    Editor.prototype.apply = function (htmlTag) {
        var selection = this.document.getSelection();
        if (0 < selection.rangeCount) {
            for (var index = 0; index < selection.rangeCount; index++) {
                var range = selection.getRangeAt(index);
                var element = this.document.createElement(htmlTag);
                range.surroundContents(element);
            }
        }
        else {
            ;
        }
    };
    return Editor;
}());
//
//
//
//
//
//
window.editor = function (elementId) {
    var element = document.getElementById(elementId);
    if (!element) {
        return;
    }
    var doc = element.contentDocument || element.contentWindow.document;
    window.editor = new Editor(doc);
};
