//
//
//

(window as any).editor = (element: HTMLElement) => {
    //const host = document.getElementById(elementId) as HTMLIFrameElement;

    const temp = document.getElementById(element.id);
    const host = element as HTMLIFrameElement;

    if (!!host) {
        const doc = host.contentDocument || host.contentWindow.document;
        (window as any).editor = new Editor(doc);
    }
};