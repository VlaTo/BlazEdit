/**
 * @interface ISelectionRange
 *
 * Represent single range of selection
 */

interface ISelectionRange {
    /**
     * @prop {ISelectionNode} Start
     * Returns range's start node
     */
    Start: ISelectionNode;

    /**
     * @prop {ISelectionNode} End
     * Returns range's end node
     */
    End: ISelectionNode;

    /**
     * @prop {number} StartOffset
     * Returns range's start offset
     */
    StartOffset: number;

    /**
     * @prop {number} EndOffset
     * Returns range's end offset
     */
    EndOffset: number;

    /**
     * @prop {string} Text
     * Returns range's selection text
     */
    Text: string;
}