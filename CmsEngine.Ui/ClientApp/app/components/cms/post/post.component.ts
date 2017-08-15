import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { PostService } from '../../../services/post.service';
import { ToastType } from '../../../models/index';

@Component({
  selector: 'cms-post',
  templateUrl: './post.component.html',
  providers: [PostService]
})
export class PostComponent implements AfterViewInit {
  public posts: string[] = [];
  public columns: string[] = [];
  public vanityId: string;

  constructor(
    private postService: PostService,
    private toastyService: ToastyService
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public onDeletePost(vanityId: string) {
    this.postService.delete(vanityId)
      .subscribe((response: any) => {
        this.postService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, (err: any) => {
        this.postService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.postService.get()
      .subscribe((posts: any) => {
        this.posts = posts;
        this.columns = this.postService.extractProperties(this.posts[0]);
      }, (err: any) => {
        this.postService.showToast(ToastType.Error, err.message);
      });
  }
}
