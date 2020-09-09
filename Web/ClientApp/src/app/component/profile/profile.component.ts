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
import { BehaviorSubject } from 'rxjs';

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

  public user$ = new BehaviorSubject<User>(null);
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
    this.safeSub(this.user$.subscribe(user => {
      if (user) {
        this.form.patchValue(user);
      }
    }));
    this.safeSub(
      this.route.paramMap.subscribe(params => {
        this.username = params.get('username') || this.identityService.user$.value.userName;
        this.safeSub(
          this.userService.getUserByUserName(this.username).subscribe(appUser => {
            if (!params.has('username')) {
              this.safeSub(this.identityService.user$.subscribe(user => {
                this.user$.next(user);
                this.loading = false;
              }));
            } else {
              if (!appUser) {
                this.router.navigateByUrl("/forbidden");
              }
              this.safeSub(this.userService.getUserIdentityData(this.username).subscribe(identityUser => {
                this.user$.next(new UserBuilder().addApplicationData(appUser).addIdentityData(identityUser).build());
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
    this.form.patchValue(this.user$.value);
    this.form.disable();
  }

  public formSubmit() {
    if (this.canUpdate && this.form.valid && this.identityService.isLoggedIn()) {
      this.loading = true;
      this.userService.editCurrentUser(this.getDataFromForm()).subscribe(user => {
        this.user$.next(new UserBuilder(this.user$.value).addApplicationData(user).build());
        this.form.disable();
        this.loading = false;
      });
    }
  }

  public banUser() {
    this.userService.banUser(this.username)
      .subscribe(() => {
        const user = this.user$.value;
        user.isBanned = true;
        this.user$.next(user);
      });
  }

  public unbanUser() {
    this.userService.unbanUser(this.username)
      .subscribe(() => {
        const user = this.user$.value;
        user.isBanned = false;
        this.user$.next(user);
      });
  }

  public deleteUser() {
    this.userService.removeUser(this.user$.value.applicationId).subscribe();
  }

  public isProfileOwner() {
    return this.identityService.isLoggedIn() && this.username === this.identityService.user$.value.userName;
  }

  public isBanned() {
    return this.user$.value && this.user$.value.isBanned;
  }

  public isInRole(rolename: string) {
    return this.user$.value && this.user$.value.roles.some(r => r === rolename);
  }

  public useDefaultImage() {
    return !(this.user$.value && this.user$.value.profileImage);
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
      profileImage: this.selectedImage || this.user$.value.profileImage
    }
  }
}
