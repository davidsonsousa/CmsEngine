import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { ToastyModule } from 'ng2-toasty';

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

// Core
import {
  CategoryComponent, PageComponent, PostComponent, TagComponent,
  WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent,
} from './components/cms/index';

// Shared
import {
  ListComponent, LoadComponent
} from './components/cms/shared/index'

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
    ListComponent, LoadComponent
  ],
  imports: [
    FormsModule,
    HttpModule,
    ToastyModule.forRoot(),
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
