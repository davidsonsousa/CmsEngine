import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { WebsiteService } from '../../../services/website.service';
import { ToastType } from '../../../models/index';

@Component({
  selector: 'cms-website',
  templateUrl: './website.component.html',
  providers: [WebsiteService]
})
export class WebsiteComponent implements AfterViewInit {
  public websites: string[] = [];
  public columns: string[] = [];
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
    this.websiteService.get()
      .subscribe((websites: any) => {
        this.websites = websites;
        this.columns = this.websiteService.extractProperties(this.websites[0]);
      }, (err: any) => {
        this.websiteService.showToast(ToastType.Error, err.message);
      });
  }
}
