//
//
//

interface IEditor {
    getContent(): string;

    setContent(content: string): void;

    apply(html: string): void;
}