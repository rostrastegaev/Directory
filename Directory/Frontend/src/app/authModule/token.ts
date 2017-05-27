export class Token {
    public accessToken: string;
    public refreshToken: string;
    public validTo: Date;

    constructor(accessToken: string, refreshToken: string, validTo: Date) {
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.validTo = validTo;
    }

    static fromJSON(json: any): Token {
        return new Token(json.acessToken, json.refreshToken, json.validTo);
    }
}