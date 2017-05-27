import { Injectable } from '@angular/core';
import { IHttpInterceptor, HttpService, IHttpHeader } from  '../commonModule/http.service';
import { StorageService, IStorageItem } from '../commonModule/localStorage.service';
import { Result } from '../commonModule/result';
import { Token } from '../authModule/token';
import { Observable } from 'rxjs/Observable';

export abstract class SignInRequest {
    grantType: string;
}

export class PasswordSignInRequest extends SignInRequest {
    email: string;
    password: string;

    constructor(email: string, password: string) {
        super();
        this.grantType = 'password';
        this.email = email;
        this.password = password;
    }
}

export class RefreshSignInRequest extends SignInRequest {
    token: string;

    constructor(token: string) {
        super();
        this.grantType = 'refresh_token'
        this.token = token;
    }
}

export interface IRegisterRequest {
    email: string;
    password: string;
    confirmation: string;
}

export class AuthService implements IHttpInterceptor {

    private _storageService: StorageService;
    private _httpService: HttpService;
    private readonly _storageKey: string = 'token';

    constructor(storageService: StorageService, httpService: HttpService){
        this._storageService = storageService;
        this._httpService = httpService;
        this.setHeaderFromStorage();
    }

    private setHeaderFromStorage(): void {
        let tokenItem = this._storageService.get(this._storageKey);
        if (tokenItem != null) {
            let token = tokenItem.data as Token;
            this._httpService.remove('Authorization');
            this._httpService.add({header: 'Authorization', value: 'Bearer ' + token.accessToken});
        }
    }

    isAuth(): boolean {
        return this._storageService.get(this._storageKey) != null;
    }

    register(request: IRegisterRequest): Observable<Result> {
        return this._httpService.post('/api/account/register', request);
    }

    intercept(): Observable<Result> {
        let token = this._storageService.get(this._storageKey).data as Token;
        if (token.validTo <= new Date()) {
            return this._signIn(new RefreshSignInRequest(token.refreshToken));
        }
        return Observable.empty();
    }

    signIn(request: PasswordSignInRequest): Observable<Result> {        
        return this._signIn(request);
    }

    private _signIn(request: SignInRequest): Observable<Result> {
        let observable = this._httpService.post('/api/account/signin', request);
        observable.subscribe(result =>
        {
            if (!result.isSuccess) {
                return;
            }
            let token = result.data as Token;
            this._storageService.put({key: this._storageKey, data: token});
            this.setHeaderFromStorage();
        });

        return observable;
    }




}