import { APP_INITIALIZER, DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import localeDk from '@angular/common/locales/en-DK';
registerLocaleData(localeDk);

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginComponent } from './components/pages/login/login.component';
import { appInitializer } from './helpers/app.initializer';
import { AuthenticationService } from './_services/authentication.service';
import { AuthenticationInterceptor } from './_interceptor/authentication.interceptor';
import { HashLocationStrategy, LocationStrategy, registerLocaleData } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { SignUpComponent } from './components/pages/sign-up/sign-up.component';
import { AdminComponent } from './components/pages/admin/admin.component';
import { CustomerComponent } from './components/pages/admin/customer/customer.component';
import { ProductAdminComponent } from './components/pages/admin/product/product.component';
import { DiscountComponent } from './components/pages/admin/discount/discount.component';
import { AdminCategoryComponent } from './components/pages/admin/category/category.component';
import { HomepageComponent } from './components/pages/homepage/homepage.component';
import { ProductComponent } from './components/pages/product/product.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProfileComponent } from './components/pages/profile/profile.component';
import { CartComponent } from './components/pages/cart/cart.component';
import { SiteAlertComponent } from './components/site-alert/site-alert.component';
import { ChangePasswordComponent } from './components/pages/profile/changePassword/change-password/change-password.component';
import { OrderComponent } from './components/pages/admin/order/order.component';
import { CategoryComponent } from './components/pages/category/category.component';
import { ProductDisplayComponent } from './components/product-display/product-display.component';
import { InputFieldComponent } from './components/input-field/input-field.component';
import { InputSelectComponent } from './components/input-select/input-select.component';
import { InputAreaComponent } from './components/input-area/input-area.component';
import { NotificationComponent } from './components/notification/notification.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

@NgModule
({
  	declarations:
	[
    	AppComponent,
     	LoginComponent,
      	HeaderComponent,
      	SignUpComponent,
      	AdminComponent,
     	CustomerComponent,
      	ProductAdminComponent,
      	DiscountComponent,
     	 AdminCategoryComponent,
      	HomepageComponent,
	  	ProductComponent,
   		NavbarComponent,
		ProfileComponent,
		CartComponent,
		SiteAlertComponent,
		ChangePasswordComponent,
		OrderComponent,
		CategoryComponent,
		ProductDisplayComponent,
		InputFieldComponent,
		InputSelectComponent,
		InputAreaComponent,
     	ProfileComponent,
     	CartComponent,
     	SiteAlertComponent,
     	ChangePasswordComponent,
     	OrderComponent,
     	CategoryComponent,
     	ProductDisplayComponent,
     	NotificationComponent
  	],
  	imports:
	[
    	BrowserModule,
    	BrowserAnimationsModule,
    	ToastrModule.forRoot(),
		AppRoutingModule,
		HttpClientModule,
		FormsModule
  	],
	providers:
	[
		{ provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [ AuthenticationService ] },
		{ provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
		{ provide: LocationStrategy, useClass: HashLocationStrategy },
		{ provide: DEFAULT_CURRENCY_CODE, useValue: 'DKK' },
		{ provide: LOCALE_ID, useValue: 'en-DK' }
	],
  	bootstrap: [AppComponent]
})

export class AppModule { }
