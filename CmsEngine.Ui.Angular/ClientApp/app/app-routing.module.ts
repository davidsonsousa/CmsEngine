import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
// Core
import {
  CategoryComponent, CategoryListComponent, CategoryNewComponent, CategoryEditComponent, CategoryFormComponent,
  PageComponent, PageListComponent, PageNewComponent, PageEditComponent, PageFormComponent,
  PostComponent, PostListComponent, PostNewComponent, PostEditComponent, PostFormComponent,
  TagComponent, TagListComponent, TagNewComponent, TagEditComponent, TagFormComponent,
  WebsiteComponent, WebsiteNewComponent, WebsiteEditComponent, WebsiteFormComponent, WebsiteListComponent,
} from './components/index';

// Layouts
import { FullLayoutComponent } from './components/layouts/full-layout.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: '',
    component: FullLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'categories', component: CategoryComponent, children: [
          { path: '', redirectTo: 'list', pathMatch: 'full' },
          { path: 'list', component: CategoryListComponent },
          { path: 'new', component: CategoryNewComponent },
          { path: 'edit/:id', component: CategoryEditComponent },
        ]
      },
      {
        path: 'pages', component: PageComponent, children: [
          { path: '', redirectTo: 'list', pathMatch: 'full' },
          { path: 'list', component: PageListComponent },
          { path: 'new', component: PageNewComponent },
          { path: 'edit/:id', component: PageEditComponent },
        ]
      },
      {
        path: 'posts', component: PostComponent, children: [
          { path: '', redirectTo: 'list', pathMatch: 'full' },
          { path: 'list', component: PostListComponent },
          { path: 'new', component: PostNewComponent },
          { path: 'edit/:id', component: PostEditComponent },
        ]
      },
      {
        path: 'tags', component: TagComponent, children: [
          { path: '', redirectTo: 'list', pathMatch: 'full' },
          { path: 'list', component: TagListComponent },
          { path: 'new', component: TagNewComponent },
          { path: 'edit/:id', component: TagEditComponent },
        ]
      },
      {
        path: 'websites', component: WebsiteComponent, children: [
          { path: '', redirectTo: 'list', pathMatch: 'full' },
          { path: 'list', component: WebsiteListComponent },
          { path: 'new', component: WebsiteNewComponent },
          { path: 'edit/:id', component: WebsiteEditComponent },
        ]
      }
    ]
  },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
