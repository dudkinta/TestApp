import { Routes } from '@angular/router';
import { RegistrationComponent } from './registration/registration.component';
import { LocationComponent } from './location/location.component';

export const routes: Routes = [
  { path: 'registration', component: RegistrationComponent },
  { path: 'location', component: LocationComponent },
  { path: '', redirectTo: '/registration', pathMatch: 'full' }
];