import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StaticOrderResponse } from '../_models/order';
import { DirectOrderResponse } from '../_models/order/DirectOrderResponse';
import { OrderRequest } from '../_models/order/OrderRequest';
import { DirectTransactionResponse, StaticTransactionResponse, TransactionRequest } from '../_models/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

	// API URL
	private url: string = environment.ApiUrl + '/transaction/';
	private transactionUrl: string = this.url + 'transaction';
	private orderUrl: string = this.url + 'order';


  	constructor(private http: HttpClient) { }

	public GetAllTransaction(): Observable<StaticTransactionResponse[]> {
		return this.http.get<StaticTransactionResponse[]>(this.transactionUrl);
	}

	public GetTransaction(transactionId: number): Observable<DirectTransactionResponse> {
		return this.http.get<DirectTransactionResponse>(`${this.transactionUrl}/${transactionId}`);
	}

	public CreateTransaction(request: TransactionRequest): Observable<DirectTransactionResponse> {
		return this.http.post<DirectTransactionResponse>(this.transactionUrl, request);
	}

	public UpdateTransaction(transactionId: number, request: TransactionRequest): Observable<DirectTransactionResponse> {
		return this.http.put<DirectTransactionResponse>(`${this.transactionUrl}/${transactionId}`, request);
	}

	public GetAllOrders(): Observable<StaticOrderResponse[]> {
		return this.http.get<StaticOrderResponse[]>(this.orderUrl);
	}

	public GetOrder(orderId: number): Observable<DirectOrderResponse> {
		return this.http.get<DirectOrderResponse>(`${this.orderUrl}/${orderId}`);
	}

	public CreateOrder(request: OrderRequest): Observable<DirectOrderResponse> {
		return this.http.post<DirectOrderResponse>(this.orderUrl, request);
	}

	public UpdateOrder(orderId: number, request: OrderRequest): Observable<DirectOrderResponse> {
		return this.http.put<DirectOrderResponse>(`${this.orderUrl}/${orderId}`, request);
	}
}
