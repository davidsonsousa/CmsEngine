import { Component, AfterViewInit } from '@angular/core';
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

  constructor(private websiteService: WebsiteService) { }

  public ngAfterViewInit(): void {
    this.websiteService.get()
      .subscribe(websites => {
        this.websites = websites;
        this.columns = this.websiteService.extractProperties(this.websites[0]);
      });
  }

  public onDeleteWebsite(vanityId: string) {
    this.websiteService.delete(vanityId)
      .subscribe(response => {
        console.log(response);
      });
  }
}
