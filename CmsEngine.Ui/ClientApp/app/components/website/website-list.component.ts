import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { WebsiteService } from '../../services/website.service';
import { ToastType, DataTableViewModel } from '../../models/index';

@Component({
  selector: 'cms-website-list',
  templateUrl: './website-list.component.html',
  providers: [WebsiteService]
})
export class WebsiteListComponent implements AfterViewInit {
  public dataTable: DataTableViewModel;
  public vanityId: string;

  constructor(
    private websiteService: WebsiteService,
    private toastyService: ToastyService
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public onDeleteWebsite(vanityId: string): void {
    this.websiteService.delete(vanityId)
      .subscribe((response: any) => {
        this.websiteService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, (err: any) => {
        this.websiteService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData(): void {
    this.websiteService.getDataTable()
      .subscribe((dataTable: DataTableViewModel) => {
        this.dataTable = dataTable;
      }, (err: any) => {
        this.websiteService.showToast(ToastType.Error, err.message);
      });
  }
}
