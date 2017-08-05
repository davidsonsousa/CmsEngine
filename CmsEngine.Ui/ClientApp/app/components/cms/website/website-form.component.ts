import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { WebsiteService } from '../../../services/website.service';
import { WebsiteEditModel } from '../../../models/website-editmodel';

@Component({
  selector: 'cms-website-form',
  templateUrl: './website-form.component.html',
  providers: [WebsiteService]
})
export class WebsiteFormComponent implements OnInit {
  public websiteEditModel: WebsiteEditModel = {
    id: 0,
    vanityId: '',
    name: '',
    description: '',
    culture: '',
    urlFormat: '',
    dateFormat: '',
    siteUrl: ''
  };

  constructor(private websiteService: WebsiteService, private route: ActivatedRoute) {
    route.params.subscribe(p => {
      this.websiteEditModel.vanityId = p["id"];
    });
  }

  public ngOnInit(): void {
    this.websiteService.get(this.websiteEditModel.vanityId)
      .subscribe(website => {
        this.websiteEditModel = website;
      });
  }

  public onSubmit() {
    this.websiteService.update(this.websiteEditModel)
      .subscribe(response => {
        alert(this.websiteEditModel.name + ' saved!');
      });
  }
}
