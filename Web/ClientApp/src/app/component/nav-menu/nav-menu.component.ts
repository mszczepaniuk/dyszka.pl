import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isLoggedIn = false;

  constructor(public identityService: IdentityService) {

  }

  ngOnInit() {
    this.isLoggedIn = this.identityService.isLoggedIn();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.identityService.logout();
  }
}
