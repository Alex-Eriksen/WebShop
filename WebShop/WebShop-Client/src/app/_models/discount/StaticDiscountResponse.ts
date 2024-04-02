export interface StaticDiscountResponse
{
	discountID: number;
	name: string;
	description: string;
	discountPercent: number;
	created_At: Date;
	modified_At: Date;
	revoked_At: Date;
}
