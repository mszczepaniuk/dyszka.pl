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
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogComponent } from './component/dialog/dialog.component';
import { CommentComponent } from './component/comment/comment.component';
import { CommentService } from './service/comment.service';
import { MessageComponent } from './component/message/message.component';
import { MessageService } from './service/message.service';
import { MessageInboxComponent } from './component/message/inbox/message-inbox.component';
import { OrderComponent } from './component/order/order.component';
import { OrderService } from './service/order.service';
import { BillingDataComponent } from './component/billing-data/billing-data.component';
import { PaymentService } from './service/payment.service';
import { OfferPromotionService } from './service/offer-promotion.service';
import { NgxJsonLdModule } from 'ngx-json-ld';

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
    OfferFormComponent,
    DialogComponent,
    CommentComponent,
    MessageComponent,
    MessageInboxComponent,
    OrderComponent,
    BillingDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    BrowserAnimationsModule,
    FontAwesomeModule,
    MatTooltipModule,
    MatDialogModule,
    NgxJsonLdModule
  ],
  providers: [
    IdentityService,
    UserService,
    AdministrationService,
    OfferService,
    CommentService,
    MessageService,
    OrderService,
    PaymentService,
    OfferPromotionService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    AuthGuard
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    DialogComponent
  ],
})
export class AppModule { }
