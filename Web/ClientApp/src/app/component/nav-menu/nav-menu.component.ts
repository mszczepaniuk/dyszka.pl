import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../service/identity.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Helpers } from '../../common/helpers';
import { faSearch, faHome } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  faSearch = faSearch;
  faHome = faHome;
  form: FormGroup;
  isExpanded = false;

  constructor(public identityService: IdentityService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({ query: ['', [Validators.required, Validators.pattern(Helpers.tagsPattern)]] });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onSubmit() {
    if (this.form.valid) {
      this.router.navigateByUrl(this.queryToUrl());
    } else {
      this.snackBar.open('Tagi powinny być wypisane po przecinku bez spacji')
    }
  }

  queryToUrl() {
    let tags = this.form.controls.query.value.split(',');
    if (!Array.isArray(tags)) {
      tags = Array.from(tags);
    }
    let query = `?tags=${tags[0]}`;
    if (tags.length > 1) {
      tags.forEach(tag => query = query.concat(`&tags=${tag}`));
    }
    return query;
  }
}
