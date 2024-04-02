import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DirectCategoryResponse } from 'src/app/_models/category';
import { DirectProductResponse, StaticProductResponse } from 'src/app/_models/product';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit
{
  	products: StaticProductResponse[] = [];
  	category: DirectCategoryResponse = { categoryID: 0, categoryName: "", products: [] }
  	constructor(private productService: ProductService, private route: ActivatedRoute) { }


  	ngOnInit(): void {
    	this.route.params.subscribe(params => {
			this.productService.GetCategoryById(params['id']).subscribe(data => this.category = data);
      		this.productService.GetAllProducts().subscribe(products => {
        		products.forEach(item => {
        	  		if(item.categoryID == params[ 'id' ]) {
        	   			this.products.push(item);
        	  		}
        		});
      		})
    	})
  	}
}
