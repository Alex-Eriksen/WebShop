export interface PaymentRequest
{
	customerID: number;
	paymentType: string;
	cardNumber: string;
	provider: string;
	expiry: Date;
}
