import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor
{
	private isLoggedIn: boolean = false;

	constructor(private authenticationService: AuthenticationService)
	{
		this.authenticationService.OnTokenChanged.subscribe((token) =>
		{
			if (token !== '')
			{
				this.isLoggedIn = true;
			}
			else
			{
				this.isLoggedIn = false;
			}
		});
  }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
	{
		const accessToken: string = this.authenticationService.AccessToken;
		const isApiUrl: boolean = request.url.startsWith(environment.ApiUrl);
		if (this.isLoggedIn && isApiUrl)
		{
			request = request.clone({
				setHeaders: { Authorization: `bearer ${accessToken}` }
			});
		}
    return next.handle(request);
  }
}
