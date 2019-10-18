//
//
//

class Editor implements IEditor {
    private document: Document;
    private instance: any;
    
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

    constructor(document: Document, instance: any) {
        this.document = document;
        this.instance = instance;
        this.document.addEventListener("selectstart", this.onSelectionStart);
        this.document.addEventListener("selectionchange", this.onSelectionChange);
        this.document.body.setAttribute("contenteditable", "true");
        //this.document.body.addEventListener("change", this.onChange);
    }

    /*getContent(): string {
        return this.document.body.innerHTML;
    }

    setContent(content: string): void {
        this.document.body.innerHTML = content;
    }*/

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
        this.instance.invokeMethodAsync("OnSelectionStart");
    }

    private onSelectionChange(): void {
        console.log("[Editor.ts] Editor.onSelectionChange");
    }
}