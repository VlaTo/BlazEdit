/**
* Editor initialization
*
*/
(window as any).editor = (elementId: string, callback: IDotNetCallback) => {
    const element = document.getElementById(elementId) as HTMLIFrameElement;
    (window as any).editor = new Editor(element, callback);
};