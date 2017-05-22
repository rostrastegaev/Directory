import { Injectable } from '@angular/core';

export interface IStorageItem {
    key: string;
    data: any;
}

@Injectable()
export class StorageService {
    put(item: IStorageItem): void {
        localStorage.setItem(item.key, JSON.stringify(item.data));
    }

    get(key: string): IStorageItem {
        return {
            key: key,
            data: JSON.parse(localStorage.getItem(key))
        };
    }

    remove(key: string): void {
        localStorage.removeItem(key);
    }
}