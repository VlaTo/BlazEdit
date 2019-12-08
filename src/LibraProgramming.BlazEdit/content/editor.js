//
//
//
var Editor = /** @class */ (function () {
    function Editor(document) {
        this.document = document;
        this.document.addEventListener("selectstart", this.onSelectionStart);
        this.document.addEventListener("selectionchange", this.onSelectionChange);
        this.document.body.setAttribute("contenteditable", "true");
        //this.document.body.addEventListener("change", this.onChange);
    }
    Editor.prototype.getContent = function () {
        return this.document.body.innerHTML;
    };
    Editor.prototype.setContent = function (content) {
        this.document.body.innerHTML = content;
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
    Editor.prototype.onSelectionStart = function () {
        console.log("[Editor.ts] Editor.onSelectionStart");
    };
    Editor.prototype.onSelectionChange = function () {
        console.log("[Editor.ts] Editor.onSelectionChange");
    };
    return Editor;
}());
//
//
//
//
//
//
window.editor = function (element) {
    //const host = document.getElementById(elementId) as HTMLIFrameElement;
    var temp = document.getElementById(element.id);
    var host = element;
    if (!!host) {
        var doc = host.contentDocument || host.contentWindow.document;
        window.editor = new Editor(doc);
    }
};
