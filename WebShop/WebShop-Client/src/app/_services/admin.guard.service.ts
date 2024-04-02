import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtDecodePlus } from '../helpers/JWTDecodePlus';
import { AuthenticationService } from './authentication.service';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate
{
	constructor(private router: Router, private authenticationService: AuthenticationService) { }

	public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree
	{
		return new Promise<boolean>(resolve =>
		{
			this.authenticationService.OnTokenChanged.subscribe((token) =>
			{
				if (token !== '')
				{
					let role: string = JwtDecodePlus.jwtDecode(token).role;
					if (role === 'Admin')
					{
						resolve(true);
					}

					resolve(false);
				}
			});
		});
	}
}
