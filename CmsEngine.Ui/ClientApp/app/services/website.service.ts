import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { Service } from './service';
import { WebsiteEditModel } from '../models/website-editmodel';

@Injectable()
export class WebsiteService extends Service {
  constructor(private protocol: Http) {
    super(protocol, 'api/website');
  }

  public create(websiteEditModel: WebsiteEditModel): any {
    return this.post(websiteEditModel);
  }

  public update(websiteEditModel: WebsiteEditModel): any {
    return this.put(websiteEditModel.vanityId, websiteEditModel);
  }
}