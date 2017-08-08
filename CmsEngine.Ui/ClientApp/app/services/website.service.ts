import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { Service } from './service';
import { WebsiteEditModel } from '../models/index';

@Injectable()
export class WebsiteService extends Service {
  constructor(
    private protocol: Http,
    private toastySvc: ToastyService,
    private routing: Router
  ) {
    super(protocol, 'api/website', toastySvc, routing);
  }

  /**
   * Create a new website
   * @param websiteEditModel
   */
  public create(websiteEditModel: WebsiteEditModel): any {
    return this.post(websiteEditModel);
  }

  /**
   * Update a specific website
   * @param websiteEditModel
   */
  public update(websiteEditModel: WebsiteEditModel): any {
    return this.put(websiteEditModel.vanityId, websiteEditModel);
  }
}