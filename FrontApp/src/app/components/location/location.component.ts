import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LocationService } from '../../services/location.service';
import { UserService } from '../../services/user.service';
import { Country, Province } from '../../models/location.models';
import { User } from '../../models/user.models';
import { Router } from '@angular/router';


@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  locationForm: FormGroup;
  countries: Country[] = [];
  provinces: Province[] = [];
  user: User;

  constructor(private fb: FormBuilder, private locationService: LocationService, private userService: UserService, private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { user: User };
    this.user = state.user;

    this.locationForm = this.fb.group({
      country: ['', [Validators.required]],
      province: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.locationService.getCountries().subscribe((data: Country[]) => {
      this.countries = data;
    });
  }

  onCountryChange(countryId: string) {
    this.locationService.getProvinces(countryId).subscribe((data: Province[]) => {
      this.provinces = data;
    });
  }

  onSubmit() {
    if (this.locationForm.valid) {
      this.user.countryId = this.locationForm.value.country;
      this.user.provinceId = this.locationForm.value.province;
      
      this.userService.sendUserData(this.user).subscribe(
        response => {
          this.router.navigate(['/success']);
        },
        error => {
          const errorMessage = error.error;
          this.router.navigate(['/usererror'], { state: { errorMessage } });
        }
      );
    }
  }
}