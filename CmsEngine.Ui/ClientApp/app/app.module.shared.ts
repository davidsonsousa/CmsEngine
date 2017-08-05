import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

import {
  // Core
  CategoryComponent, PageComponent, PostComponent, TagComponent,
  WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent,
  // Shared
  ListComponent
} from './components/cms/index';

export const sharedConfig: NgModule = {
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent,
    HomeComponent,
    // Core
    CategoryComponent,
    PageComponent,
    PostComponent,
    TagComponent,
    WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent,
    // Shared
    ListComponent
  ],
  imports: [
    FormsModule,
    HttpModule,
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
      { path: 'websites/new', component: WebsiteNewComponent },
      { path: 'websites/edit/:id', component: WebsiteEditComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ]
};
