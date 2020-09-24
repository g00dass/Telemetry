import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

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
    telemetryUrl = 'https://statistics/api/';

    constructor(private http: HttpClient) { }

    getStats() {
        return this.http.get(this.telemetryUrl + 'appInfo/all');
  }
}