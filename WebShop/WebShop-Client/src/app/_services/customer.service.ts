import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CustomerRequest, DirectCustomerResponse, StaticCustomerResponse } from '../_models/customer';
import { DirectPaymentResponse, PaymentRequest } from '../_models/payment';
import { NewCustomerRequest } from '../_models/_misc/NewCustomerRequest';

@Injectable({
  providedIn: 'root'
})
export class CustomerService
{
	constructor(private http: HttpClient) { }

	private get customerUrl(): string
	{
		return environment.ApiUrl + "/Customer";
	}

	private get paymentUrl(): string
	{
		return this.customerUrl + "/payment";
	}

	public getAll(): Observable<StaticCustomerResponse[]>
	{
		return this.http.get<StaticCustomerResponse[]>(this.customerUrl);
	}

	public create(request: NewCustomerRequest): Observable<DirectCustomerResponse>
	{
		return this.http.post<DirectCustomerResponse>(this.customerUrl, request);
	}

	public getById(customerId: number): Observable<DirectCustomerResponse>
	{
		return this.http.get<DirectCustomerResponse>(`${this.customerUrl}/${customerId}`);
	}

	public update(customerId: number, request: NewCustomerRequest): Observable<DirectCustomerResponse>
	{
		return this.http.put<DirectCustomerResponse>(`${this.customerUrl}/${customerId}`, request);
	}

	public delete(customerId: number): Observable<DirectCustomerResponse>
	{
		return this.http.delete<DirectCustomerResponse>(`${this.customerUrl}/${customerId}`);
	}

	public createPayment(request: PaymentRequest): Observable<DirectPaymentResponse>
	{
		return this.http.post<DirectPaymentResponse>(this.paymentUrl, request);
	}

	public updatePayment(paymentId: number, request: PaymentRequest): Observable<DirectPaymentResponse>
	{
		return this.http.put<DirectPaymentResponse>(`${this.paymentUrl}/${paymentId}`, request);
	}

	public deletePayment(paymentId: number): Observable<DirectPaymentResponse>
	{
		return this.http.delete<DirectPaymentResponse>(`${this.paymentUrl}/${paymentId}`);
	}

	public getPayment(paymentId: number): Observable<DirectPaymentResponse>
	{
		return this.http.get<DirectPaymentResponse>(`${this.paymentUrl}/${paymentId}`);
	}
}
