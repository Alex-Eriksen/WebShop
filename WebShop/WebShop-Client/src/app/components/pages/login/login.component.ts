import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtDecodePlus } from 'src/app/helpers/JWTDecodePlus';
import { AuthenticationRequest } from 'src/app/_models/authentication';
import { AuthenticationService } from 'src/app/_services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit
{
	constructor(private router: Router, private authenticationService: AuthenticationService, private route: ActivatedRoute,) { }

	public request: AuthenticationRequest = { username_Email: '', password: '' };
	private returnUrl: string = "";
	private guardType: number = 0;
	public isValid: boolean = false;

	ngOnInit(): void
	{
		this.returnUrl = this.route.snapshot.queryParams[ "returnUrl" ] || "/";
		this.guardType = this.route.snapshot.queryParams[ "guard" ] || 0;
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
		for (let prop of Object.entries(this.request))
		{
			if (typeof (prop[ 1 ]) === 'string')
			{
				if (prop[ 1 ] === '')
				{
					this.isValid = false;
					return;
				}
			}
		}
		this.isValid = true;
	}

	public login(): void
	{
		if (this.isValid)
		{
			this.authenticationService.authenticate(this.request).subscribe
			({
				next: () =>
				{
					this.router.navigate([ this.returnUrl ]);
				},
				error: (err) =>
				{
					console.error(Object.values(err.error.errors).join(', '));
				}
			});
		}
	}

	public register(): void
	{
		this.router.navigate([ 'sign-up' ], { queryParams: { returnUrl: 'login' } })
	}
}
