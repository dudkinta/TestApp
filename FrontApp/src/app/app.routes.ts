import { Routes } from '@angular/router';
import { RegistrationComponent } from './components/registration/registration.component';
import { LocationComponent } from './components/location/location.component';
import { RegistrationSuccessComponent } from './components/registration-success/registration-success.component';
import { RegistrationErrorComponent } from './components/registration-error/registration-error.component';

export const routes: Routes = [
  { path: 'registration', component: RegistrationComponent },
  { path: 'location', component: LocationComponent },
  { path: 'success', component: RegistrationSuccessComponent },
  { path: 'usererror', component: RegistrationErrorComponent },
  { path: '', redirectTo: '/registration', pathMatch: 'full' }
];