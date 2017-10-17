import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { TagService } from '../../services/tag.service';
import { TagEditModel, ToastType } from '../../models/index';

@Component({
  selector: 'cms-tag-form',
  templateUrl: './tag-form.component.html',
  providers: [TagService]
})
export class TagFormComponent implements OnInit {
  public tagEditModel: TagEditModel = {
    id: 0,
    vanityId: '',
    name: '',
    slug: ''
  };

  constructor(
    private tagService: TagService,
    private toastyService: ToastyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    activatedRoute.params.subscribe(p => {
      this.tagEditModel.vanityId = p['id'];
    });
  }

  public ngOnInit(): void {
    if (this.tagEditModel.vanityId) {
      this.tagService.get(this.tagEditModel.vanityId)
        .subscribe((tag: any) => {
          this.tagEditModel = tag;
        });
    }
  }

  public onSubmit() {
    if (this.tagEditModel.id || this.tagEditModel.vanityId) {
      this.tagService.update(this.tagEditModel)
        .subscribe((response: any) => {
          this.tagService.showToastAndRedirect('/tags', ToastType.Success, response.message);
        }, (err: any) => {
          this.tagService.showToast(ToastType.Error, err.message);
        });
    } else {
      this.tagService.create(this.tagEditModel)
        .subscribe((response: any) => {
          this.tagService.showToastAndRedirect('/tags', ToastType.Success, response.message);
        }, (err: any) => {
          this.tagService.showToast(ToastType.Error, err.message);
        });
    }
  }
}
