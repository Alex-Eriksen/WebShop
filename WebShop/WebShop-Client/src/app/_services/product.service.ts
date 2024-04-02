import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment} from '../../environments/environment'
import { ProductRequest, DirectProductResponse, StaticProductResponse } from '../_models/product/index';
import { CategoryRequest, DirectCategoryResponse, StaticCategoryResponse } from '../_models/category';
import { ProductTypeRequest, DirectProductTypeResponse, StaticProductTypeResponse } from '../_models/productType';
import { DirectManufacturerResponse, ManufacturerRequest, StaticManufacturerResponse } from '../_models/Manufacturer';

@Injectable({
  providedIn: 'root'
})

export class ProductService 
{
	// API URL
	private url: string = environment.ApiUrl + '/product/';
	private categoryUrl: string = this.url + 'category/';
	private productTypUrl: string = this.url + 'type/';
	private manufacturerUrl: string = this.url + 'manufacturer/';
	private photoUrl: string = this.url + 'photo';

 	constructor(private httpClient: HttpClient) { }

	//#region PRODUCT CRUD

	public GetAllProducts(): Observable<StaticProductResponse[]>
	{
		return this.httpClient.get<StaticProductResponse[]>(this.url);
 	}

	public GetProductById(productId: number): Observable<DirectProductResponse>
	{
		return this.httpClient.get<DirectProductResponse>(this.url + productId);
 	}

	public CreateProduct(productRequest: ProductRequest): Observable<DirectProductResponse>
	{
		return this.httpClient.post<DirectProductResponse>(this.url, productRequest, { withCredentials: true });
 	}

	public UpdateProduct(productId: number, productRequest: ProductRequest): Observable<DirectProductResponse>
	{
		return this.httpClient.put<DirectProductResponse>(this.url + productId, productRequest, { withCredentials: true });
 	}

	public DeleteProduct(productId: number): Observable<DirectProductResponse>
	{
		return this.httpClient.delete<DirectProductResponse>(this.url + productId, { withCredentials: true });
	}

	//#endregion

	//#region CATEGORY CRUD

	public GetAllCategories(): Observable<StaticCategoryResponse[]> 
	{
		return this.httpClient.get<StaticCategoryResponse[]>(this.categoryUrl);
 	}

 	public GetCategoryById(categoryId: number): Observable<DirectCategoryResponse> 
	{
		return this.httpClient.get<DirectCategoryResponse>(this.categoryUrl + categoryId);
 	}

 	public CreateCategory(categoryRequest: CategoryRequest): Observable<DirectCategoryResponse> 
	{
		return this.httpClient.post<DirectCategoryResponse>(this.categoryUrl, categoryRequest);
 	}

 	public UpdateCategory(categoryId: number, categoryRequest: CategoryRequest): Observable<DirectCategoryResponse> 
	{
		return this.httpClient.post<DirectCategoryResponse>(this.categoryUrl + categoryId, categoryRequest);
 	}

	public DeleteCategory(categoryId: number): Observable<DirectCategoryResponse> 
	{
		return this.httpClient.delete<DirectCategoryResponse>(this.categoryUrl + categoryId);
	}

	//#endregion

	//#region TYPE CRUD

	public GetAllTypes(): Observable<StaticProductTypeResponse[]> 
	{
		return this.httpClient.get<StaticProductTypeResponse[]>(this.productTypUrl);
 	}

 	public GetTypeById(productTypeId: number): Observable<DirectProductTypeResponse> 
	{
		return this.httpClient.get<DirectProductTypeResponse>(this.productTypUrl + productTypeId);
 	}

 	public CreateType(productTypeRequest: ProductTypeRequest): Observable<DirectProductTypeResponse> 
	{
		return this.httpClient.post<DirectProductTypeResponse>(this.productTypUrl, productTypeRequest);
 	}

 	public UpdateType(productTypeId: number, productTypeRequest: ProductTypeRequest): Observable<DirectProductTypeResponse> 
	{
		return this.httpClient.post<DirectProductTypeResponse>(this.productTypUrl + productTypeId, productTypeRequest);
 	}

	public DeleteType(productTypeId: number): Observable<DirectProductTypeResponse> 
	{
		return this.httpClient.delete<DirectProductTypeResponse>(this.productTypUrl + productTypeId);
	}

	//#endregion

	//#region MANUFACTURER CRUD

	public GetAllManufacturers(): Observable<StaticManufacturerResponse[]> 
	{
		return this.httpClient.get<StaticManufacturerResponse[]>(this.manufacturerUrl);
 	}

 	public GetManufacturerById(manufacturerId: number): Observable<DirectManufacturerResponse> 
	{
		return this.httpClient.get<DirectManufacturerResponse>(this.manufacturerUrl + manufacturerId);
 	}

 	public CreateManufacturer(manufacturerRequst: ManufacturerRequest): Observable<DirectManufacturerResponse> 
	{
		return this.httpClient.post<DirectManufacturerResponse>(this.manufacturerUrl, manufacturerRequst);
 	}

 	public UpdateManufacturer(manufacturerId: number, manufacturerRequst: ManufacturerRequest): Observable<DirectManufacturerResponse> 
	{
		return this.httpClient.post<DirectManufacturerResponse>(this.manufacturerUrl + manufacturerId, manufacturerRequst);
 	}

	public DeleteManufacturer(manufacturerId: number): Observable<DirectManufacturerResponse> 
	{
		return this.httpClient.delete<DirectManufacturerResponse>(this.manufacturerUrl + manufacturerId);
	}

	//#endregion

	//#region PHOTO

	public deletePhoto(photoId: number): Observable<any>
	{
		return this.httpClient.delete<any>(`${this.photoUrl}/${photoId}`);
	}

	public createPhoto(productId: number, request: FormData): Observable<any>
	{
		return this.httpClient.post<any>(`${this.photoUrl}/${productId}`, request);
	}
	//#endregion
}
