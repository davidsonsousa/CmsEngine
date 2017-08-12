import { Component, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { ToastyService } from 'ng2-toasty';

import { CategoryService } from '../../../services/category.service';
import { ToastType } from '../../../models/index';

@Component({
  selector: 'cms-category',
  templateUrl: './category.component.html',
  providers: [CategoryService]
})
export class CategoryComponent implements AfterViewInit {
  public categories = [];
  public columns = [];
  public vanityId: string;

  constructor(
    private categoryService: CategoryService,
    private toastyService: ToastyService
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public onDeleteCategory(vanityId: string) {
    this.categoryService.delete(vanityId)
      .subscribe(response => {
        this.categoryService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, err => {
        this.categoryService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.categoryService.get()
      .subscribe(categories => {
        this.categories = categories;
        this.columns = this.categoryService.extractProperties(this.categories[0]);
      }, err => {
        this.categoryService.showToast(ToastType.Error, err.message);
      });
  }
}
