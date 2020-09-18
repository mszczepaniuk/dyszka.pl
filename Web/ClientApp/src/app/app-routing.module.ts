import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './component/home/home.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { ForbiddenComponent } from './component/forbidden/forbidden.component';
import { ProfileComponent } from './component/profile/profile.component';
import { AuthGuard } from './authorization/auth-guard';
import { AdministrationComponent } from './component/administration/administration.component';
import { OfferFormComponent } from './component/offer/form/offer-form.component';
import { OfferDetailsComponent } from './component/offer/details/offer-details.component';
import { MessageComponent } from './component/message/message.component';
import { MessageInboxComponent } from './component/message/inbox/message-inbox.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  {
    path: 'forbidden',
    pathMatch: 'full',
    component: ForbiddenComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'administration',
    component: AdministrationComponent,
    canActivate: [AuthGuard],
    data: { role: 'admin' }
  },
  {
    path: 'messages',
    children: [
      {
        path: ':username',
        component: MessageComponent,
        canActivate: [AuthGuard]
      },
      {
        path: '',
        pathMatch: 'full',
        component: MessageInboxComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: 'offer',
    children: [
      {
        path: 'form',
        component: OfferFormComponent,
        canActivate: [AuthGuard]
      },
      {
        path: ':id',
        component: OfferDetailsComponent
      }
    ],
  },
  {
    path: 'profile',
    children: [
      {
        path: ':username',
        component: ProfileComponent
      },
      {
        path: '',
        canActivate: [AuthGuard],
        pathMatch: 'full',
        component: ProfileComponent
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'forbidden'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
