import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StaticCategoryResponse } from 'src/app/_models/category';
import { ProductService } from 'src/app/_services/product.service';

@Component({
	selector: 'navbar',
  	templateUrl: './navbar.component.html',
  	styleUrls: ['./navbar.component.css']
})

export class NavbarComponent implements OnInit 
{

	categories: StaticCategoryResponse[] = [];
  constructor(private productService: ProductService, private router: Router) { }

  ngOnInit(): void 
	{
	this.getCategoryItems();
  }

  getCategoryItems(): void 
	{
		this.productService.GetAllCategories().subscribe
		({
			next: (x) => 
			{
				this.categories = x;
			},
			error: (err) => 
			{
				console.log(Object.values(err.error.errors).join(', '));
			},
			complete: () => 
			{
				console.log("Successfully fetched categories");
			}
		})
  }
}
