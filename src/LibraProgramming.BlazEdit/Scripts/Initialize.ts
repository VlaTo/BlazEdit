//
//
//

(window as any).editor = (elementId: string, callback: IDotNetCallback) => {
    const element = document.getElementById(elementId) as HTMLIFrameElement;

    const temp = document.getElementById(element.id);
    const host = element as HTMLIFrameElement;

    (window as any).editor = new Editor(document, callback);
};