import { Component, OnInit } from '@angular/core';
import { UserService } from '../api/services/user.service';
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
      this.userService.userPost({ body: this.form.value }).subscribe(_ => {
        console.log('posted to server');
        this.router.navigate(['/home']);
      });
  }

  checkIfUserExists(): void {
    this.userService.userEmailGet(this.form.get('email')?.value).subscribe(_ => {
      console.log("user doesn't exist");
    },
      error => { console.log("user exists", error); alert("User already exists") }
    )
  }
}
