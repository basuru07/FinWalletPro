import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './_components/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './_components/register/register.component';
import { SidebarComponent } from './_components/sidebar/sidebar.component';
import { DashboardComponent } from './_components/dashboard/dashboard.component';
import { HistoryComponent } from './_components/history/history.component';
import { DetailComponent } from './_components/detail/detail.component';
import { TransferComponent } from './_components/transfer/transfer.component';
import { BeneficiariesComponent } from './_components/beneficiaries/beneficiaries.component'; // <-- ADD THIS
import { JwtInterceptor } from './_interceptor/jwtInterceptor';
import { ShellComponent } from './_components/shell/shell.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardComponent } from './_components/card/card.component';
import { NotificationComponent } from './_components/notification/notification.component';
import { ProfileComponent } from './_components/profile/profile.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    SidebarComponent,
    DashboardComponent,
    HistoryComponent,
    DetailComponent,
    TransferComponent,
    BeneficiariesComponent,
    ShellComponent,
    CardComponent,
    NotificationComponent,
    ProfileComponent,


  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
CommonModule,
RouterModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
