import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TelemetryDetailsComponent } from './telemetry/telemetry-details/telemetry-details.component';
import { TelemetryComponent } from './telemetry/telemetry.component';

const routes: Routes = [
  { 
    path: '',
    component: TelemetryComponent,
    children: [
      { path: 'details/:id', component: TelemetryDetailsComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
