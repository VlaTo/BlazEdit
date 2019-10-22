/**
 * @class Editor
 *
 */
class Editor implements IEditor {
    private document: Document;
    private callback: IDotNetCallback;
    
    /**
     * @prop {string} [content] gets or sets content for editing.
     * @returns {string} 
     */
    get content(): string {
         return this.document.body.innerHTML;
    }

    set content(value: string) {
        this.document.body.innerHTML = value;
    }

    /**
     * 
     * @param {Document} document
     * @param {any} instance
     */
    constructor(document: Document, callback: IDotNetCallback) {
        this.document = document;
        this.callback = callback;

        const onSelectionStart = this.onSelectionStart.bind(this);
        const onSelectionChange = this.onSelectionChange.bind(this);

        this.document.addEventListener("selectstart", onSelectionStart);
        this.document.addEventListener("selectionchange", onSelectionChange);

        this.document.body.setAttribute("contenteditable", "true");
    }

    /**
     * Wraps current selection with htmlTag specified.
     * @param {string} htmlTag
     */
    apply(htmlTag: string): void {
        const selection = this.document.getSelection();

        if (0 < selection.rangeCount) {
            for (let index = 0; index < selection.rangeCount; index++) {
                const range = selection.getRangeAt(index);
                const element = this.document.createElement(htmlTag);

                range.surroundContents(element);
            }
        } else {
            ;
        }
    }

    private onSelectionStart(e: UIEvent): void {
        this.callback.invokeMethodAsync("OnSelectionStart", e);
    }

    private onSelectionChange(e: UIEvent): void {
        this.callback.invokeMethodAsync("OnSelectionChange", e);
    }
}