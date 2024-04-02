import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CartItem } from '../_models/_misc/cartItem';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class CartService
{
	private basketName = "webshopProjectBasket";

	currentBasketSubject: BehaviorSubject<CartItem[]>;
	currentBasket: Observable<CartItem[]>;

  constructor(private notification: NotificationService)
	{
		this.currentBasketSubject = new BehaviorSubject<CartItem[]>(JSON.parse(localStorage.getItem(this.basketName) || "[]"));
		this.currentBasket = this.currentBasketSubject.asObservable();
	}

	get currentBasketValue(): CartItem[]
	{
		return this.currentBasketSubject.value;
	}

	saveBasket(basket: CartItem[])
	{
		localStorage.setItem(this.basketName, JSON.stringify(basket));
		this.currentBasketSubject.next(basket);
	}

	addItemToBasket(item: CartItem): void
	{
		let productFound = false;
		let basket = this.currentBasketValue;
		basket.forEach(basketItem =>
		{
			if(basketItem.productId == item.productId)
			{
				if((basketItem.quantity) != item.maxQuantity) {
					basketItem.quantity += item.quantity;
				}
				productFound = true;
				if(basketItem.quantity <= 0)
				{
					this.removeItemFromBasket(item.productId);
				}
			}
		});

		if(!productFound)
		{
			this.notification.showInfo("Successfully added item to cart", "Added item to cart!")
			basket.push(item);
		}
		this.saveBasket(basket);
	}

	removeItemFromBasket(productId: number)
	{
		let basket = this.currentBasketValue;
		for(let item of basket)
		{
			basket = basket.filter((product) =>  {
				return product.productId !== productId});

			this.notification.showInfo(`${item.title} removed from cart!`,"Item removed!");
			this.saveBasket(basket);
		}
	}

	clearBasket(): void
	{
		// Saves empty array
		this.saveBasket([]);
	}

	getBasketTotal(): number
	{
		let total: number = 0;
		this.currentBasketSubject.value.forEach(item =>
		{
			total += item.price * item.quantity;
		})
		return total;
	}

	updateItemInBasket(cartItem: CartItem) {
		let basket = this.currentBasketValue;
		basket.forEach(basketItem => {
			if(basketItem.productId == cartItem.productId) {
				basketItem.quantity = cartItem.quantity;
				if(basketItem.quantity <= 0) {
					this.removeItemFromBasket(cartItem.productId);
				}
			}
		});

		this.saveBasket(basket);
	}

	getProductQuantity(): number {
		let basketValue = this.currentBasketValue;
		let quantity = 0;
		for(let item of basketValue) {
			quantity += item.quantity;
		}
		return quantity;
	}
}
