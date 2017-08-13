import { Component, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { ToastyService } from 'ng2-toasty';

import { PageService } from '../../../services/page.service';
import { ToastType } from '../../../models/index';

@Component({
  selector: 'cms-page',
  templateUrl: './page.component.html',
  providers: [PageService]
})
export class PageComponent implements AfterViewInit {
  public pages = [];
  public columns = [];
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
      .subscribe(response => {
        this.pageService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, err => {
        this.pageService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.pageService.get()
      .subscribe(pages => {
        this.pages = pages;
        this.columns = this.pageService.extractProperties(this.pages[0]);
      }, err => {
        this.pageService.showToast(ToastType.Error, err.message);
      });
  }
}
