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
}