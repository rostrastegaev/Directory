export class RegistrationRequest {
    email: string;
    password: string;
    confirmation: string;
}

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

export class RefreshTokenSignInRequest extends SignInRequest {
    token: string;

    constructor(token: string) {
        super();
        this.grantType = 'refresh_token';
        this.token = token;
    }
}