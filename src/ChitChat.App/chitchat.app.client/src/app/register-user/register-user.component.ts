import { Component, OnInit } from '@angular/core';
import { UserService } from '../api/services/user.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent {

  constructor(private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router
  ){}

  form = this.formBuilder.group({
    displayName: [''],
    email: [''],
    password: [''],
    confirmPassword: ['']
  })

  ngOnInit(): void{   
  }

  registerUser() {
    console.log(this.form.value)
    this.userService.userPost({ body: this.form.value })
      .subscribe(_ => console.log("posted to server"))
    this.router.navigate(['/home'])
  }
}
