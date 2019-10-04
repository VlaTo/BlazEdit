﻿//
//
//

(window as any).editor = (elementId: string) => {
    const element = document.getElementById(elementId) as HTMLIFrameElement;

    if (!element) {
        return;
    }

    const doc = element.contentDocument || element.contentWindow.document;

    (window as any).editor = new Editor(doc);
};