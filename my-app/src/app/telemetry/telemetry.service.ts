import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface StatInfo {
    id : AppId
    statistics : AppStatistics
    lastUpdatedAt : Date
}

export interface AppId {
    deviceId : string
}

export interface AppStatistics {
    appVersion : string
    userName : string
    osName : string
}

@Injectable()
export class TelemetryService {
    telemetryUrl = environment.statisticsApiUrl;

    constructor(private http: HttpClient) { 
        console.log(this.telemetryUrl)
    }

    getStats() {
        return this.http.get(this.telemetryUrl + 'appInfo/all');
  }
}