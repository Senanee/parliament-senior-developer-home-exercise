import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoadingComponent } from './components/loading/loading.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { SidebarComponent } from './components/layout/sidebar/sidebar.component';
import { MainLayoutComponent } from './components/layout/main-layout/main-layout.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { MaxDateValidator } from './validations/max-date-validator';

@NgModule({
  declarations: [
    PageNotFoundComponent,
    FooterComponent,
    HeaderComponent,
    SidebarComponent, 
    LoadingComponent,
    MainLayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports:[
    PageNotFoundComponent,
    FooterComponent,
    HeaderComponent,
    SidebarComponent, 
    LoadingComponent,
    MainLayoutComponent,
  ]
})
export class SharedModule { }
