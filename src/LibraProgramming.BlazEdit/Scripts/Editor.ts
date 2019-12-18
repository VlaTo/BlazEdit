/**
 * @class Editor
 *
 */
class Editor implements IEditor {
    private host: HTMLIFrameElement;
    private contentDocument: Document;
    private callback: IDotNetCallback;

    /**
     * @prop {string} [content] gets or sets content for editing.
     * @returns {string} 
     */
    get content(): string {
        return this.contentDocument.body.innerHTML;
    }

    set content(value: string) {
        this.contentDocument.body.innerHTML = value;
    }

    /**
     * 
     * @param {Document} document
     * @param {any} instance
     */
    constructor(host: HTMLIFrameElement, callback: IDotNetCallback) {
        this.host = host;
        this.callback = callback;

        const onSelectionStart = this.onSelectionStart.bind(this);
        const onSelectionChange = this.onSelectionChange.bind(this);

        this.contentDocument = host.contentDocument;

        this.contentDocument.addEventListener("selectstart", onSelectionStart);
        this.contentDocument.addEventListener("selectionchange", onSelectionChange);
        this.contentDocument.body.setAttribute("contenteditable", "true");
    }

    /**
     * Wraps current selection with htmlTag specified.
     * @param {ISelectionFormat} format
     */
    formatSelection(format: ISelectionFormat): void {
        const selection = this.contentDocument.getSelection();

        if (0 < selection.rangeCount) {
            for (let index = 0; index < selection.rangeCount; index++) {
                const range = selection.getRangeAt(index);
                const element = this.contentDocument.createElement(format.elementName);

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
            Start: null,
            End: null,
            StartOffset: -1,
            EndOffset: -1,
            Text: ""
        };

        ranges.push(item);

        this.callback.invokeMethodAsync("OnSelectionStart", ranges);
    }

    private onSelectionChange(e: UIEvent): void {
        const selection = this.contentDocument.getSelection();
        const ranges: ISelectionRange[] = new Array<ISelectionRange>();

        if (0 < selection.rangeCount) {
            for (let index = 0; index < selection.rangeCount; index++) {
                const range = selection.getRangeAt(index);
                const item: ISelectionRange = {
                    Start: null,
                    End: null,
                    StartOffset: range.startOffset,
                    EndOffset: range.endOffset,
                    Text: range.toString()
                };

                let node = range.startContainer;
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

        this.callback
            .invokeMethodAsync("OnSelectionChange", ranges)
            .then(
                data => console.log('OnSelectionChange completed'),
                reason => console.log('OnSelectionChange failed')
            );
    }
}