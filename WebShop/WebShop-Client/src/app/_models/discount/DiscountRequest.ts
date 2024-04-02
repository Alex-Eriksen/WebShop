export interface DiscountRequest
{
	discountID: number;
	name: string;
	description: string;
	discountPercent: number;
	revoked_At: Date;
}
