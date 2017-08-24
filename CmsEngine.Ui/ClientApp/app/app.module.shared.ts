import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { ToastyModule } from 'ng2-toasty';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

// Core
import {
  CategoryComponent, CategoryNewComponent, CategoryEditComponent, CategoryFormComponent,
  PageComponent, PageNewComponent, PageEditComponent, PageFormComponent,
  PostComponent, PostNewComponent, PostEditComponent, PostFormComponent,
  TagComponent, TagNewComponent, TagEditComponent, TagFormComponent,
  WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent,
} from './components/cms/index';

// Shared
import {
  ListComponent, LoadComponent
} from './components/cms/shared/index';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    // Core
    CategoryComponent, CategoryNewComponent, CategoryEditComponent, CategoryFormComponent,
    PageComponent, PageNewComponent, PageEditComponent, PageFormComponent,
    PostComponent, PostNewComponent, PostEditComponent, PostFormComponent,
    TagComponent, TagNewComponent, TagEditComponent, TagFormComponent,
    WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent,
    // Shared
    ListComponent, LoadComponent
  ],
  imports: [
    CommonModule,
    HttpModule,
    FormsModule,
    ToastyModule.forRoot(),
    RouterModule.forRoot([
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'categories', component: CategoryComponent },
      { path: 'categories/new', component: CategoryNewComponent },
      { path: 'categories/edit/:id', component: CategoryEditComponent },
      { path: 'pages', component: PageComponent },
      { path: 'pages/new', component: PageNewComponent },
      { path: 'pages/edit/:id', component: PageEditComponent },
      { path: 'posts', component: PostComponent },
      { path: 'posts/new', component: PostNewComponent },
      { path: 'posts/edit/:id', component: PostEditComponent },
      { path: 'tags', component: TagComponent },
      { path: 'tags/new', component: TagNewComponent },
      { path: 'tags/edit/:id', component: TagEditComponent },
      { path: 'websites', component: WebsiteComponent },
      { path: 'websites/new', component: WebsiteNewComponent },
      { path: 'websites/edit/:id', component: WebsiteEditComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ]
})
export class AppModuleShared {
}
