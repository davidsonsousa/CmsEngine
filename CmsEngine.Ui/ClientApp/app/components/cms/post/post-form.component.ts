import { Component, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { PostService } from '../../../services/post.service';
import { PostEditModel, ToastType } from '../../../models/index';
import { DocumentStatus } from '../../../models/enums';

@Component({
  selector: 'cms-post-form',
  templateUrl: './post-form.component.html',
  providers: [PostService]
})
export class PostFormComponent implements OnInit {
  public postEditModel: PostEditModel = {
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
    private postService: PostService,
    private toastyService: ToastyService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    activatedRoute.params.subscribe(p => {
      this.postEditModel.vanityId = p["id"];
    });
  }

  public ngOnInit(): void {
    if (this.postEditModel.vanityId) {
      this.postService.get(this.postEditModel.vanityId)
        .subscribe((post: any) => {
          this.postEditModel = post;
        });
    }
  }

  public onSubmit() {
    if (this.postEditModel.id || this.postEditModel.vanityId) {
      this.postService.update(this.postEditModel)
        .subscribe((response: any) => {
          this.postService.showToastAndRedirect('/posts', ToastType.Success, response.message);
        }, (err: any) => {
          this.postService.showToast(ToastType.Error, err.message);
        });
    } else {
      this.postService.create(this.postEditModel)
        .subscribe((response: any) => {
          this.postService.showToastAndRedirect('/posts', ToastType.Success, response.message);
        }, (err: any) => {
          this.postService.showToast(ToastType.Error, err.message);
        });
    }
  }
}
