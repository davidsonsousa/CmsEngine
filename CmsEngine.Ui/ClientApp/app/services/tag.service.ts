import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { Service } from './service';
import { TagEditModel } from '../models/index';

@Injectable()
export class TagService extends Service {
  constructor(
    private protocol: Http,
    private toastySvc: ToastyService,
    private routing: Router
  ) {
    super(protocol, 'api/tag', toastySvc, routing);
  }

  /**
   * Create a new tag
   * @param tagEditModel
   */
  public create(tagEditModel: TagEditModel): any {
    return this.post(tagEditModel);
  }

  /**
   * Update a specific tag
   * @param tagEditModel
   */
  public update(tagEditModel: TagEditModel): any {
    return this.put(tagEditModel.vanityId, tagEditModel);
  }
}