import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { Service } from './service';
import { PageEditModel } from '../models/index';

@Injectable()
export class PageService extends Service {
  constructor(
    private protocol: Http,
    private toastySvc: ToastyService,
    private routing: Router
  ) {
    super(protocol, 'api/page', toastySvc, routing);
  }

  /**
   * Create a new page
   * @param pageEditModel
   */
  public create(pageEditModel: PageEditModel): any {
    return this.post(pageEditModel);
  }

  /**
   * Update a specific page
   * @param pageEditModel
   */
  public update(pageEditModel: PageEditModel): any {
    return this.put(pageEditModel.vanityId, pageEditModel);
  }
}