import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StaticProductResponse } from 'src/app/_models/product';
import { CartItem } from 'src/app/_models/_misc/cartItem';
import { CartService } from 'src/app/_services/cart.service';
import { DiscountService } from 'src/app/_services/discount.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'product-display',
  templateUrl: './product-display.component.html',
  styleUrls: ['./product-display.component.css']
})
export class ProductDisplayComponent implements OnInit
{
	@Input() product: StaticProductResponse = {
		categoryID: 0,
		discountID: null,
		imageName: '',
		manufacturerID: 0,
		productDescription: '',
		productID: 0,
		productName: '',
		productPrice: 0,
		productQuantity: 0,
		productTypeID: 0,
		releaseDate: new Date()
	};

	cartItem: CartItem = { discountID: 0, discountAmount: 0, imageLocation: "", maxQuantity: 0, price: 0, productId: 0, quantity: 0, title: ""}

	public discountAmount: number = 0;
	public discountPrice: number = 0;

  	constructor(private discountService: DiscountService, private cartService: CartService, private productService: ProductService, private router: Router) { }
	ngOnInit(): void
	{
		if (this.product.discountID != null)
		{
			this.discountService.getById(this.product.discountID).subscribe(data => this.discountAmount = data.discountPercent);
		}
		this.cartItem = {
			productId: this.product.productID,
			title: this.product.productName,
			quantity: 1,
			maxQuantity: this.product.productQuantity,
			price: this.product.productPrice,
			discountID: this.product.discountID,
			discountAmount: this.discountAmount,
			imageLocation: "http://localhost:5001/api/product/photo/"+this.product.imageName
		}
	}

	goToProduct() {
		this.router.navigate(["product/"+this.product.productID])
	}

	addToCart() {
		console.log(this.cartItem);
		this.cartService.addItemToBasket(this.cartItem);
	}
}
