import { StaticCustomerResponse } from "../customer";
import { StaticTransactionResponse } from "../transaction";

export interface DirectOrderResponse {
    orderID: number;
    customer: StaticCustomerResponse;
    orderTotal: number;
    transactions: StaticTransactionResponse[];
}
