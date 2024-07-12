import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Country {
  id: string;
  name: string;
}

interface Province {
  id: string;
  name: string;
}

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  locationForm: FormGroup;
  countries: Country[] = [];
  provinces: Province[] = [];

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.locationForm = this.fb.group({
      country: ['', [Validators.required]],
      province: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.http.get<Country[]>('/api/countries').subscribe((data: Country[]) => {
      this.countries = data;
    });
  }

  onCountryChange(countryId: string) {
    this.http.get<Province[]>(`/api/countries/${countryId}/provinces`).subscribe((data: Province[]) => {
      this.provinces = data;
    });
  }

  onSubmit() {
    if (this.locationForm.valid) {
      // Логика отправки данных формы
    }
  }
}