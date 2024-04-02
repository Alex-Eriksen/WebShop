import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './components/pages/homepage/homepage.component';
import { AdminComponent } from './components/pages/admin/admin.component';
import { AdminCategoryComponent } from './components/pages/admin/category/category.component';
import { CustomerComponent } from './components/pages/admin/customer/customer.component';
import { DiscountComponent } from './components/pages/admin/discount/discount.component';
import { ProductAdminComponent } from './components/pages/admin/product/product.component';
import { LoginComponent } from './components/pages/login/login.component';
import { SignUpComponent } from './components/pages/sign-up/sign-up.component';
import { AdminGuard } from './_services/admin.guard.service';
import { ProductComponent } from './components/pages/product/product.component';
import { ProfileComponent } from './components/pages/profile/profile.component';
import { CartComponent } from './components/pages/cart/cart.component';
import { AuthenticationGuard } from './_services/authentication.guard.service';
import { ChangePasswordComponent } from './components/pages/profile/changePassword/change-password/change-password.component';
import { OrderComponent } from './components/pages/admin/order/order.component';
import { CategoryComponent } from './components/pages/category/category.component';

const routes: Routes = [
	{ path: '', component: HomepageComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'sign-up', component: SignUpComponent },
	{ path: 'product/:id', component: ProductComponent },
	{ path: 'category/:id', component: CategoryComponent },
	{ path: 'cart', component: CartComponent, canActivate: [AuthenticationGuard]},
	{ path: 'admin', component: AdminComponent, canActivate: [AdminGuard] },
	{ path: 'admin/customer', component: CustomerComponent, canActivate: [AdminGuard] },
	{ path: 'admin/product', component: ProductAdminComponent, canActivate: [AdminGuard] },
	{ path: 'admin/discount', component: DiscountComponent, canActivate: [AdminGuard] },
	{ path: 'admin/category', component: AdminCategoryComponent, canActivate: [AdminGuard] },
	{ path: 'admin/order', component: OrderComponent, canActivate: [AdminGuard] },
	{ path: 'pages/profile', component: ProfileComponent, canActivate: [AuthenticationGuard] },
	{ path: 'profile/changePassword', component: ChangePasswordComponent, canActivate: [AuthenticationGuard] },

	{ path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
