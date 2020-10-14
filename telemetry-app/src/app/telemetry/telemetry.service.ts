import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

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

    getAllAppInfos() : Observable<AppInfo[]>{
        return this.http.get<AppInfo[]>(this.telemetryUrl + 'appInfo/all');
    }

    getAppInfo(id: string) : Observable<AppInfo> {
        return this.http.get<AppInfo>(this.telemetryUrl + 'appInfo/' + id);
    }

    getEvents(id: string) : Observable<StatisticsEvent[]> {
        return this.http.get<StatisticsEvent[]>(this.telemetryUrl + 'appInfo/' + id + '/events-history');
    }
}