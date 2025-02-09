import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../api/services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup;
  constructor(private formBuilder: FormBuilder,
    private matDialog: MatDialog,
    private authService: AuthService,
    private snackBar: MatSnackBar) {

    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator })
  }

  passwordMatchValidator(group: FormGroup) {
  const newPassword = group.get('newPassword');
  const confirmPassword = group.get('confirmPassword');

  if (!newPassword || !confirmPassword) return null;

  if (confirmPassword.value !== newPassword.value) {
    confirmPassword.setErrors({ mismatch: true });
  } else {
    confirmPassword.setErrors(null);
  }

  return null;
}
  changePassword() {
    if (this.changePasswordForm.invalid) {
      console.error('Form is invalid');
      return;
    }
    console.log(this.changePasswordForm.value);
    this.authService.changePassword(this.changePasswordForm.value).subscribe({
      next: () => {
        console.log('Password changed successfully');
        this.snackBar.open('Password changed successfully', 'close', {
          duration: 5000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['snackbar-success']
        });
        this.closeChangePasswordDialog();
      },
      error: (err) => {
        console.error('Error changing password:', err);
        this.snackBar.open('Failed to change password', 'Retry', {
          duration: 5000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: ['snackbar-error']
        });
      }
    });
  }

  closeChangePasswordDialog() {
    this.matDialog.closeAll();
  }


}
