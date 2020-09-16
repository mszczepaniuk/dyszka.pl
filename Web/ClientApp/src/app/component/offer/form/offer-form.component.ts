import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatDialog } from '@angular/material';
import { OfferService } from '../../../service/offer.service';
import { DialogComponent } from '../../dialog/dialog.component';
import { DialogResult } from '../../../enum/dialog-result.enum';

@Component({
  selector: 'app-offer-form',
  templateUrl: './offer-form.component.html',
  styleUrls: ['./offer-form.component.css']
})
export class OfferFormComponent extends BaseComponent implements OnInit {
  private form: FormGroup;
  private loading: boolean;
  private selectedImage: string = '';
  private submitted = false;
  private errors: string[];

  constructor(private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private offerService: OfferService,
    private dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.form = this.createForm();
  }

  private onSubmit() {
    this.submitted = true;
    if (this.form.valid) {
      this.dialog.open(DialogComponent, {
        width: '450px',
        data: {
          message: 'Czy na pewno chcesz dodać ofertę? Nie można jej potem modyfikować.',
        }
      }).afterClosed().subscribe((result: DialogResult) => {
        if (result === DialogResult.Yes) {
          this.offerService.addOffer(this.getFormData());
        }
      });
    } else {
      this.setErrors();
    }
  }

  private createForm() {
    return this.formBuilder.group({
      image: ['', [Validators.required]],
      title: ['', [Validators.required, Validators.maxLength(30)]],
      price: ['', [Validators.required, Validators.min(10)]],
      tags: ['', [Validators.required, Validators.pattern(/^([a-z]|[A-Z]){2,20}$|(^((([a-z]|[A-Z]){2,20}),){1,4})([a-z]|[A-Z]){2,20}$/)]],
      shortDescription: ['', [Validators.required, Validators.maxLength(160)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]]
    });
  }

  private processFile(image) {
    this.snackBar.open('Przetwarzanie zdjęcia');
    this.loading = false;
    const reader = new FileReader();
    reader.readAsDataURL(image.files[0]);
    reader.onload = () => {
      this.selectedImage = reader.result.toString();
      this.loading = true;
      this.snackBar.open('Zdjęcie przetworzono poprawnie', '', { duration: 2000 });
    }
    reader.onerror = () => {
      this.selectedImage = '';
      this.form.controls.image.setValue('');
      this.loading = true;
      this.snackBar.open('Błąd podczas przetwarzania', '', { duration: 2000 });
    }
  }

  private getFormData() {
    var data = this.form.getRawValue();
    data.image = this.selectedImage;
    data.tags = data.tags.split(',');
    return data;
  }

  private setErrors() {
    this.errors = [];
    if (this.form.controls.image.errors && this.form.controls.image.errors.required) {
      this.errors.push('Należy wybrać zdjęcie');
    }
    if (this.form.controls.title.errors && this.form.controls.title.errors.required) {
      this.errors.push('Należy podać tytuł');
    }
    if (this.form.controls.price.errors && this.form.controls.price.errors.required) {
      this.errors.push('Należy podać cenę');
    }
    if (this.form.controls.tags.errors && this.form.controls.tags.errors.required) {
      this.errors.push('Należy podać tagi');
    }
    if (this.form.controls.shortDescription.errors && this.form.controls.shortDescription.errors.required) {
      this.errors.push('Należy podać krótki opis');
    }
    if (this.form.controls.description.errors && this.form.controls.description.errors.required) {
      this.errors.push('Należy podać długi opis');
    }
    if (this.form.controls.shortDescription.errors && this.form.controls.shortDescription.errors.maxlength) {
      this.errors.push('Maksymalna długość krótkiego opisu to 160 znaków');
    }
    if (this.form.controls.description.errors && this.form.controls.description.errors.maxlength) {
      this.errors.push('Maksymalna długość długiego opisu to 1000 znaków');
    }
    if (this.form.controls.title.errors && this.form.controls.title.errors.maxlength) {
      this.errors.push('Maksymalna długość tytułu to 30 znaków');
    }
    if (this.form.controls.price.errors && this.form.controls.price.errors.min) {
      this.errors.push('Cena powinna wynosić conajmniej 10 zł');
    }
    if (this.form.controls.tags.errors && this.form.controls.tags.errors.pattern) {
      this.errors.push('Tagi należy podać oddzielone od siebie przecinkami, bez spacji');
      this.errors.push('Tag może zawierać wyłącznie litery bez polskich znaków od 2 do 20 znaków');
    }
  }
}
