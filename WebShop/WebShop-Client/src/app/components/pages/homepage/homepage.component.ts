import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StaticCategoryResponse } from 'src/app/_models/category';
import { StaticProductResponse } from 'src/app/_models/product';
import { DiscountService } from 'src/app/_services/discount.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})

export class HomepageComponent implements OnInit 
{
	categories: StaticCategoryResponse[] = [];

	itemsOnDiscount: StaticProductResponse[] = [];
	discounts: { [ key: number ]: number; } = {};

  constructor(private productService: ProductService, private discountService: DiscountService, private router: Router) { }

	ngOnInit(): void
	{
		this.getDiscountItems();
  }

	public getDiscountItems(): void
	{
		this.discountService.getAll().subscribe(discounts =>
		{
			for (let discount of discounts)
			{
				this.discountService.getById(discount.discountID).subscribe(data =>
				{
					for (let product of data.products)
					{
						this.itemsOnDiscount.push(product);
						this.discounts[product.productID] = discount.discountPercent;
					}
				});;
			}
		});
	}

	public goToProduct(productId: number): void
	{
		this.router.navigate([ `product/${productId}` ]);
	}
}
