import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogResult } from '../../enum/dialog-result.enum';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html'
})
export class DialogComponent {

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: {
      message: string,
      yesButtonTitle: string,
      noButtonTitle: string,
      dialogTitle: string
    }) {
    const config: any = data || {};
    this.message = config.message || 'Czy jesteś pewien?';
    this.yesButtonTitle = config.yesButtonTitle || 'Tak';
    this.noButtonTitle = config.noButtonTitle || 'Nie';
    this.dialogTitle = config.dialogTitle || 'Ostrzeżenie';
  }

  message: string;
  yesButtonTitle: string;
  noButtonTitle: string;
  dialogTitle: string;

  DialogResult = DialogResult;

  onNoClick(): void {
    this.dialogRef.close(DialogResult.No);
  }

  onYesClick(): void {
    this.dialogRef.close(DialogResult.Yes);
  }
}
