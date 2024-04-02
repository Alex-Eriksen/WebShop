import { StaticCustomerResponse } from "../customer";

export interface DirectCountryResponse
{
	countryID: number;
	countryName: number;
	customers: StaticCustomerResponse[];
}
