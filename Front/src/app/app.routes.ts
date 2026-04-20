import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page.component';
import { LicencePageComponent } from './pages/licence-page.component';
import { ProductPageComponent } from './pages/product-page.component';
import { RegisterPageComponent } from './pages/register-page.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'licence', component: LicencePageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'products', component: ProductPageComponent },
  { path: '**', redirectTo: 'login' },
];
