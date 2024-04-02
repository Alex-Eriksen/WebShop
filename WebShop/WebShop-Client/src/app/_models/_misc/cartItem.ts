export interface CartItem
{
	productId: number;
	title: string;
	quantity: number;
	maxQuantity: number
	price: number;
	discountID: number | null;
	discountAmount: number | null;
	imageLocation: string;
}
