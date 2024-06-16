import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import {MatTabsModule} from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';

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
import { UserProfileComponent } from './user-profile/user-profile.component';
import { MainUserProfileComponent } from './main-user-profile/main-user-profile.component';
import { AuthInterceptor } from './api/services/auth.interceptor';
import { UserHubComponent } from './user-hub/user-hub.component';
import { ManageInvitationComponent } from './manage-invitation/manage-invitation.component';
import { ChannelsComponent } from './channels/channels.component';
import { ChannelPopupComponent } from './channel-popup/channel-popup.component';
import { ChannelsChatComponent } from './channels-chat/channels-chat.component';
import { ErrorInterceptor } from './api/services/error.interceptor';
import { NotFoundErrorComponent } from './not-found-error/not-found-error.component';
import { SeverErrorComponent } from './sever-error/sever-error.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DmsComponent,
    FriendsComponent,
    SidenavComponent,
    RegisterUserComponent,
    SigninUserComponent,
    UserProfileComponent,
    MainUserProfileComponent,
    UserHubComponent,
    ManageInvitationComponent,
    ChannelsComponent,
    ChannelPopupComponent,
    ChannelsChatComponent,
    NotFoundErrorComponent,
    SeverErrorComponent,
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
    MatMenuModule,
    MatDialogModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    OverlayModule,
    MatDividerModule,
    MatCardModule,
    MatTabsModule,
    MatProgressBarModule,
    RouterModule.forRoot([
      { path: 'signin-user', component: SigninUserComponent },
      { path: '500', component: SeverErrorComponent},
      { path: 'register-user', component: RegisterUserComponent },
      { path: 'home',component: SidenavComponent, children: [
          { path: '', component: HomeComponent },
          { path: 'friends', component: FriendsComponent },
          { path: 'dms/:userId', component: DmsComponent },
          { path: 'dms', component: DmsComponent },
          { path: 'manage-invitations', component: ManageInvitationComponent },
          { path: 'channels', component: ChannelsComponent },
          { path: 'channel/:channelId', component: ChannelsChatComponent },
          { path: '**', component: NotFoundErrorComponent },
      ]},
      { path: '', redirectTo: '/signin-user', pathMatch: 'full' },
    ]),
    NoopAnimationsModule
  ],
  providers: [
    {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
