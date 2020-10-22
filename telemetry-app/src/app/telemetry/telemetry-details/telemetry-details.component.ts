import { Component, Input, OnInit } from '@angular/core';
import {AppInfo, StatisticsEvent, TelemetryService} from '../telemetry.service'
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-telemetry-details',
  templateUrl: './telemetry-details.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry-details.component.less']
})

export class TelemetryDetailsComponent implements OnInit {
  id : string
  appInfo$ : Observable<AppInfo>;
  events$ : Observable<StatisticsEvent[]>
  displayedColumns: string[] = ['name', 'date', 'description'];

  constructor(
    private route: ActivatedRoute,
    private telemetryService: TelemetryService
  ) { }

  ngOnInit(): void {
    this.appInfo$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.telemetryService.getAppInfo(params.get('id')))
    );

    this.events$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.telemetryService.getEvents(params.get('id')))
    );
  }

  onDeleteButtonClick() {
    this.telemetryService
      .deleteEvents(this.route.snapshot.paramMap.get('id'))
      .subscribe(x => this.ngOnInit());
  }
}
