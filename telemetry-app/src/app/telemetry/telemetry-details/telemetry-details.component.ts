import { Component, OnInit } from '@angular/core';
import { AppInfo, StatisticsEvent, TelemetryService } from '../telemetry.service'
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';

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
  destroy$: Subject<boolean> = new Subject<boolean>();
  
  constructor(
    private route: ActivatedRoute,
    private telemetryService: TelemetryService
  ) { }

  ngOnInit(): void {
    this.appInfo$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.telemetryService.getAppInfo(params.get('id'))),
      takeUntil(this.destroy$)
    );

    this.events$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.telemetryService.getEvents(params.get('id'))),
      takeUntil(this.destroy$)
    );
  }

  onDeleteButtonClick() {
    this.telemetryService
      .deleteEvents(this.route.snapshot.paramMap.get('id'))
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => this.ngOnInit());
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
