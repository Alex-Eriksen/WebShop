import { PhotoRequest } from "../photo";

export interface ProductRequest
{
	productID: number;
	productPrice: number;
	productName: string;
	productQuantity: number;
	productDescription: string;
	manufacturerID: number;
	categoryID: number;
	releaseDate: Date;
	productTypeID: number;
	discountID: number | null;
	imageName: string | null;
	photos: PhotoRequest[];
}
