import { AuthGuard } from './_guard/auth.guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './_components/login/login.component';
import { RegisterComponent } from './_components/register/register.component';
import { DashboardComponent } from './_components/dashboard/dashboard.component';
import { BeneficiariesComponent } from './_components/beneficiaries/beneficiaries.component';
import { HistoryComponent } from './_components/history/history.component';
import { TransferComponent } from './_components/transfer/transfer.component';
import { SidebarComponent } from './_components/sidebar/sidebar.component';
import { DetailComponent } from './_components/detail/detail.component';
import { ShellComponent } from './_components/shell/shell.component';
import { GuestGuard } from './_guard/guest.guard';
import { CardComponent } from './_components/card/card.component';
import { NotificationComponent } from './_components/notification/notification.component';
import { ProfileComponent } from './_components/profile/profile.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'transactions', component: HistoryComponent },
  { path: 'transactions/:id', component: DetailComponent },
  { path: 'transfer', component: TransferComponent },
  { path: 'beneficiaries', component: BeneficiariesComponent },
  { path: 'cards', component: CardComponent },
  { path: 'notifications', component: NotificationComponent },
  { path: 'profile', component: ProfileComponent },

  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // {
  //   path: 'auth',
  //   canActivate: [GuestGuard],
  //   component: ShellComponent,
  //   children: [
  //     { path: '', redirectTo: '/login', pathMatch: 'full' },
  //     { path: 'login', component: LoginComponent },
  //     { path: 'register', component: RegisterComponent },
  //   ],
  // },

  // {
  //   path: '',
  //   canActivate: [AuthGuard],
  //   component: ShellComponent,
  //   children: [
  //     { path: 'dashboard', component: DashboardComponent },
  //     { path: 'transactions', component: HistoryComponent },
  //     { path: 'transactions/:id', component: DetailComponent },
  //     { path: 'transfer', component: TransferComponent },
  //     { path: 'beneficiaries', component: BeneficiariesComponent },
  //     { path: 'cards', component: CardComponent },
  //     { path: 'notifications', component: NotificationComponent },
  //     { path: 'profile', component: ProfileComponent },

  //     { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  //   ],
  // },
  { path: '**', redirectTo: '/dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
