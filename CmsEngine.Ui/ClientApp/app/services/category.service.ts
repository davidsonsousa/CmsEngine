import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { Service } from './service';
import { CategoryEditModel } from '../models/index';

@Injectable()
export class CategoryService extends Service {
  constructor(
    private protocol: Http,
    private toastySvc: ToastyService,
    private routing: Router
  ) {
    super(protocol, 'api/category', toastySvc, routing);
  }

  /**
   * Create a new category
   * @param categoryEditModel
   */
  public create(categoryEditModel: CategoryEditModel): any {
    return this.post(categoryEditModel);
  }

  /**
   * Update a specific category
   * @param categoryEditModel
   */
  public update(categoryEditModel: CategoryEditModel): any {
    return this.put(categoryEditModel.vanityId, categoryEditModel);
  }
}