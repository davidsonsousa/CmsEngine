import { Component, AfterViewInit } from '@angular/core';
import { Router } from "@angular/router";
import { ToastyService } from 'ng2-toasty';

import { WebsiteService } from '../../../services/website.service';

@Component({
  selector: 'cms-website',
  templateUrl: './website.component.html',
  providers: [WebsiteService]
})
export class WebsiteComponent implements AfterViewInit {
  public websites = [];
  public columns = [];
  public vanityId: string;

  constructor(
    private websiteService: WebsiteService,
    private toastyService: ToastyService,
    private router: Router
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public loadData() {
    this.websiteService.get()
      .subscribe(websites => {
        this.websites = websites;
        this.columns = this.websiteService.extractProperties(this.websites[0]);
      });
  }

  public onDeleteWebsite(vanityId: string) {
    this.websiteService.delete(vanityId)
      .subscribe(response => {
        this.toastyService.success({
          title: 'Success',
          msg: response.message,
          theme: 'bootstrap',
          showClose: true,
          timeout: 10000
        });

        this.loadData();
      });
  }
}
