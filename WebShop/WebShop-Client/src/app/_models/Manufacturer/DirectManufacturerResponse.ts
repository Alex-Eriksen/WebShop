import { StaticProductResponse } from "../product";

export interface DirectManufacturerResponse
{
	manufacturerID: number;
	manufacturerName: string;
	products: StaticProductResponse[];
}
