import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { PageService } from '../../services/page.service';
import { ToastType, DataTableViewModel } from '../../models/index';

@Component({
  selector: 'cms-page-list',
  templateUrl: './page-list.component.html',
  providers: [PageService]
})
export class PageListComponent implements AfterViewInit {
  public dataTable: DataTableViewModel;
  public vanityId: string;

  constructor(
    private pageService: PageService,
    private toastyService: ToastyService
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public onDeletePage(vanityId: string) {
    this.pageService.delete(vanityId)
      .subscribe((response: any) => {
        this.pageService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, (err: any) => {
        this.pageService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.pageService.getDataTable()
      .subscribe((dataTable: DataTableViewModel) => {
        this.dataTable = dataTable;
      }, (err: any) => {
        this.pageService.showToast(ToastType.Error, err.message);
      });
  }
}
