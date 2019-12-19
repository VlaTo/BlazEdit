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
     * @constructor
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
     * @func formatSelection
     * @param {ISelectionFormat} format
     * Wraps current selection with htmlTag specified.
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
                data => {},
                reason => {}
            );
    }

    private onSelectionChange(e: UIEvent): void {
        const selection = this.contentDocument.getSelection();
        const ranges: ISelectionRange[] = this.buildSelectionRanges(selection);

        this.callback
            .invokeMethodAsync("OnSelectionChange", ranges)
            .then(
                data => {} ,
                reason => {}
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

                const nodes = new Array<{ node: Node, item: ISelectionNode }>();

                this.createStartNodes(range, item, nodes);
                this.createEndNodes(range, item, nodes);

                ranges.push(item);
            }
        }

        return ranges;
    }

    private findNodeIndex(parents: { node: Node, item: ISelectionNode }[], actual: Node): number {
        for (let index = 0; index < parents.length; index++) {
            if (parents[index].node === actual) {
                return index;
            }
        }

        return -1;
    }

    private createStartNodes(range: Range, item: ISelectionRange, nodes: { node: Node, item: ISelectionNode }[]): void {
        let last: ISelectionNode = null;
        let current = range.startContainer;

        while (null != current) {
            const node: ISelectionNode = {
                Name: current.nodeName,
                NextNode: null
            };

            if (null == item.Start) {
                item.Start = node;
            }

            if (last != null) {
                last.NextNode = node;
            }

            last = node;

            nodes.push({ node: current, item: node });

            current = current.parentNode;
        }
    }

    private createEndNodes(range: Range, item: ISelectionRange, nodes: { node: Node, item: ISelectionNode }[]): void {
        let current = range.endContainer;
        let last: ISelectionNode = null;

        while (null != current) {
            const index = this.findNodeIndex(nodes, current);
            const node: ISelectionNode = (-1 < index)
                ? nodes[index].item
                : { Name: current.nodeName, NextNode: null };

            if (null == item.End) {
                item.End = node;
            }

            if (-1 < index) {
                break;
            }

            if (null != last) {
                last.NextNode = node;
            }

            last = node;

            current = current.parentNode;
        }
    }
}