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
     * @func getContent
     *
     */
    getContent(): string {
        return this.content;
    }

    private onSelectionStart(e: UIEvent): void {
        const selection = this.contentDocument.getSelection();
        const ranges: ISelectionRange[] = this.buildSelectionRanges(selection);
        this.callback
            .invokeMethodAsync("OnSelectionStart", ranges)
            .then(
                data => console.log('OnSelectionStart completed'),
                reason => console.log('OnSelectionStart failed')
            );
    }

    private onSelectionChange(e: UIEvent): void {
        const selection = this.contentDocument.getSelection();
        const ranges: ISelectionRange[] = this.buildSelectionRanges(selection);

        this.callback
            .invokeMethodAsync("OnSelectionChange", ranges)
            .then(
                data => console.log('OnSelectionChange completed'),
                reason => console.log('OnSelectionChange failed')
            );
    }

    private buildSelectionRanges(selection: Selection): ISelectionRange[] {
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

                const parents = new Array<{ node: Node, item: ISelectionNode }>();

                // start
                let node = range.startContainer;

                while (null != node) {
                    item.Start = {
                        Name: node.nodeName,
                        NextNode: item.Start
                    };

                    parents.push({ node: node, item: item.Start });

                    node = node.parentNode;
                }

                // end
                node = range.endContainer;

                let position = this.findParent(parents, node);

                if (0 > position) {
                    while (null != node) {

                        position = this.findParent(parents, node.parentNode);

                        item.End = {
                            Name: node.nodeName,
                            NextNode: (0 > position) ? item.End : parents[position].item
                        };

                        if (-1 < position) {
                            break;
                        }
                    }
                } else {
                    item.End = parents[position].item;
                }

                ranges.push(item);
            }
        }

        return ranges;
    }

    private findParent(parents: { node: Node, item: ISelectionNode }[], actual: Node): number {
        for (let index = 0; index < parents.length; index++) {
            if (parents[index].node === actual) {
                return index;
            }
        }

        return -1;
    }
}