import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DirectDiscountResponse, DiscountRequest, StaticDiscountResponse } from '../_models/discount';

@Injectable({
  providedIn: 'root'
})
export class DiscountService
{
	private url: string = environment.ApiUrl + "/Discount";

	constructor(private http: HttpClient) { }

	public getAll(): Observable<StaticDiscountResponse[]>
	{
		return this.http.get<StaticDiscountResponse[]>(this.url);
	}

	public create(request: DiscountRequest): Observable<DirectDiscountResponse>
	{
		return this.http.post<DirectDiscountResponse>(this.url, request);
	}

	public getById(discountId: number): Observable<DirectDiscountResponse>
	{
		return this.http.get<DirectDiscountResponse>(`${this.url}/${discountId}`);
	}

	public update(discountId: number, request: DiscountRequest): Observable<DirectDiscountResponse>
	{
		return this.http.put<DirectDiscountResponse>(`${this.url}/${discountId}`, request);
	}

	public delete(discountId: number): Observable<DirectDiscountResponse>
	{
		return this.http.delete<DirectDiscountResponse>(`${this.url}/${discountId}`);
	}
}
