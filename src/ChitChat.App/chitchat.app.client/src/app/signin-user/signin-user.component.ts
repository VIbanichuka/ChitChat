import { Component, OnInit } from '@angular/core';
import { AuthService } from '../api/services/auth.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin-user',
  templateUrl: './signin-user.component.html',
  styleUrls: ['./signin-user.component.css']
})

export class SigninUserComponent {

  constructor(private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  form = this.formBuilder.group({
    email: [''],
    password: ['']
  })

  ngOnInit(): void {

  }

  signin() {
    console.log(this.form.value)
    this.authService.authPost({ body: this.form.value }).subscribe(
      token => { console.log(token);}
    );
  }
}
