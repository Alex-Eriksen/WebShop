import { StaticAccountResponse } from "../account";
import { StaticCountryResponse } from "../country";
import { StaticOrderResponse } from "../order";
import { StaticPaymentResponse } from "../payment";

export interface DirectCustomerResponse
{
	customerID: number;
	account: StaticAccountResponse;
	firstName: string;
	lastName: string;
	phoneNumber: string;
	country: StaticCountryResponse;
	zipCode: number;
	gender: string;
	created_At: Date;
	payments: StaticPaymentResponse[];
	orders: StaticOrderResponse[];
}
