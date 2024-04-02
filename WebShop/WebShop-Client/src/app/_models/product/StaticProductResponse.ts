export interface StaticProductResponse
{
    productID: number;
    productName: string;
    productPrice: number;
    productQuantity: number;
    productDescription: string;
    manufacturerID: number;
    categoryID: number;
    releaseDate: Date;
    productTypeID: number;
	discountID: number | null;
	imageName: string | null;
}
