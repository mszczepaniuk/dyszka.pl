import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { IdentityService } from "../../service/identity.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  })
  public submitted = false;

  constructor(public identityService: IdentityService) { }

  onSubmit() {
    this.submitted = true;
    this.identityService.logIn(this.loginForm.controls["username"].value, this.loginForm.controls["password"].value);
  }
}
