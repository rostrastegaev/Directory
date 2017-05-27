import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Result } from './result';
import { Observable } from 'rxjs/Observable';
import { ForkJoinObservable } from 'rxjs/observable/ForkJoinObservable';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/map';

export interface IHttpHeader {
    header: string;
    value: string;  
}

export interface IHttpInterceptor {
    intercept(): Observable<Result>;
}

export class HttpService {
    private _httpHeaders: Headers;
    private _http: Http;
    private _interceptors: IHttpInterceptor[];

    constructor(http: Http){
        this._httpHeaders = new Headers();
        this._http = http;
        this._interceptors = [];
    }
    
    add(httpHeader: IHttpHeader): void {
        this._httpHeaders.append(httpHeader.header, httpHeader.value);
    }
    
    remove(header: string): void {
        this._httpHeaders.delete(header);
    }

    addInterceptor(interceptor: IHttpInterceptor): void {
        this._interceptors.push(interceptor);
    }

    get(url: string): Observable<Result> {
        return this.runRequest(() => this._http.get(url, {headers: this._httpHeaders}));
    }

    post(url: string, obj: any): Observable<Result> {
        return this.runRequest(() => this._http.post(url, obj, {headers: this._httpHeaders}));
    }

    put(url: string, obj: any): Observable<Result> {
        return this.runRequest(() => this._http.put(url, obj, {headers: this._httpHeaders}));
    }

    delete(url: string): Observable<Result> {
        return this.runRequest(() => this._http.delete(url, {headers: this._httpHeaders}));
    }

    private runRequest(method: () => Observable<Response>): Observable<Result> {
        return this.notifyInterceptors().flatMap(() => method().map(this.toResult));
    }

    private toResult(response: Response): Result {
        return Result.fromJSON(response.json());
    }

    private notifyInterceptors(): Observable<Result> {
        let results = new Array<Observable<Result>>();
        for (let interceptor of this._interceptors) {
            results.push(interceptor.intercept());
        }
        return new ForkJoinObservable(results);
    }
}