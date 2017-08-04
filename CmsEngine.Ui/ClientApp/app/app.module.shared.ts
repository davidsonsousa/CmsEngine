import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

// Core
import {
  CategoryComponent, PageComponent, PostComponent, TagComponent, WebsiteComponent
} from './components/cms/index';

export const sharedConfig: NgModule = {
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent,
    HomeComponent,
    CategoryComponent,
    PageComponent,
    PostComponent,
    TagComponent,
    WebsiteComponent
  ],
  imports: [
    RouterModule.forRoot([
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'categories', component: CategoryComponent },
      { path: 'pages', component: PageComponent },
      { path: 'posts', component: PostComponent },
      { path: 'tags', component: TagComponent },
      { path: 'websites', component: WebsiteComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ]
};
