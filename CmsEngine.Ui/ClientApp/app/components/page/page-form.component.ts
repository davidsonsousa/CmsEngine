import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { PageService } from '../../services/page.service';
import { PageEditModel, ToastType } from '../../models/index';
import { DocumentStatus } from '../../models/enums';

@Component({
  selector: 'cms-page-form',
  templateUrl: './page-form.component.html',
  providers: [PageService]
})
export class PageFormComponent implements OnInit {
  public pageEditModel: PageEditModel = {
    id: 0,
    vanityId: '',
    title: '',
    slug: '',
    description: '',
    documentContent: '',
    author: '',
    status: DocumentStatus.Draft
  };

  constructor(
    private pageService: PageService,
    private toastyService: ToastyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    activatedRoute.params.subscribe(p => {
      this.pageEditModel.vanityId = p['id'];
    });
  }

  public ngOnInit(): void {
    if (this.pageEditModel.vanityId) {
      this.pageService.get(this.pageEditModel.vanityId)
        .subscribe((page: any) => {
          this.pageEditModel = page;
        });
    }
  }

  public onSubmit() {
    if (this.pageEditModel.id || this.pageEditModel.vanityId) {
      this.pageService.update(this.pageEditModel)
        .subscribe((response: any) => {
          this.pageService.showToastAndRedirect('/pages', ToastType.Success, response.message);
        }, (err: any) => {
          this.pageService.showToast(ToastType.Error, err.message);
        });
    } else {
      this.pageService.create(this.pageEditModel)
        .subscribe((response: any) => {
          this.pageService.showToastAndRedirect('/pages', ToastType.Success, response.message);
        }, (err: any) => {
          this.pageService.showToast(ToastType.Error, err.message);
        });
    }
  }
}
