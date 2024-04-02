import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { CartService } from 'src/app/_services/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit
{
  	constructor(private authenticationService: AuthenticationService, private router: Router, private cartService: CartService) { }

  	public loggedIn: boolean = false;
	public basketCount: number = 0;
	ngOnInit(): void
	{

		this.cartService.currentBasket.subscribe(() => {
			this.basketCount = this.cartService.getProductQuantity();
		})

		this.authenticationService.OnTokenChanged.subscribe((token) =>
		{
			if (token !== '')
			{
				this.loggedIn = true;
			}
			else
			{
				this.loggedIn = false;
			}
		});
	}

	public logOut(): void
	{
		this.authenticationService.revokeToken().subscribe();
	}

	public logIn(): void
	{
		this.router.navigate([ 'login' ], { queryParams: { returnUrl: this.router.url } });
	}
}
