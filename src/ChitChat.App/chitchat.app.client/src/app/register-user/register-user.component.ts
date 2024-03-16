import { Component, OnInit } from '@angular/core';
import {UserService} from '../api/services/user.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})

export class RegisterUserComponent implements OnInit {
  form: FormGroup;

  constructor(private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {

    this.form = this.formBuilder.group({
      displayName: ['', [Validators.required, Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(300)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
  }

  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')!.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  registerUser() {
    if (this.form.invalid)
      return;

      console.log(this.form.value);
      this.userService.registerUser(this.form.value).subscribe(_ => {
        console.log('posted to server');
        this.router.navigate(['/signin-user']);
      });
  }

  emailExists: boolean = false;
  checkIfUserExists(): void {
    const email = this.form.get('email')?.value;
    console.log(email);
    if (!email)
      return;
    this.userService.getEmail(email).subscribe(response => {
      console.log('user does not exist', response);
      this.emailExists = false;
    },
      error => { console.log(error); this.emailExists = true; }
    )
  }

  getEmailValidationMessage(): string {
    return this.emailExists ? 'Email exists aleady' : '';
  }

  displayNameExists: boolean = false;
  checkIfDisplayNameExists(): void {
    const displayName = this.form.get('displayName')?.value;
    console.log(displayName);
    if (!displayName)
      return;
    this.userService.getDisplayName(displayName).subscribe(response => {
      console.log('display name does not exist', response);
      this.displayNameExists = false;
    },
      error => { console.log(error); this.displayNameExists = true; }
    )
  }

  getDisplayNameValidationMessage(): string {
    return this.displayNameExists ? 'Display name exists aleady' : '';
  }
}
