/**
 * @interface IEditor
 *
 */
interface IEditor {

    /**
     * @prop {string} content
     *
     */
    content: string;

    /**
     * @param {ISelectionFormat} format
     *
     */
    formatSelection(format: ISelectionFormat): void;

    /**
     * @param {string} text
     *
     */
    setContent(text: string): void;

    /**
     * @return {string}
     *
     */
    getContent(): string;
}

interface INode {
    name: string;
    next: INode;
}