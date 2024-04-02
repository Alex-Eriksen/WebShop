import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountRequest } from 'src/app/_models/account';
import { CustomerRequest } from 'src/app/_models/customer';
import { NewCustomerRequest } from 'src/app/_models/_misc/NewCustomerRequest';
import { Pair } from 'src/app/_models/_misc/pair';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { CustomerService } from 'src/app/_services/customer.service';
import { NotificationService } from 'src/app/_services/notification.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit
{
	private returnUrl: string = "";
	public accountRequest: AccountRequest = { email: '', password: '', role: 'Customer', username: '' };
	public passwordValidator: string = '';
	public customerRequest: CustomerRequest = { customerID:0, accountID: 0, countryID: 0, firstName: '', lastName: '', gender: '', phoneNumber: '', zipCode: 0 };
	private request: NewCustomerRequest = { account: this.accountRequest, customer: this.customerRequest };
	public isValid: boolean = false;

	public genders: Pair[] = [
		{ key: "Mand", value: "Male" },
		{ key: "Kvinde", value: "Female" }
	];

	public countries: Pair[] = [
		{ key: "Danmark", value: 1 }
	];

	constructor(
		private authenticationService: AuthenticationService,
		private router: Router,
		private route: ActivatedRoute,
		private customerService: CustomerService,
		private notification: NotificationService) { }

	ngOnInit(): void
	{
		this.returnUrl = this.route.snapshot.queryParams[ "returnUrl" ] || "/";
		this.authenticationService.OnTokenChanged.subscribe((token) =>
		{
			if (token !== "")
			{
				this.router.navigate([ this.returnUrl ]);
			}
		});
	}

	public validate(value: boolean): void
	{
		for (let prop of Object.values(this.accountRequest))
		{
			if (typeof (prop[1]) === 'string')
			{
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

		for (let prop of Object.entries(this.customerRequest))
		{
			if (typeof (prop[1]) === 'string')
			{
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

		if (this.accountRequest.password != this.passwordValidator || this.accountRequest.password === '' || this.passwordValidator === '')
		{
			this.isValid = false;
			return;
		}

		this.isValid = true;
	}

	public submit(): void
	{
		if (this.isValid)
		{
			this.customerService.create(this.request).subscribe
			({
				next: () =>
				{
					this.router.navigate([ this.returnUrl ]);
				},
				error: (err) =>
				{
					console.error(Object.values(err.error.errors).join(', '));
					this.notification.showError(Object.values(err.error.errors).join(', '), "Something went wrong!");
				}
			});
		}
	}
}
