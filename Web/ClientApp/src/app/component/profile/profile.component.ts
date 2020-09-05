import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { IdentityService } from '../../service/identity.service';
import { UserService } from '../../service/user.service';
import { User } from '../../model/user.model';
import { UserBuilder } from '../../model/builder/user.builder';
import { FormBuilder, FormGroup } from '@angular/forms';
import { faOutdent, faEnvelope, faEdit, faSave, faUndo, faUserMinus, faBan } from '@fortawesome/free-solid-svg-icons';
import { Config } from '../../config';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends BaseComponent implements OnInit {
  defaultImage = Config.defaultProfileImage;
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
  private selectedImage = '';
  private canUpdate = true;
  private loading = true;

  constructor(private route: ActivatedRoute,
    public identityService: IdentityService,
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar) {
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
                this.loading = false;
              }));
            } else {
              if (!user) {
                this.router.navigateByUrl("/forbidden");
              }
              this.user = new UserBuilder().addApplicationData(user).build();
              this.form.patchValue(this.user);
              this.safeSub(this.userService.getUserIdentityData(this.username).subscribe(user => {
                this.user = new UserBuilder(this.user).addIdentityData(user).build();
                this.loading = false;
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
    if (this.canUpdate && this.form.valid && this.identityService.isLoggedIn()) {
      this.loading = true;
      this.userService.editCurrentUser(this.getDataFromForm()).subscribe(user => {
        this.user = new UserBuilder(this.user).addApplicationData(user).build();
        this.form.disable();
        this.form.patchValue(this.user);
        this.loading = false;
      });
    }
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

  public processFile(image) {
    this.snackBar.open('Przetwarzanie zdjęcia');
    this.canUpdate = false;
    const reader = new FileReader();
    reader.readAsDataURL(image.files[0]);
    reader.onload = () => {
      this.selectedImage = reader.result.toString();
      this.canUpdate = true;
      this.snackBar.open('Zdjęcie przetworzono poprawnie', '', { duration: 2000 });
    }
    reader.onerror = () => {
      this.selectedImage = '';
      this.canUpdate = true;
      this.snackBar.open('Błąd podczas przetwarzania', '', { duration: 2000 });
    }
  }

  private createForm() {
    return this.formBuilder.group({
      description: [''],
      userName: [''],
      telephoneNumber: [''],
      email: ['']
    });
  }

  private getDataFromForm() {
    return {
      description: this.form.controls.description.value,
      userName: this.form.controls.userName.value,
      telephoneNumber: this.form.controls.telephoneNumber.value,
      email: this.form.controls.email.value,
      profileImage: this.selectedImage
    }
  }
}
