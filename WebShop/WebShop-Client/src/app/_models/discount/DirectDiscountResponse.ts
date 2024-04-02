import { StaticProductResponse } from "../product";
import { StaticTransactionResponse } from "../transaction";

export interface DirectDiscountResponse
{
	discountID: number;
	name: string;
	description: string;
	discountPercent: number;
	created_At: Date;
	modified_At: Date;
	revoked_At: Date;
	products: StaticProductResponse[];
	transactions: StaticTransactionResponse[];
}
