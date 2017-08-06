import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

export class Service {

  constructor(private http: Http, private endpoint: string) { }

  /**
   * Extract the property names from an item
   * @param item
   */
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

  /**
   * Get item by vanityId
   * Returns multiple items if vanityId is undefined
   * @param vanityId
   */
  public get(vanityId?: string): any {
    if (vanityId) {
      return this.http.get(this.endpoint + '/' + vanityId)
        .map(res => res.json());
    } else {
      return this.http.get(this.endpoint)
        .map(res => res.json());
    }
  }

  /**
   * Post an item to the API
   * @param item
   */
  protected post(item: any) {
    if (item) {
      return this.http.post(this.endpoint, item)
        .map(res => res.json());
    }
  }

  /**
   * Put an item to the API by its vanityId
   * @param vanityId
   * @param item
   */
  protected put(vanityId: string, item: any): any {
    if (item) {
      return this.http.put(this.endpoint + '/' + vanityId, item)
        .map(res => res.json());
    }
  }

  /**
   * Delete an item by its vanityId
   * @param vanityId
   */
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