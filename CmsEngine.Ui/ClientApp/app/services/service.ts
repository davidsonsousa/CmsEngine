import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

export class Service {

  constructor(private http: Http, private endpoint: string) { }

  public extractProperties(item: any): any[] {
    let propList = [];

    if (item) {
      for (var prop in item) {
        if (item.hasOwnProperty(prop)) {
          propList.push(prop);
        }
      }
    }

    return propList;
  }

  public get(vanityId?: string): any {
    if (vanityId) {
      return this.http.get(this.endpoint + '/' + vanityId)
        .map(res => res.json());
    } else {
      return this.http.get(this.endpoint)
        .map(res => res.json());
    }
  }

  protected put(vanityId: string, item: any): any {
    if (item) {
      return this.http.put(this.endpoint + '/' + vanityId, item)
        .map(res => res.json());
    }
  }

  public delete(vanityId: string): any {
    if (vanityId) {
      return this.http.delete(this.endpoint + '/' + vanityId)
        .map(res => res.json());
    }
    else {
      console.error('No vanityId');
    }
  }
}