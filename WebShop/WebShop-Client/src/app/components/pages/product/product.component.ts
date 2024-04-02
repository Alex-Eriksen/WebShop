import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StaticCategoryResponse } from 'src/app/_models/category';
import { DirectProductResponse } from 'src/app/_models/product';
import { CartItem } from 'src/app/_models/_misc/cartItem';
import { CartService } from 'src/app/_services/cart.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})

export class ProductComponent implements OnInit
{
	currentProduct: DirectProductResponse =
	{
		productID: 0,
		photos: [],
		productDescription: '',
		productName: '',
		productPrice: 0,
		productQuantity: 0,
		releaseDate: "",
		category:
		{
			categoryID: 0,
			categoryName: "",
			productCount: 0
		},
		manufacturer:
		{
			manufacturerID: 0,
			manufacturerName: ""
		},
		productType:
		{
			productTypeID: 0,
			productTypeName: ""
		},
		discount: null,
		transactions: []
	}

	cartItem: CartItem = {
		productId: 0,
		title: '',
		quantity: 0,
		maxQuantity: 0,
		price: 0,
		discountID: 0,
		discountAmount: 0,
		imageLocation: ''
	}

	categories: StaticCategoryResponse[] = [];

  	constructor(private route: ActivatedRoute, private productService: ProductService, private cartService: CartService, private router: Router) { }

	ngOnInit(): void
	{
		this.route.params.subscribe(params =>
		{
			this.productService.GetProductById(params[ 'id' ]).subscribe(data =>
			{
				this.currentProduct = data;
				this.cartItem = {
					productId: this.currentProduct.productID,
					title: this.currentProduct.productName,
					quantity: 1,
					maxQuantity: this.currentProduct.productQuantity,
					price: this.currentProduct.productPrice,
					discountID: this.currentProduct.discount!.discountID,
					discountAmount: this.currentProduct.discount!.discountPercent,
					imageLocation: this.currentProduct.photos[0].imageName,
				}
				console.log(this.cartItem);
			});
		});
	}

	addItem(): void {
		this.cartService.addItemToBasket(this.cartItem);
		console.log(this.cartItem);
		this.router.navigate(["cart"]);
  }
}
