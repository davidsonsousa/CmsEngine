import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';

import { TagService } from '../../services/tag.service';
import { ToastType, DataTableViewModel } from '../../models/index';

@Component({
  selector: 'cms-tag-list',
  templateUrl: './tag-list.component.html',
  providers: [TagService]
})
export class TagListComponent implements AfterViewInit {
  public dataTable: DataTableViewModel;
  public vanityId: string;

  constructor(
    private tagService: TagService,
    private toastyService: ToastyService
  ) { }

  public ngAfterViewInit(): void {
    this.loadData();
  }

  public onDeleteTag(vanityId: string) {
    this.tagService.delete(vanityId)
      .subscribe((response: any) => {
        this.tagService.showToast(ToastType.Success, response.message);
        this.loadData();
      }, (err: any) => {
        this.tagService.showToast(ToastType.Error, err.message);
      });
  }

  private loadData() {
    this.tagService.getDataTable()
      .subscribe((dataTable: DataTableViewModel) => {
        this.dataTable = dataTable;
      }, (err: any) => {
        this.tagService.showToast(ToastType.Error, err.message);
      });
  }
}
