import { Component, OnInit } from '@angular/core';
import { CustomerRequest, DirectCustomerResponse } from '../../../_models/customer'
import { CustomerService } from 'src/app/_services/customer.service';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { JwtDecodePlus } from 'src/app/helpers/JWTDecodePlus';
import { AccountRequest } from 'src/app/_models/account';
import { NewCustomerRequest } from 'src/app/_models/_misc/NewCustomerRequest';
import { Router } from '@angular/router';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})

export class ProfileComponent implements OnInit
{
	private customerID: number = 0;
	public allowEdit: boolean = false;
	public isAdmin: boolean = false;

  	customer: DirectCustomerResponse =
		{
    	account:
			{
    	  accountID: 0,
    	  email: "",
    	  role: "",
    	  username: "",
    	},
    	country:
			{
    	  countryID: 0,
    	  countryName: ""
    	},
    	created_At: new Date(),
		customerID: 0,
		firstName: "",
		gender: "",
		lastName: "",
		orders: [],
		payments: [],
		phoneNumber: "",
		zipCode: 0,
  	};

	customerRequest: CustomerRequest =
	{
		customerID: 0,
		accountID: 0,
		firstName: '',
		lastName: '',
		phoneNumber: '',
		countryID: 0,
		zipCode: 0,
		gender: ''
	};

  	constructor(private customerService: CustomerService, private authService: AuthenticationService, private router: Router) { }

	ngOnInit(): void
	{
		this.authService.OnTokenChanged.subscribe((token) =>
		{
			this.customerID = JwtDecodePlus.jwtDecode(token).nameid;
			this.isAdmin = JwtDecodePlus.jwtDecode(token).role === 'Admin' ? true : false;
			this.customerService.getById(this.customerID).subscribe(data => this.customer = data);
		});
  	}

	public save(): void
	{
		if (confirm('Er du sikker på at du vil gemme oplysningerne?'))
		{
			this.customerRequest =
			{
				customerID: this.customer.customerID,
				accountID: this.customer.account.accountID,
				firstName: this.customer.firstName,
				lastName: this.customer.lastName,
				phoneNumber: this.customer.phoneNumber,
				countryID: this.customer.country.countryID,
				zipCode: this.customer.zipCode,
				gender: this.customer.gender
			};

			let accountRequest: AccountRequest =
			{
				email: this.customer.account.email,
				role: this.customer.account.role,
				username: this.customer.account.username,
				password: 'NONE'
			};

			let request: NewCustomerRequest =
			{
				account: accountRequest,
				customer: this.customerRequest
			};

			this.customerService.update(this.customer.customerID, request).subscribe(x =>
			{
				this.customer = x;

				this.allowEdit = false;
			});
			console.log('Gemt');
		}
		else
		{
			console.log('Blev ikke gemt');
			window.location.reload();
		}
	}

	public edit(): void
	{
		if (!this.allowEdit)
		{
			this.allowEdit = true;
		}
	}

	public route(): void
	{
		this.router.navigate(['profile/changePassword'])
	}
}
