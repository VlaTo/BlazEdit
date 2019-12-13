/**
 * @interface ISelectionChangeCallbackArgs
 *
 */
interface ISelectionChangeCallbackArgs {
    /**
     * @property {ISelectionNode} startNode
     *
     */
    startNode: ISelectionNode;

    /**
     * @property {ISelectionNode} endNode
     *
     */
    endNode: ISelectionNode;

    /**
     * @property {string} text
     *
     */
    text: string;
}