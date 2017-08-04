import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

import { Service } from './service';

@Injectable()
export class WebsiteService extends Service {
  private readonly endpoint = 'api/website';

  constructor(private http: Http) { super(); }

  public getWebsites() {
    return this.http.get(this.endpoint)
      .map(res => res.json());
  }
}