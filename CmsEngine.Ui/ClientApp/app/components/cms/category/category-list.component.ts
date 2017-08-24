import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { CategoryService } from '../../../services/category.service';
import { ToastType } from '../../../models/index';

@Component({
  selector: 'cms-category-list',
  templateUrl: './category-list.component.html',
  providers: [CategoryService]
})
export class CategoryListComponent implements AfterViewInit {
  public categories: string[] = [];
  public columns: string[] = [];
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
      .subscribe((response: any) => {
        this.categoryService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, (err: any) => {
        this.categoryService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.categoryService.get()
      .subscribe((categories: any) => {
        this.categories = categories;
        this.columns = this.categoryService.extractProperties(this.categories[0]);
      }, (err: any) => {
        this.categoryService.showToast(ToastType.Error, err.message);
      });
  }
}
