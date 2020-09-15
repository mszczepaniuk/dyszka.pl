import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './component/nav-menu/nav-menu.component';
import { HomeComponent } from './component/home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { IdentityService } from './service/identity.service';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material'
import { FormErrorComponent } from './component/form/form-error.component';
import { ForbiddenComponent } from './component/forbidden/forbidden.component';
import { ProfileComponent } from './component/profile/profile.component';
import { AuthGuard } from './authorization/auth-guard';
import { UserService } from './service/user.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AdministrationService } from './service/administration.service';
import { AdministrationComponent } from './component/administration/administration.component';
import { OfferDetailsComponent } from './component/offer/details/offer-details.component';
import { OfferFormComponent } from './component/offer/form/offer-form.component';
import { OfferService } from './service/offer.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    FormErrorComponent,
    ForbiddenComponent,
    ProfileComponent,
    AdministrationComponent,
    OfferDetailsComponent,
    OfferFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    BrowserAnimationsModule,
    FontAwesomeModule
  ],
  providers: [
    IdentityService,
    UserService,
    AdministrationService,
    OfferService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
