import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorMessage } from 'src/app/models/error.models';

@Component({
  selector: 'app-registration-error',
  templateUrl: './registration-error.component.html',
  styleUrls: ['./registration-error.component.css']
})
export class RegistrationErrorComponent {
  message: string = 'Ошибка регистрации. Пожалуйста, попробуйте снова.';
  errormessage: string;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras.state as { errorMessage: ErrorMessage };
    this.errormessage = state.errorMessage.error;
  }

  onReturn() {
    this.router.navigate(['/registration']);
  }
}