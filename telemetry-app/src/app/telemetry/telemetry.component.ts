import { Component, OnInit } from '@angular/core';
import {AppInfo, TelemetryService,} from './telemetry.service'
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, interval, Observable, Subject } from 'rxjs';
import { filter, startWith, switchMap, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-telemetry',
  templateUrl: './telemetry.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry.component.less']
})

export class TelemetryComponent implements OnInit {
  destroy$: Subject<boolean> = new Subject<boolean>();
  displayedColumns: string[] = ['userName', 'lastUpdatedAt'];
  isAutoRefreshEnabled = false
  periods = [5, 15, 30, 60, 300]
  autoRefreshPeriod = 30
  selectedId = null

  stats$ = new BehaviorSubject<AppInfo[]>([]);
  intervalSubject = new BehaviorSubject<number>(0);

  constructor(
    private telemetryService: TelemetryService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.intervalSubject.next(this.autoRefreshPeriod * 1000)

    let interval$ = this.intervalSubject.pipe(switchMap(x => interval(x)))

    combineLatest(
      [
        interval$.pipe(startWith(0), filter(x => this.isAutoRefreshEnabled || x == 0)),
        this.route.paramMap
      ]
    ).pipe(
        switchMap(_ => {
          return this.telemetryService.getAllAppInfos()
        }),
        takeUntil(this.destroy$)
    ).subscribe(x => this.stats$.next(x));
  }

  setId(row?: AppInfo) {
    this.selectedId = row.id
    this.router.navigate(['/details/' + row.id]);
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

  onPeriodChanged(value) {
    this.intervalSubject.next(this.autoRefreshPeriod * 1000)
  }
}
