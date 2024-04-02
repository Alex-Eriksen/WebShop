import { StaticProductResponse } from "../product";
import { StaticOrderResponse } from "../order";
import { StaticDiscountResponse } from "../discount";

export interface DirectTransactionResponse {
    transactionID: number;
    product: StaticProductResponse;
    order: StaticOrderResponse;
    productAmount: number;
    productPrice: number;
    discount: StaticDiscountResponse | null;
    created_At: string;
    modified_At: string;
}
