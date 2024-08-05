import {Toast} from 'primereact/toast';
import React, {createRef, ReactNode} from 'react';


export class MessageService {
    private static toastRef = createRef<Toast>();

    public static success(summary: string, detail: string, life: number = 3000): void {
        MessageService.toastRef.current?.show({severity: 'success', summary, detail, life});
    }

    public static error(summary: string, detail: string, life: number = 3000): void {
        MessageService.toastRef.current?.show({severity: 'error', summary, detail, life});
    }
}