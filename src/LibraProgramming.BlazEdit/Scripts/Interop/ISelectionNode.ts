/**
 * @interface ISelectionNode
 *
 */
interface ISelectionNode {
    /**
     * @property {ISelectionNode} nextNode
     *
     */
    nextNode: ISelectionNode;

    /**
     * @property {string} name
     *
     */
    name: string;
}