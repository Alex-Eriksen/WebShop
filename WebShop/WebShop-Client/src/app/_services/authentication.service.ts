import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JwtDecodePlus } from '../helpers/JWTDecodePlus';
import { AuthenticationRequest, AuthenticationResponse } from '../_models/authentication';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService
{
	private url: string = environment.ApiUrl + "/Authentication";
	private accessTokenSubject: BehaviorSubject<string>;
	public OnTokenChanged: Observable<string>;

	public get AccessToken(): string
	{
		if (this.accessTokenSubject == null)
		{
			this.accessTokenSubject = new BehaviorSubject<string>('');
		}
		return this.accessTokenSubject.value;
	}

	public get Expiration(): number
	{
		return JwtDecodePlus.jwtDecode(this.AccessToken).exp;
	}

	constructor(private http: HttpClient, private router: Router)
	{
		this.accessTokenSubject = new BehaviorSubject<string>('');
		this.OnTokenChanged = this.accessTokenSubject.asObservable();
	}

	/** Sends a string with either a username or an email, together with a string of the password.
	 *  Responds with a new RefreshToken and an AccessToken
	 */
	public authenticate(request: AuthenticationRequest): Observable<AuthenticationResponse>
	{
		return this.http.post<AuthenticationResponse>(this.url, request, { withCredentials: true })
			.pipe(map(response =>
			{
				this.accessTokenSubject.next(response.accessToken);
				return response;
			}));
	}

	public refreshToken(): Observable<AuthenticationResponse>
	{
		return this.http.put<AuthenticationResponse>(this.url, {}, { withCredentials: true })
			.pipe(map(response =>
			{
				this.accessTokenSubject.next(response.accessToken);
				return response;
			}));
	}

	public revokeToken(): Observable<string>
	{
		return this.http.delete<string>(this.url, { withCredentials: true })
			.pipe(map(response =>
			{
				this.accessTokenSubject.next('');
				this.router.navigate([ '/login' ]);
				return response;
			}));
	}
}
