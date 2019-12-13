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
     * @param {ISelectionFormat} format
     */
    formatSelection(format: ISelectionFormat): void {
        const selection = this.document.getSelection();

        if (0 < selection.rangeCount) {
            for (let index = 0; index < selection.rangeCount; index++) {
                const range = selection.getRangeAt(index);
                const element = this.document.createElement(format.elementName);

                range.surroundContents(element);
            }
        } else {
            ;
        }
    }

    /**
     * @param text
     *
     */
    setContent(text: string): void {
        this.content = text;
    }

    /**
     *
     */
    getContent(): string {
        return this.content;
    }

    private onSelectionStart(e: UIEvent): void {
        const ranges: ISelectionRange[] = new Array<ISelectionRange>();
        let item: ISelectionRange = {
            start: null,
            end: null,
            startOffset: -1,
            endOffset: -1,
            text: ""
        };

        ranges.push(item);

        this.callback.invokeMethodAsync("OnSelectionStart", ranges);
    }

    private onSelectionChange(e: UIEvent): void {
        const selection = this.document.getSelection();
        const ranges: ISelectionRange[] = new Array<ISelectionRange>();

        if (0 < selection.rangeCount) {
            for (let index = 0; index < selection.rangeCount; index++) {
                const range = selection.getRangeAt(index);
                const item: ISelectionRange = {
                    start: null,
                    end: null,
                    startOffset: range.startOffset,
                    endOffset: range.endOffset,
                    text: range.toString()
                };

                let node = range.startContainer;
                while (null != node) {
                    item.start = {
                        name: node.nodeName,
                        nextNode: item.start
                    };
                    node = node.parentNode;
                }

                node = range.endContainer;
                while (null != node) {
                    item.end = {
                        name: node.nodeName,
                        nextNode: item.end
                    };
                    node = node.parentNode;
                }

                ranges.push(item);
            }
        }

        this.callback.invokeMethodAsync("OnSelectionChange", ranges);
    }
}