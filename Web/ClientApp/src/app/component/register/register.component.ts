import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  registerForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required])
  })

  constructor(private identityService: IdentityService) { }

  onSubmit() {
    this.identityService.register(this.registerForm.controls['username'].value,
      this.registerForm.controls['password'].value,
      this.registerForm.controls['confirmPassword'].value);
  }
}
