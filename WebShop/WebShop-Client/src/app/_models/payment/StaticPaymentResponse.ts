export interface StaticPaymentResponse
{
	paymentID: number;
	customerID: number;
	paymentType: string;
	cardNumber: string;
	provider: string;
	expiry: Date;
	created_At: Date;
}
