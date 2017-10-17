import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { Service } from './service';
import { PostEditModel } from '../models/index';

@Injectable()
export class PostService extends Service {
  constructor(
    private protocol: Http,
    private toastySvc: ToastyService,
    private routing: Router
  ) {
    super(protocol, 'api/post', toastySvc, routing);
  }

  /**
   * Create a new post
   * @param postEditModel
   */
  public create(postEditModel: PostEditModel): any {
    return this.post(postEditModel);
  }

  /**
   * Update a specific post
   * @param postEditModel
   */
  public update(postEditModel: PostEditModel): any {
    return this.put(postEditModel.vanityId, postEditModel);
  }
}
