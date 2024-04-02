import { StaticCustomerResponse } from "../customer";

export interface DirectPaymentResponse
{
	paymentID: number;
	customer: StaticCustomerResponse;
	paymentType: string;
	cardNumber: string;
	provider: string;
	expiry: Date;
	created_At: Date;
}
