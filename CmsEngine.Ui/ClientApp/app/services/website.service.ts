import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

export class WebsiteService {
  private readonly endpoint = 'api/wesbite';

  constructor(private http: Http) { }

  public getWebsites() {
    return this.http.get(this.endpoint)
      .map(res => res.json());
  }
}