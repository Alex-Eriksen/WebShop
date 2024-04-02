import { Component, OnInit } from '@angular/core';
import { map, Observable, pipe } from 'rxjs';
import { JwtDecodePlus } from 'src/app/helpers/JWTDecodePlus';
import { OrderRequest } from 'src/app/_models/order/OrderRequest';
import { StaticPhotoResponse } from 'src/app/_models/photo';
import { DirectProductResponse } from 'src/app/_models/product';
import { TransactionRequest } from 'src/app/_models/transaction';
import { CartItem } from 'src/app/_models/_misc/cartItem';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { CartService } from 'src/app/_services/cart.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { ProductService } from 'src/app/_services/product.service';
import { TransactionService } from 'src/app/_services/transaction.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})

export class CartComponent implements OnInit
{
	photos: StaticPhotoResponse[] = [];
	private customerID = 0;
	basketTotal: number = 0;
	amount: number = 1;
	cartItems: CartItem[] = []
	transactions: TransactionRequest[] = [];
	orderRequest: OrderRequest = { customerID: 0, orderTotal: 0 };
	transactionRequest: TransactionRequest = { productID: 0, orderID: 0, productAmount: 0, productPrice: 0, discountID: 0, created_At: new Date(), modified_At: new Date() };

	constructor(private cartService: CartService, private transactionService: TransactionService, private authService: AuthenticationService, private productService: ProductService, private notification: NotificationService) {}

	image(id: number): StaticPhotoResponse[] {
		return this.photos.filter(x => x.productID == id);
	}

  	ngOnInit(): void {
		this.cartService.currentBasket.subscribe(items =>
		{
			this.cartItems = items;
			this.cartItems.forEach(item =>
			{
				this.productService.GetProductById(item.productId).subscribe(data =>
				{
					this.photos.push(data.photos[ 0 ]);
					if (data.discount != null)
					{
						item.discountAmount = data.discount.discountPercent;
					}
				});
			});
		});
		this.basketTotal = this.cartService.getBasketTotal();
		this.authService.OnTokenChanged.subscribe(x => {
				this.customerID = JwtDecodePlus.jwtDecode(x).nameid;
		});
	}

	public onQuantityChange(item: CartItem, value: any): void
	{
		if(value.value <= item.maxQuantity) {
			this.basketTotal = this.cartService.getBasketTotal();
			this.cartService.saveBasket(this.cartItems)
			return;
		}
		this.notification.showWarning("Reached maximum quantity of item!", "Cannot exceed maximum quantity");
		this.cartItems.find(x => x.productId == item.productId)!.quantity = item.maxQuantity;
	}

	public removeItem(item: CartItem): void
	{
		if(confirm(`Er du sikker på du vil fjerne ${item.title} fra din indkøbskurv?`))
		this.cartService.removeItemFromBasket(item.productId);
		this.basketTotal = this.cartService.getBasketTotal();
	}

	public checkOut(): void
	{
		this.orderRequest = { customerID: this.customerID, orderTotal: this.basketTotal} ;

		this.transactionService.CreateOrder(this.orderRequest).subscribe({
			next: (x) => {
				this.cartItems.forEach(item => {
					this.transactionRequest = {
						productID: item.productId,
						orderID: x.orderID,
						productAmount: item.quantity,
						productPrice: item.price,
						discountID: item.discountID,
						created_At: new Date(),
						modified_At: new Date()
					};

					this.transactionService.CreateTransaction(this.transactionRequest).subscribe();
				});
			},
			error: (err) => {
				console.log(Object.values(err.error.errors).join(', '));
				this.notification.showError(Object.values(err.error.errors).join(', '), "Something went wrong!");
			},
			complete: () => {
				console.log("Successfully completed order.");
				this.notification.showSuccess("Successfully purchased products!", "Purchased Items")
				this.cartService.clearBasket();
			}
		})
	}
}
