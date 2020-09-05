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
  defaultImage = 'data:image/png;base64,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';
  faOutdent = faOutdent;
  faEnvelope = faEnvelope;
  faEdit = faEdit;
  faSave = faSave;
  faUndo = faUndo;
  faUserMinus = faUserMinus;
  faBan = faBan;

  public user: User;
  public form: FormGroup;

  private username: string;

  constructor(private route: ActivatedRoute,
    public identityService: IdentityService,
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
        this.username = params.get('username') || this.identityService.user$.value.userName;
        this.safeSub(
          this.userService.getUserByUserName(this.username).subscribe(user => {
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
              this.safeSub(this.userService.getUserIdentityData(this.username).subscribe(user => {
                this.user = new UserBuilder(this.user).addIdentityData(user).build();
              }));
            }
          })
        );
      })
    );
  }

  public editClick() {
    this.form.enable();
  }

  public undoClick() {
    this.form.patchValue(this.user);
    this.form.disable();
  }

  public formSubmit() {

  }

  public banUser() {

  }

  public deleteUser() {

  }

  public isProfileOwner() {
    return this.identityService.isLoggedIn() && this.username === this.identityService.user$.value.userName;
  }

  public isBanned() {
    return this.user && this.user.isBanned;
  }

  public isInRole(rolename: string) {
    return this.user && this.user.roles.some(r => r === rolename);
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
