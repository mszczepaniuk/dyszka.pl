import { AbstractControl } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-form-error',
  templateUrl: './form-error.component.html'
})
export class FormErrorComponent {
  @Input() control: AbstractControl;
  @Input() submitted: boolean;
  @Input() error: string;
  @Input() label = true;
}
