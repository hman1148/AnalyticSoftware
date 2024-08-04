import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LandingComponent } from './landing.component';
import { LoginComponent } from '../auth/login/login.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', component: LandingComponent },
        {path: 'login', component: LoginComponent},
    ])],
    exports: [RouterModule]
})
export class LandingRoutingModule { }
