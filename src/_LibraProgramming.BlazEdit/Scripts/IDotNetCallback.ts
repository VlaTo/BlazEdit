/**
 *  @interface IDotNetCallback
 */
interface IDotNetCallback {
    /**
     * 
     * @param {} html
     */
    invokeMethodAsync(methodName: string, ...args: any): Promise<any>;
}