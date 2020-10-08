import { Component, OnInit } from '@angular/core';
import {AppInfo, TelemetryService,} from './telemetry.service'
import {SelectionModel} from '@angular/cdk/collections';
import { from } from 'rxjs';

@Component({
  selector: 'app-telemetry',
  templateUrl: './telemetry.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry.component.less']
})
export class TelemetryComponent implements OnInit {

  id: string
  stats : AppInfo[]
  displayedColumns: string[] = ['userName', 'lastUpdatedAt', 'appVersion', 'osName'];

  selection = new SelectionModel<AppInfo>(true, []);

  constructor(private telemetryService: TelemetryService) {

   }

  ngOnInit(): void {
    this.showStats();
  }

  showStats() {
    this.telemetryService.getAllAppInfos()
      .subscribe((data: AppInfo[]) => {
        //console.log(data);
        this.stats = data
      });
  }

  clear() {
    this.stats = undefined;
  }

    setId(row?: AppInfo) {
      this.id = row.id
      console.log(this.id)
    }

}
