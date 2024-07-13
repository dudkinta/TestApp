import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration-success',
  templateUrl: './registration-success.component.html',
  styleUrls: ['./registration-success.component.css']
})
export class RegistrationSuccessComponent {
  message: string = 'Пользователь успешно зарегистрирован!';

  constructor(private router: Router) {}
  
  onAgain() {
    this.router.navigate(['/registration']);
  }
}