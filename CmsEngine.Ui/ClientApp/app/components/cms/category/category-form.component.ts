import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { CategoryService } from '../../../services/category.service';
import { CategoryEditModel, ToastType } from '../../../models/index';

@Component({
  selector: 'cms-category-form',
  templateUrl: './category-form.component.html',
  providers: [CategoryService]
})
export class CategoryFormComponent implements OnInit {
  public categoryEditModel: CategoryEditModel = {
    id: 0,
    vanityId: '',
    name: '',
    slug: '',
    description: ''
  };

  constructor(
    private categoryService: CategoryService,
    private toastyService: ToastyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    activatedRoute.params.subscribe(p => {
      this.categoryEditModel.vanityId = p["id"];
    });
  }

  public ngOnInit(): void {
    if (this.categoryEditModel.vanityId) {
      this.categoryService.get(this.categoryEditModel.vanityId)
        .subscribe(category => {
          this.categoryEditModel = category;
        });
    }
  }

  public onSubmit() {
    if (this.categoryEditModel.id || this.categoryEditModel.vanityId) {
      this.categoryService.update(this.categoryEditModel)
        .subscribe(response => {
          this.categoryService.showToastAndRedirect('/categories', ToastType.Success, response.message);
        }, err => {
          this.categoryService.showToast(ToastType.Error, err.message);
        });
    } else {
      this.categoryService.create(this.categoryEditModel)
        .subscribe(response => {
          this.categoryService.showToastAndRedirect('/categories', ToastType.Success, response.message);
        }, err => {
          this.categoryService.showToast(ToastType.Error, err.message);
        });
    }
  }
}
