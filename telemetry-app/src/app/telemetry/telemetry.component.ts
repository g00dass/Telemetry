import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { AppInfo, StatisticsEventType, TelemetryService } from './telemetry.service'
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, interval, Observable, Subject } from 'rxjs';
import { filter, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
    private router: Router,
    public dialog: MatDialog
  ) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
      width: '100%',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
  
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

@Component({
  selector: 'event-types-dialog',
  templateUrl: 'event-types-dialog.html',
  providers: [TelemetryService],
})

export class DialogOverviewExampleDialog {

  eventTypes$ : Observable<StatisticsEventType[]>
  displayedColumns: string[] = ['name', 'description'];

  constructor(
    private telemetryService: TelemetryService,
    private router: Router,
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit() : void {
    this.eventTypes$ = this.telemetryService.getEventTypes();
  }

  onOkClick(): void {
    this.dialogRef.close();
  }

  addType(type : string, description : string) {
    this.telemetryService
      .addEventTypes([{name : type, description : description}])
      .subscribe(_ => {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false; 
          this.router.navigate([this.router.url]);
      })
  }
}
