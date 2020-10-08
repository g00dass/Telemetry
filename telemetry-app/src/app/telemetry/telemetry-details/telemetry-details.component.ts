import { Component, Input, OnInit } from '@angular/core';
import {AppInfo, StatisticsEvent, TelemetryService} from '../telemetry.service'
import { SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-telemetry-details',
  templateUrl: './telemetry-details.component.html',
  providers: [TelemetryService],
  styleUrls: ['./telemetry-details.component.less']
})
export class TelemetryDetailsComponent implements OnInit {
  
  @Input() id : string
  
  appInfo : AppInfo
  events : StatisticsEvent[]
  displayedColumns: string[] = ['name', 'date', 'description'];

  constructor(private telemetryService: TelemetryService) { }

  ngOnInit(): void {
  }
  
  ngOnChanges(changes: SimpleChanges) {
    this.showEvents(this.id)
    this.showAppInfo(this.id)
  }
  
  showEvents(id : string) {
    //console.log(id)
    this.telemetryService.getEvents(id)
      .subscribe((data: StatisticsEvent[]) => {
        console.log(data);
        this.events = data
      });
  }

  showAppInfo(id: string) {
    this.telemetryService.getAppInfo(id)
    .subscribe((data: AppInfo) => {
      console.log(data);
      this.appInfo = data
    });
  }

  clear() {
    this.events = undefined;
  }
}
