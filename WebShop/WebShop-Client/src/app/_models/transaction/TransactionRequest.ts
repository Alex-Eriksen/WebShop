export interface TransactionRequest {
    productID: number;
    orderID: number;
    productAmount: number;
    productPrice: number;
    discountID: number | null;
    created_At: Date;
    modified_At: Date;
}
