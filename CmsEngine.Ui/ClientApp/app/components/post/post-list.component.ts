import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { PostService } from '../../services/post.service';
import { ToastType, DataTableViewModel } from '../../models/index';

@Component({
  selector: 'cms-post-list',
  templateUrl: './post-list.component.html',
  providers: [PostService]
})
export class PostListComponent implements AfterViewInit {
  public dataTable: DataTableViewModel;
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
    this.postService.getDataTable()
      .subscribe((dataTable: DataTableViewModel) => {
        this.dataTable = dataTable;
      }, (err: any) => {
        this.postService.showToast(ToastType.Error, err.message);
      });
  }
}
