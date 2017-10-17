import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { CategoryService } from '../../services/category.service';
import { ToastType, DataTableViewModel } from '../../models/index';

@Component({
  selector: 'cms-category-list',
  templateUrl: './category-list.component.html',
  providers: [CategoryService]
})
export class CategoryListComponent implements AfterViewInit {
  public dataTable: DataTableViewModel;
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
    this.categoryService.getDataTable()
      .subscribe((dataTable: DataTableViewModel) => {
        this.dataTable = dataTable;
      }, (err: any) => {
        this.categoryService.showToast(ToastType.Error, err.message);
      });
  }
}
