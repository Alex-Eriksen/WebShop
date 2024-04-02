import { StaticCategoryResponse } from "../category";
import { StaticDiscountResponse } from "../discount";
import { StaticManufacturerResponse } from "../Manufacturer";
import { StaticPhotoResponse } from "../photo";
import { StaticProductTypeResponse } from "../productType";
import { StaticTransactionResponse } from "../transaction";

export interface DirectProductResponse
{
	productID: number;
	productName: string;
	productPrice: number;
	productQuantity: number;
	productDescription: string;
	manufacturer: StaticManufacturerResponse;
	category: StaticCategoryResponse;
	releaseDate: string;
	productType: StaticProductTypeResponse;
	discount: StaticDiscountResponse | null;
	transactions: StaticTransactionResponse[];
	photos: StaticPhotoResponse[];
}
