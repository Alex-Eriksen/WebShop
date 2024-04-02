import { Component, OnInit } from '@angular/core';
import { DiscountRequest, StaticDiscountResponse } from 'src/app/_models/discount';
import { DiscountService } from 'src/app/_services/discount.service';

@Component({
  selector: 'app-discount',
  templateUrl: './discount.component.html',
  styleUrls: ['./discount.component.css']
})

export class DiscountComponent implements OnInit
{
	public discounts: StaticDiscountResponse[] = [];
	public discount: DiscountRequest = {
		discountID: 0,
		description: '',
		discountPercent: 0,
		name: '',
		revoked_At: new Date()
	};

	public isValid: boolean = false;

  	constructor(private discountService: DiscountService) { }

	ngOnInit(): void
	{
		this.discountService.getAll().subscribe(data => this.discounts = data);
	}

	public create(): void
	{
		this.discount.discountID = -1;
	}

	public edit(discount: StaticDiscountResponse): void
	{
		this.discount.discountID = discount.discountID;
		this.discount.description = discount.description;
		this.discount.discountPercent = discount.discountPercent;
		this.discount.revoked_At = discount.revoked_At;
		this.discount.name = discount.name;
	}

	public delete(discount: StaticDiscountResponse): void
	{
		if (confirm(`Er du sikker på du vil slette ${discount.name}?`))
		{
			this.discountService.delete(discount.discountID).subscribe(data => this.discounts = this.discounts.filter(x => x.discountID != data.discountID));
		}
	}

	public save(): void
	{
		if (!this.isValid) return;

		if (this.discount.discountID === -1)
		{
			// Create new discount
			this.discountService.create(this.discount).subscribe
			({
				next: (data) =>
				{
					this.discounts.push({
						created_At: data.created_At,
						description: data.description,
						discountID: data.discountID,
						discountPercent: data.discountPercent,
						name: data.name,
						modified_At: data.modified_At,
						revoked_At: data.revoked_At
					});
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
			// Update old discount
			this.discountService.update(this.discount.discountID, this.discount).subscribe
			({
				error: (err) =>
				{
					console.log(Object.values(err.error.errors).join(', '));
				},
				complete: () =>
				{
					this.cancel();
				}
			});
		}
	}

	public validate(value: boolean): void
	{
		for (let prop of Object.entries(this.discount))
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
		this.discount =
		{
			discountID: 0,
			description: '',
			discountPercent: 0,
			name: '',
			revoked_At: new Date()
		};
	}
}
