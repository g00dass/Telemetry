import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface AppInfo {
    id : string
    appVersion : string
    userName : string
    osName : string
    lastUpdatedAt : Date
}

export interface StatisticsEvent {
    date : Date
    name : string
    description : string
}

@Injectable()
export class TelemetryService {
    telemetryUrl = environment.statisticsApiUrl;

    constructor(private http: HttpClient) { 
        console.log(this.telemetryUrl)
    }

    getAllAppInfos() {
        return this.http.get(this.telemetryUrl + 'appInfo/all');
    }

    getAppInfo(id: string) {
        return this.http.get(this.telemetryUrl + 'appInfo/' + id);
    }

    getEvents(id: string) {
        return this.http.get(this.telemetryUrl + 'appInfo/' + id + '/events-history');
    }
}