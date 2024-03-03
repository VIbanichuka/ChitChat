import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { DmsComponent } from './dms/dms.component';
import { FriendsComponent } from './friends/friends.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { RegisterUserComponent } from './register-user/register-user.component';
import { SigninUserComponent } from './signin-user/signin-user.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DmsComponent,
    FriendsComponent,
    SidenavComponent,
    RegisterUserComponent,
    SigninUserComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    MatSidenavModule,
    FormsModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'signin-user', component: SigninUserComponent },
      { path: 'register-user', component: RegisterUserComponent },

      {
        path: 'home',
        component: SidenavComponent,
        children: [
          { path: 'home', component: HomeComponent },
          { path: 'friends', component: FriendsComponent },
          { path: 'dms', component: DmsComponent },
        ]
      },

      { path: '', redirectTo: '/signin-user', pathMatch: 'full' },
    ]),
    NoopAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }