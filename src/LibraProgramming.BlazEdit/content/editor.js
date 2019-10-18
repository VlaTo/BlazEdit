//
//
//
var Editor = /** @class */ (function () {
    function Editor(document, instance) {
        this.document = document;
        this.instance = instance;
        this.document.addEventListener("selectstart", this.onSelectionStart);
        this.document.addEventListener("selectionchange", this.onSelectionChange);
        this.document.body.setAttribute("contenteditable", "true");
        //this.document.body.addEventListener("change", this.onChange);
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
    /*getContent(): string {
        return this.document.body.innerHTML;
    }

    setContent(content: string): void {
        this.document.body.innerHTML = content;
    }*/
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
        this.instance.invokeMethodAsync("OnSelectionStart");
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
window.editor = function (elementId, instance) {
    var element = document.getElementById(elementId);
    if (!element) {
        return;
    }
    var doc = element.contentDocument || element.contentWindow.document;
    window.editor = new Editor(doc, instance);
};
