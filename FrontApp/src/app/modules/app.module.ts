import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from '../components/app/app.component';
import { RegistrationComponent } from '../components/registration/registration.component';
import { LocationComponent } from '../components/location/location.component';
import { routes } from '../app.routes';
import { ConsulService } from '../services/consul.service';

@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    LocationComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes)
  ],
  providers: [ConsulService],
  bootstrap: [AppComponent]
})
export class AppModule { }