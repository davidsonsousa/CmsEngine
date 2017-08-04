import { Component, Input } from '@angular/core';

@Component({
  selector: 'cms-list',
  templateUrl: './list.component.html'
})
export class ListComponent {
  @Input() tableColumns = [];
  @Input() tableItems = [];

  get keys() {
    return this.tableColumns;
  }

  get items() {
    return this.tableItems;
  }
}
