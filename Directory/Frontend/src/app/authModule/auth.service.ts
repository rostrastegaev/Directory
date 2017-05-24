import { Injectable } from '@angular/core';
import { HttpService, IHttpHeader, IHttpInterceptor } from '../commonModule/http.service';
import { StorageService } from '../commonModule/storage.service';
import { Result } from '../commonModule/result';
import { Observable } from 'rxjs/Observable';
import { RegistrationRequest, SignInRequest, PasswordSignInRequest, RefreshTokenSignInRequest } from './auth.requests';

class AuthResponse {
    accessToken: string;
    refreshToken: string;
    validTo: Date;
}

@Injectable()
export class AuthService implements IHttpInterceptor {
    private _httpService: HttpService;
    private _storageService: StorageService;
    private readonly _auhtorizationHeader: string = 'Authorization';
    private readonly _authKey: string = 'auth.service';

    constructor(httpService: HttpService, storageService: StorageService) {
        this._httpService = httpService;
        this._storageService = storageService;        
    }

    register(request: RegistrationRequest): Observable<Result> {
        return this._httpService.post('/api/account/register', request);
    }

    signIn(request: PasswordSignInRequest): Observable<Result> {
        return this._signIn(request);
    }

    intercept(): Observable<Result> {
        let authData = this._storageService.get(this._authKey);
        if (authData == null) {
            return Observable.empty();
        }

        let authResponse = authData.data as AuthResponse;
        let now = new Date();
        if (authResponse.validTo > now) {
            return Observable.empty();
        }

        return this._signIn(new RefreshTokenSignInRequest(authResponse.refreshToken));
    }

    isAuth(): boolean {
        let authData = this._storageService.get(this._authKey);
        return authData == null;
    }

    signOut(): void {
        this._storageService.remove(this._authKey);
        this._httpService.remove(this._auhtorizationHeader);
    }

    private _signIn(request: SignInRequest): Observable<Result> {
        let observable = this._httpService.post('/api/account/signin', request);
        observable.subscribe((result) => {
            let authResponse = result.data as AuthResponse;
            this._storageService.put({
                key: this._authKey,
                data: authResponse
            });
            this._httpService.remove(this._auhtorizationHeader);
            this._httpService.add({
                header: this._auhtorizationHeader,
                value: 'Bearer ' + authResponse.accessToken
            });
        });
        return observable;
    }
}