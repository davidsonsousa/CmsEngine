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

  constructor(private websiteService: WebsiteService) { }

  ngAfterViewInit(): void {
    this.websiteService.getWebsites()
      .subscribe(websites => {
        this.websites = websites;
        this.columns = this.websiteService.extractProperties(this.websites[0]);
      });
  }
}
