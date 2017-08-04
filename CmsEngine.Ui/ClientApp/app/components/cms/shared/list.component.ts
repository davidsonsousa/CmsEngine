import { Component, Input } from '@angular/core';

@Component({
  selector: 'cms-list',
  templateUrl: './list.component.html'
})
export class ListComponent {
  @Input() columns = [];
  @Input() items = [];
  public itemKeys = [];

  ngOnInit() {
    let p = this.items[0];

    for (var key in p) {
      if (p.hasOwnProperty(key)) {
        this.itemKeys.push(key);
      }
    }
  }
}
