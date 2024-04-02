import { AccountRequest } from "../account";
import { CustomerRequest } from "../customer";

export interface NewCustomerRequest
{
	customer: CustomerRequest;
	account: AccountRequest;
}
