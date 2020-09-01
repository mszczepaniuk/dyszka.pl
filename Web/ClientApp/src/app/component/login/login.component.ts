import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { IdentityService } from "../../service/identity.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  })

  constructor(public identityService: IdentityService,
    public router: Router) { }

  onSubmit() {
    this.identityService.logIn(this.loginForm.controls["username"].value, this.loginForm.controls["password"].value);
  }
}
