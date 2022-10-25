import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AlertModule, AlertConfig } from 'ngx-bootstrap/alert';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { AppComponent } from './app.component';
import { ReportsComponent } from './reports/reports.component';
import { BsDatepickerModule, BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgSelectModule } from '@ng-select/ng-select';
import { KeysPipe } from './keys.pipe';

@NgModule({
  declarations: [
    AppComponent,
    ReportsComponent,
    KeysPipe
  ],
  imports: [
    BrowserModule, HttpClientModule, FormsModule, BrowserAnimationsModule, AccordionModule, CarouselModule, CollapseModule, NgSelectModule,
    AlertModule,
    ButtonsModule,
    BsDatepickerModule.forRoot(),
    TooltipModule.forRoot()

  ],
  providers: [BsDatepickerConfig, AlertConfig],
  bootstrap: [AppComponent]
})
export class AppModule { }
