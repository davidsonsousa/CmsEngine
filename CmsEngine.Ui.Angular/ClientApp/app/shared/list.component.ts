import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DataTableViewModel } from '../models/index';

@Component({
  selector: 'cms-list',
  templateUrl: './list.component.html'
})
export class ListComponent {
  @Input() dataTable: DataTableViewModel;
  @Output() vanityIdToInteract = new EventEmitter<string>();

  get columns() {
    if (this.dataTable) {
      return this.dataTable.columns;
    }
  }

  get rows() {
    if (this.dataTable) {
      return this.dataTable.rows;
    }
  }

  //// TODO: Improve the DocumentStatus replacement
  //// The general idea is to replace the status numbers by their respective names, maybe with colors to better indicate status
  //// The aproach below works, but it's far from the best
  //get items() {
  //  let tempItems = JSON.stringify(this.dataTable.data)
  //    .replace(/"status":0/g, `"status":"Published"`)
  //    .replace(/"status":1/g, `"status":"Pending approval"`)
  //    .replace(/"status":2/g, `"status":"Draft"`);

  //  return JSON.parse(tempItems);
  //}

  public deleteItem(vanityId: string): void {
    if (confirm('Are you sure you want to delete this item?')) {
      this.vanityIdToInteract.emit(vanityId);
    }
  }
}
