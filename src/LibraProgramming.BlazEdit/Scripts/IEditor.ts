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
     * @param {string} html
     *
     */
    apply(html: string): void;
}