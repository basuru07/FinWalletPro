import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
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
    BeneficiariesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,

    ReactiveFormsModule
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US }],
  bootstrap: [AppComponent]
})
export class AppModule { }
