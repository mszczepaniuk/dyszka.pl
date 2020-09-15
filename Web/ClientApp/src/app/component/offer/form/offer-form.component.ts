import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { OfferService } from '../../../service/offer.service';

@Component({
  selector: 'app-offer-form',
  templateUrl: './offer-form.component.html',
  styleUrls: ['./offer-form.component.css']
})
export class OfferFormComponent extends BaseComponent implements OnInit {
  private form: FormGroup;
  private loading: boolean;
  private selectedImage: string = '';

  constructor(private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private offerService: OfferService) {
    super();
  }

  ngOnInit(): void {
    this.form = this.createForm();
  }

  private onSubmit() {
    this.offerService.addOffer(this.getFormData());
  }

  private createForm() {
    return this.formBuilder.group({
      image: [''],
      title: [''],
      price: [''],
      tags: [''],
      shortDescription: [''],
      description: ['']
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
}
