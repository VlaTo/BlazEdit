//
//
//

class Editor implements IEditor {
    document: Document;

    constructor(document: Document) {
        this.document = document;
        this.document.addEventListener("selectstart", this.onSelectionStart);
        this.document.addEventListener("selectionchange", this.onSelectionChange);
        this.document.body.setAttribute("contenteditable", "true");
        //this.document.body.addEventListener("change", this.onChange);
    }

    getContent(): string {
        return this.document.body.innerHTML;
    }

    setContent(content: string): void {
        this.document.body.innerHTML = content;
    }

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

    private onSelectionStart(): void {
        console.log("[Editor.ts] Editor.onSelectionStart");
    }

    private onSelectionChange(): void {
        console.log("[Editor.ts] Editor.onSelectionChange");
    }
}