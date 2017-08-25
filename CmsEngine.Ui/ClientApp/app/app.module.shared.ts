import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { ToastyModule } from 'ng2-toasty';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';

// Core
import {
  CategoryComponent, CategoryListComponent, CategoryNewComponent, CategoryEditComponent, CategoryFormComponent,
  PageComponent, PageListComponent, PageNewComponent, PageEditComponent, PageFormComponent,
  PostComponent, PostListComponent, PostNewComponent, PostEditComponent, PostFormComponent,
  TagComponent, TagListComponent, TagNewComponent, TagEditComponent, TagFormComponent,
  WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent, WebsiteListComponent,
} from './components/cms/index';

// Layouts
import { FullLayoutComponent } from './components/layouts/full-layout.component';

// Shared
import {
  ListComponent, LoadComponent
} from './components/cms/shared/index';

// Routing module
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    FullLayoutComponent,
    HomeComponent,
    // Core
    CategoryComponent, CategoryListComponent, CategoryNewComponent, CategoryEditComponent, CategoryFormComponent,
    PageComponent, PageListComponent, PageNewComponent, PageEditComponent, PageFormComponent,
    PostComponent, PostListComponent, PostNewComponent, PostEditComponent, PostFormComponent,
    TagComponent, TagListComponent, TagNewComponent, TagEditComponent, TagFormComponent,
    WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent, WebsiteListComponent,
    // Shared
    ListComponent, LoadComponent
  ],
  imports: [
    CommonModule,
    HttpModule,
    FormsModule,
    ToastyModule.forRoot(),
    AppRoutingModule
  ]
})
export class AppModuleShared {
}
