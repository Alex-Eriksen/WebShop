import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})

export class AdminComponent implements OnInit
{
  constructor(private router: Router) { }

	ngOnInit(): void { }

	public route(selector: number): void
	{
		switch (selector)
		{
			case 0:
				this.router.navigate([ 'admin/customer' ]);
				return;
			case 1:
				this.router.navigate([ 'admin/product' ]);
				return;
			case 2:
				this.router.navigate([ 'admin/discount' ]);
				return;
			case 3:
				this.router.navigate([ 'admin/category' ]);
				return;
			case 4:
				this.router.navigate([ 'admin/order' ]);
		}
	}
}
