import { Component, OnInit } from '@angular/core';
import {StatInfo, TelemetryService} from './telemetry.service'

@Component({
  selector: 'app-telemetry',
  templateUrl: './telemetry.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry.component.less']
})
export class TelemetryComponent implements OnInit {

  stats : StatInfo[]
  displayedColumns: string[] = ['id.deviceId', 'statistics.userName', 'lastUpdatedAt', 'statistics.appVersion', 'statistics.osName'];
  constructor(private telemetryService: TelemetryService) { }

  ngOnInit(): void {
  }

  showStats() {
    this.telemetryService.getStats()
      .subscribe((data: StatInfo[]) => {
        console.log(data);
        this.stats = data
      });
  }

  clear() {
    this.stats = undefined;
  }

}
