interface ISelectionRange {
    start: ISelectionNode;
    end: ISelectionNode;
    startOffset: number;
    endOffset: number;
    text: string;
}