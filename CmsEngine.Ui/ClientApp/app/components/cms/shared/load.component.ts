import {
  Router,
  // import as RouterEvent to avoid confusion with the DOM Event
  Event as RouterEvent,
  NavigationStart,
  NavigationEnd,
  NavigationCancel,
  NavigationError
} from '@angular/router'
import { Component, Input } from '@angular/core';


@Component({
  selector: 'cms-load',
  templateUrl: './load.component.html'
})
export class LoadComponent {
  @Input() loading = true;

  get isLoaded() {
    return this.loading;
  }
}