import { StaticCustomerResponse } from "../customer";

export interface DirectAccountResponse
{
	accountID: number;
	username: string;
	email: string;
	role: string;
	customer: StaticCustomerResponse;
}
