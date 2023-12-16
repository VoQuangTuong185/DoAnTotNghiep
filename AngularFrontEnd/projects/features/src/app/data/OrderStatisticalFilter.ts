export class OrderStatisticalFilter {
    public Method!: string;
    public DateFrom!: Date;
    public DateTo!: Date;
    constructor(Method: string, DateFrom: Date, DateTo: Date){
        this.Method = Method;
        this.DateFrom = DateFrom;
        this.DateTo = DateTo;
    }
}