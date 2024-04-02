import { Component, OnInit } from '@angular/core';
import { AccountRequest } from 'src/app/_models/account';
import { StaticCountryResponse } from 'src/app/_models/country';
import { CustomerRequest, StaticCustomerResponse } from 'src/app/_models/customer';
import { NewCustomerRequest } from 'src/app/_models/_misc/NewCustomerRequest';
import { Pair } from 'src/app/_models/_misc/pair';
import { CustomerService } from 'src/app/_services/customer.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})

export class CustomerComponent implements OnInit
{
	public countrys: StaticCountryResponse[] = [];
	public customers: StaticCustomerResponse[] = [];
	public account: AccountRequest =
	{
		email: '',
		password: '',
		role: '',
		username: ''
	};

	public customer: CustomerRequest =
	{
		customerID: 0,
		accountID: 0,
		countryID: 0,
		firstName: '',
		gender: '',
		lastName: '',
		phoneNumber: '',
		zipCode: 0
	};

	public roles: Pair[] = [
		{ key: 'Administrator', value: 'Admin' },
		{ key: 'Kunde', value: 'Customer' }
	];

	public genders: Pair[] = [
		{ key: 'Mand', value: 'Male' },
		{ key: 'Kvinde', value: 'Female' }
	];

	public isValid: boolean = false;

  	constructor(private customerService: CustomerService) { }

	ngOnInit(): void
	{
		this.customerService.getAll().subscribe(data => this.customers = data);
	}

	public edit(customer: StaticCustomerResponse): void
	{
		this.customerService.getById(customer.customerID).subscribe(data =>
		{
			this.account.email = data.account.email;
			this.account.role = data.account.role;
			this.account.username = data.account.username;
			this.account.password = 'NONE';

			this.customer.accountID = data.account.accountID;
			this.customer.countryID = data.country.countryID;
			this.customer.firstName = data.firstName;
			this.customer.gender = data.gender;
			this.customer.lastName = data.lastName;
			this.customer.customerID = data.customerID;
			this.customer.phoneNumber = data.phoneNumber;
			this.customer.zipCode = data.zipCode;
		});
	}

	public delete(customer: StaticCustomerResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${customer.firstName} ${customer.lastName}?`))
		{
			this.customerService.delete(customer.customerID).subscribe(data => this.customers = this.customers.filter(x => x.customerID != data.customerID));
		}
	}

	public save(): void
	{
		if (this.customer.customerID === 0)
		{
			return;
		}

		let request: NewCustomerRequest = { account: this.account, customer: this.customer };

		this.customerService.update(this.customer.customerID, request).subscribe
		({
			complete: () =>
			{
				this.cancel();
			},
			error: (err) =>
			{
				console.log(Object.values(err.error.errors).join(', '));
			}
		});
	}

	public validate(value: boolean): void
	{
		for (let prop of Object.entries(this.customer))
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
		for (let prop of Object.entries(this.account))
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
		this.isValid = true;
	}

	public cancel(): void
	{
		this.customer =
		{
			customerID: 0,
			accountID: 0,
			countryID: 0,
			firstName: '',
			gender: '',
			lastName: '',
			phoneNumber: '',
			zipCode: 0
		};
	}
}
