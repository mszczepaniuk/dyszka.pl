import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  registerForm = this.formBuilder.group({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.pattern(/\d/)]),
    confirmPassword: new FormControl('', [])
  },
    {
      validators: this.passwordMatchValidator
    });


  private submitted = false;
  private errors: string[];

  constructor(private identityService: IdentityService,
    private formBuilder: FormBuilder) { }

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.valid) {
      this.identityService.register(this.registerForm.controls['username'].value,
        this.registerForm.controls['password'].value,
        this.registerForm.controls['confirmPassword'].value);
    } else {
      this.setErrors();
    }
  }

  private setErrors() {
    this.errors = [];
    console.log(this.registerForm.controls.password.errors);
    if (this.registerForm.controls.username.errors.required) {
      this.errors.push('Nazwa użytkownika nie może być pusta');
    }
    if (this.registerForm.controls.password.errors.required) {
      this.errors.push('Hasło musi mieć conajmniej 6 znaków długości');
      this.errors.push('Hasło musi zawierać przynajmniej jedną liczbę');
    }
    if (this.registerForm.controls.password.errors.minlength) {
      this.errors.push('Hasło musi mieć conajmniej 6 znaków długości');
    }
    if (this.registerForm.controls.password.errors.pattern) {
      this.errors.push('Hasło musi zawierać przynajmniej jedną liczbę');
    }
    if (this.registerForm.errors.mismatch) {
      this.errors.push('Oba hasła muszą być takie same');
    }
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null : { 'mismatch': true };
  }
}
