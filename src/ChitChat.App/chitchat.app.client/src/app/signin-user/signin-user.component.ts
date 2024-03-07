import { Component, OnInit } from '@angular/core';
import { AuthService } from '../api/services/auth.service';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-signin-user',
  templateUrl: './signin-user.component.html',
  styleUrls: ['./signin-user.component.css']
})

export class SigninUserComponent implements OnInit {

  constructor(private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  form = this.formBuilder.group({
    email: ['' , Validators.compose([Validators.required, Validators.min(5), Validators.max(300)])],
    password: ['', Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(20)])],
  })

  signin() {

    if (this.form.invalid)
      return;

    console.log(this.form.value)
    this.authService.authPost({ body: this.form.value }).subscribe(
      token => { console.log(token); this.router.navigate(['/home']); },
      error => {
        console.error("Invalid authentication", error);
      }
    );
  }

  ngOnInit(): void {

  }
}
