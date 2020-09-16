import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });
  private submitted = false;
  private errors: string[];

  constructor(public identityService: IdentityService) { }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.valid) {
      this.identityService.logIn(this.loginForm.controls['username'].value, this.loginForm.controls['password'].value);
    } else {
      this.setErrors();
    }
  }

  private setErrors() {
    this.errors = [];
    if (this.loginForm.controls.username.errors.required) {
      this.errors.push('Nazwa użytkownika nie może być pusta');
    }
    if (this.loginForm.controls.password.errors.required) {
      this.errors.push('Hasło nie może być puste');
    }
  }
}
