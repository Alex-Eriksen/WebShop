import { Component, OnInit } from '@angular/core';
import { StaticCategoryResponse } from 'src/app/_models/category';
import { StaticDiscountResponse } from 'src/app/_models/discount';
import { StaticManufacturerResponse } from 'src/app/_models/Manufacturer';
import { PhotoRequest, StaticPhotoResponse } from 'src/app/_models/photo';
import { ProductRequest, StaticProductResponse } from 'src/app/_models/product';
import { StaticProductTypeResponse } from 'src/app/_models/productType';
import { Pair } from 'src/app/_models/_misc/pair';
import { DiscountService } from 'src/app/_services/discount.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})

export class ProductAdminComponent implements OnInit
{
	public products: StaticProductResponse[] = [];
	public categories: Pair[] = [];
	public productTypes: Pair[] = [];
	public discounts: Pair[] = [];
	public manufacturers: Pair[] = [];
	public photos: StaticPhotoResponse[] = [];
	public currentProduct: ProductRequest =
	{
		productID: 0,
		photos: [],
		productDescription: '',
		productName: '',
		productPrice: 0,
		productQuantity: 0,
		releaseDate: new Date(),
		categoryID: 0,
		manufacturerID: 0,
		productTypeID: 0,
		discountID: null,
		imageName: ""
	};

	public photo: PhotoRequest = { photoID: 0, productID: 0 };

	public isValid: boolean = false;

	constructor(private productService: ProductService, private discountService: DiscountService) { }

	ngOnInit(): void
	{
		this.productService.GetAllProducts().subscribe(data => this.products = data);
		this.discountService.getAll().subscribe(data =>
		{
			this.discounts = [];
			for (let response of data)
			{
				this.discounts.push({ key: response.name, value: response.discountID });
			}
		});
	}

	public deletePhoto(photo: StaticPhotoResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${photo.imageName}?`))
		{
			this.productService.deletePhoto(photo.photoID).subscribe(data => this.photos = this.photos.filter(x => x.photoID != data.photoID));
		}
	}

	public edit(productID: number): void
	{
		new Promise<void>((resolve) =>
		{
			new Promise<void>((res) =>
			{
				this.productService.GetAllCategories().subscribe(data =>
				{
					this.categories = [];
					for (let response of data)
					{
						this.categories.push({ key: response.categoryName, value: String(response.categoryID) });
					}
					res();
				});
			}).then(() =>
			{
				this.productService.GetAllTypes().subscribe(data =>
				{
					this.productTypes = [];
					for (let response of data)
					{
						this.productTypes.push({ key: response.productTypeName, value: response.productTypeID });
					}
				});
			}).then(() =>
			{
				this.productService.GetAllManufacturers().subscribe(data =>
				{
					this.manufacturers = [];
					for (let response of data)
					{
						this.manufacturers.push({ key: response.manufacturerName, value: response.manufacturerID });
					}
				});
			}).finally(() =>
			{
				resolve();
			});
		}).then(() =>
		{
			this.productService.GetProductById(productID).subscribe(data =>
			{
				this.currentProduct.categoryID = data.category.categoryID;
				this.currentProduct.manufacturerID = data.manufacturer.manufacturerID;
				this.currentProduct.productDescription = data.productDescription;
				this.currentProduct.productName = data.productName;
				this.currentProduct.productPrice = data.productPrice;
				this.currentProduct.productQuantity = data.productQuantity;
				this.currentProduct.releaseDate = new Date(data.releaseDate);
				this.currentProduct.productTypeID = data.productType.productTypeID;
				this.currentProduct.photos = data.photos;
				this.currentProduct.productID = data.productID;
				this.photos = data.photos;
				if (data.discount != null)
				{
					this.currentProduct.discountID = data.discount.discountID;
				}
			});
		});
	}

	public create(): void
	{
		this.productService.GetAllCategories().subscribe(data =>
		{
			this.categories = [];
			for (let response of data)
			{
				this.categories.push({ key: response.categoryName, value: response.categoryID });
			}
		});
		this.productService.GetAllTypes().subscribe(data =>
		{
			this.productTypes = [];
			for (let response of data)
			{
				this.productTypes.push({ key: response.productTypeName, value: response.productTypeID });
			}
		});
		this.productService.GetAllManufacturers().subscribe(data =>
		{
			this.manufacturers = [];
			for (let response of data)
			{
				this.manufacturers.push({ key: response.manufacturerName, value: response.manufacturerID });
			}
		});
		this.currentProduct.productID = -1;
		this.photos = [];
	}

	public createPhoto(): void
	{
		this.photo.photoID = -1;
	}

	public save(files: FileList | null): void
	{
		if (this.isValid == false) return;

		if (this.currentProduct.productID == -1)
		{
			this.productService.CreateProduct(this.currentProduct).subscribe
			({
				next: (data) =>
				{
					this.currentProduct.productID = data.productID;
					this.products.push(this.currentProduct);
					if (files != null)
					{
						if (files.length === 0)
						{
							return;
						}

						const formData: FormData = new FormData();
						for (let i = 0; i < files.length; i++)
						{
							let fileToUpload: File = files[ i ];
							formData.append('imageFile', fileToUpload, fileToUpload.name);
						}

						this.productService.createPhoto(this.currentProduct.productID, formData).subscribe();
					}
					this.cancel();
				},
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				}
			});
		}
		else
		{
			this.productService.UpdateProduct(this.currentProduct.productID, this.currentProduct).subscribe
			({
				next: () =>
				{
					this.products = [];
					this.productService.GetAllProducts().subscribe(data => this.products = data);
					if (files != null)
					{
						if (files.length !== 0)
						{
							const formData: FormData = new FormData();
							for (let i = 0; i < files.length; i++)
							{
								let fileToUpload: File = files[ i ];
								formData.append('imageFile', fileToUpload, fileToUpload.name);
							}
							this.productService.createPhoto(this.currentProduct.productID, formData).subscribe();
						}
					}
					this.cancel();
				},
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				}
			});
		}
	}

	public validate(value: boolean): void
	{
		for (let prop of Object.entries(this.currentProduct))
		{
			if (typeof (prop[1]) === 'string')
			{
				if (prop[ 0 ] == "imageName")
				{
					continue;
				}
				if (prop[1] === '')
				{
					this.isValid = false;
					console.log(`${prop[0]}: ${prop[1]}`);
					return;
				}
			}
			else if (typeof (prop[1]) === 'number')
			{
				if (prop[1] === null || prop[1] === undefined)
				{
					this.isValid = false;
					console.log(`${prop[0]}: ${prop[1]}`);
					return;
				}
			}
		}

		this.isValid = true;
	}

	public cancel(): void
	{
		this.currentProduct =
		{
			productID: 0,
			photos: [],
			productDescription: '',
			productName: '',
			productPrice: 0,
			productQuantity: 0,
			releaseDate: new Date(),
			categoryID: 0,
			manufacturerID: 0,
			productTypeID: 0,
			discountID: 0,
			imageName: ''
		};
		this.photo = { photoID: 0, productID: 0 };
	}
}
