//
//
//

class Editor implements IEditor {
    document: Document;

    constructor(document: Document) {
        this.document = document;
    }

    getContent(): string {
        return this.document.body.innerHTML;
    }

    setContent(content: string): void {
        this.document.body.setAttribute("contenteditable", "true");
        this.document.body.addEventListener("change", this.onChange);
        this.document.body.innerHTML = content;
    }

    private onChange(): void {
        console.log("[Editor.ts] Editor.onChange");
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
}