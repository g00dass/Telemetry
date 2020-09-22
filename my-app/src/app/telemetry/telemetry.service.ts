import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

export interface StatInfo {
    clientId : ClientId
    data : Data
    lastUpdatedAt : Date
}

export interface ClientId {
    id : string
    userName : string
    osName : string
}

export interface Data {
    appVersion : string
}

@Injectable()
export class TelemetryService {
    telemetryUrl = 'https://localhost:32774/statistics/';

    constructor(private http: HttpClient) { }

    getStats() {
        return this.http.get(this.telemetryUrl + 'allStats');
  }
}