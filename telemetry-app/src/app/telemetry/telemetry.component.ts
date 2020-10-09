import { Component, OnInit } from '@angular/core';
import {AppInfo, TelemetryService,} from './telemetry.service'
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-telemetry',
  templateUrl: './telemetry.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry.component.less']
})

export class TelemetryComponent implements OnInit {
  stats$: Observable<any>;
  stats : AppInfo[]
  displayedColumns: string[] = ['userName', 'lastUpdatedAt'];

  constructor(
    private telemetryService: TelemetryService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.stats$ = this.route.paramMap.pipe(
      switchMap(params => {
        return this.telemetryService.getAllAppInfos()
      })
    );
  }

  setId(row?: AppInfo) {
    this.router.navigate(['/details/' + row.id]);
  }
}
