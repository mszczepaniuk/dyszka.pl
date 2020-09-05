import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { IdentityService } from '../../service/identity.service';
import { UserService } from '../../service/user.service';
import { User } from '../../model/user.model';
import { UserBuilder } from '../../model/builder/user.builder';
import { NgForm, Validators, FormBuilder, FormGroup, AbstractControl, ValidatorFn, FormControl } from '@angular/forms';
import { faOutdent, faEnvelope, faEdit, faSave, faUndo, faUserMinus, faBan } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends BaseComponent implements OnInit {
  faOutdent = faOutdent;
  faEnvelope = faEnvelope;
  faEdit = faEdit;
  faSave = faSave;
  faUndo = faUndo;
  faUserMinus = faUserMinus;
  faBan = faBan;

  public user: User;
  public form: FormGroup;

  constructor(private route: ActivatedRoute,
    private identityService: IdentityService,
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder) {
    super();
  }

  ngOnInit() {
    this.form = this.createForm();
    this.form.disable();
    this.safeSub(
      this.route.paramMap.subscribe(params => {
        const username = params.get('username') || this.identityService.user$.value.userName;
        this.safeSub(
          this.userService.getUserByUserName(username).subscribe(user => {
            if (!params.has('username')) {
              this.safeSub(this.identityService.user$.subscribe(user => {
                this.user = user;
                this.form.patchValue(this.user);
              }));
            } else {
              if (!user) {
                this.router.navigateByUrl("/forbidden");
              }
              this.user = new UserBuilder().addApplicationData(user).build();
              this.form.patchValue(this.user);
            }
          })
        );
      })
    );
  }

  private createForm() {
    return this.formBuilder.group({
      description: [''],
      userName: [''],
      telephoneNumber: [''],
      email: ['']
  });
  }
}
