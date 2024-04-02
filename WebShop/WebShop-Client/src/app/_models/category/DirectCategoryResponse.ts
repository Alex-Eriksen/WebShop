import { StaticProductResponse } from "../product";

export interface DirectCategoryResponse
{
	categoryID: number;
	categoryName: string;
	products: StaticProductResponse[];
}
