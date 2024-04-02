import { Component, OnInit } from '@angular/core';
import { CategoryRequest, StaticCategoryResponse } from 'src/app/_models/category';
import { ManufacturerRequest, StaticManufacturerResponse } from 'src/app/_models/Manufacturer';
import { ProductTypeRequest, StaticProductTypeResponse } from 'src/app/_models/productType';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class AdminCategoryComponent implements OnInit
{
	public categories: StaticCategoryResponse[] = [];
	public productTypes: StaticProductTypeResponse[] = [];
	public manufacturers: StaticManufacturerResponse[] = [];
	public category: CategoryRequest = { categoryID: 0, categoryName: '' };
	public productType: ProductTypeRequest = { productTypeID: 0, productTypeName: '' };
	public manufacturer: ManufacturerRequest = { manufacturerID: 0, manufacturerName: '' };

  constructor(private productService: ProductService) { }

	ngOnInit(): void
	{
		this.productService.GetAllCategories().subscribe(data => this.categories = data);
		this.productService.GetAllTypes().subscribe(data => this.productTypes = data);
		this.productService.GetAllManufacturers().subscribe(data => this.manufacturers = data);
	}

	public saveCategory(): void
	{
		if (this.category.categoryID === 0)
		{
			// Create new category
			this.productService.CreateCategory(this.category).subscribe
			({
				next: (response) =>
				{
					this.categories.push({ categoryID: response.categoryID, categoryName: response.categoryName, productCount: response.products.length });
					this.cancelCategory();
				},
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				}
			});
		}
		else
		{
			// Update old category
			this.productService.UpdateCategory(this.category.categoryID, this.category).subscribe({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.cancelCategory();
				}
			});
		}
	}

	public saveProductType(): void
	{
		if (this.productType.productTypeID === 0)
		{
			// Create new product type
			this.productService.CreateType(this.productType).subscribe
			({
				next: (response) =>
				{
					this.productTypes.push({
						productTypeID: response.productTypeID,
						productTypeName: response.productTypeName
					});
					this.cancelProductType();
				},
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				}
			});
		}
		else
		{
			// Update old product type
			this.productService.UpdateType(this.productType.productTypeID, this.productType).subscribe({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.cancelProductType();
				}
			});
		}
	}

	public cancelCategory(): void
	{
		this.category = { categoryID: 0, categoryName: '' };
	}

	public cancelProductType(): void
	{
		this.productType = { productTypeID: 0, productTypeName: '' };
	}

	public editCategory(category: StaticCategoryResponse): void
	{
		this.category = 
		{
			categoryID: category.categoryID,
			categoryName: category.categoryName
		}
	}

	public editProductType(productType: StaticProductTypeResponse): void
	{
		this.productType = 
		{
			productTypeID: productType.productTypeID,
			productTypeName: productType.productTypeName
		}
	}

	public deleteCategory(category: StaticCategoryResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${category.categoryName}?`))
		{
			this.productService.DeleteCategory(category.categoryID).subscribe
			({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.productService.GetAllCategories().subscribe(data => this.categories = data);
				}
			});
		}
	}

	public deleteProductType(productType: StaticProductTypeResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${productType.productTypeName}?`))
		{
			this.productService.DeleteType(productType.productTypeID).subscribe
			({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.productService.GetAllCategories().subscribe(data => this.categories = data);
				}
			});
		}
	}

	public saveManufacturer(): void
	{
		if (this.manufacturer.manufacturerID === 0)
		{
			// Create new product type
			this.productService.CreateManufacturer(this.manufacturer).subscribe
			({
				next: (response) =>
				{
					this.manufacturers.push
					({
						manufacturerID: response.manufacturerID,
						manufacturerName: response.manufacturerName
					});
					this.cancelManufacturer();
				},
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				}
			});
		}
		else
		{
			// Update old product type
			this.productService.UpdateManufacturer(this.manufacturer.manufacturerID, this.manufacturer).subscribe
			({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.cancelManufacturer();
				}
			});
		}
	}

	public editManufacturer(manufacturer: StaticManufacturerResponse): void
	{
		this.manufacturer = 
		{
			manufacturerID: manufacturer.manufacturerID,
			manufacturerName: manufacturer.manufacturerName
		};
	}

	public deleteManufacturer(manufacturer: StaticManufacturerResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${manufacturer.manufacturerName}?`))
		{
			this.productService.DeleteManufacturer(manufacturer.manufacturerID).subscribe
			({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.productService.GetAllManufacturers().subscribe(data => this.manufacturers = data);
				}
			});
		}
	}

	public cancelManufacturer(): void
	{
		this.manufacturer = { manufacturerID: 0, manufacturerName: '' };
	}
}
