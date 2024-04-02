import { StaticProductResponse } from "../product/StaticProductResponse";

export interface DirectProductTypeResponse
{
	productTypeID: number;
	productTypeName: string;
	products: StaticProductResponse[];
}
