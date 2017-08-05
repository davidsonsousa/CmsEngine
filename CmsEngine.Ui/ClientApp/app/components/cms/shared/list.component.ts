import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'cms-list',
  templateUrl: './list.component.html'
})
export class ListComponent {
  @Input() tableColumns = [];
  @Input() tableItems = [];
  @Output() vanityIdToInteract = new EventEmitter<string>();

  get keys() {
    return this.tableColumns;
  }

  get items() {
    return this.tableItems;
  }

  public deleteItem(vanityId: string): void {
    if (confirm('Are you sure you want to delete this item?')) {
      this.vanityIdToInteract.emit(vanityId);
    }
  }
}
