export class Result {
    public isSuccess: boolean;
    public errorCodes: number[];
    public data: any;

    constructor(isSuccess: boolean, errorCodes: number[], data: any) {
        this.isSuccess = isSuccess;
        this.errorCodes = errorCodes;
        this.data = data;
    }

    static fromJSON(json: any): Result {
        return new Result(json.isSuccess, json.errorCodes, json.data);
    }
}