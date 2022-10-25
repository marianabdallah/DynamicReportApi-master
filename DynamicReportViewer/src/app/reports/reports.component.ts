import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { InputExecute, SystemReport_s } from '../Models/SystemReport_s';
import { GetSystemReportByIdResponse } from '../Models/SystemReport_s';
import { ExecuteSystemReportByIdRequest } from '../Models/SystemReport_s';
import { ExecuteSystemReportByIdResponse } from '../Models/SystemReport_s';

@Component({
  selector: 'reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {

  public systemReports?: SystemReport_s[];
  public selectedReport = new SystemReport_s();
  public systemReport = new GetSystemReportByIdResponse();
  public reportResponse = new ExecuteSystemReportByIdResponse();

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadReporst();
  }


  loadReporst(): void {
    let API_URL = environment.API_URL + "/SystemReport/GetAllSystemReports";

    this.http.get<SystemReport_s[]>(API_URL).subscribe((result: any) => {
      this.systemReports = result;
    }, error => console.error(error));
  }

  loadInputs(): void {
    let id = this.selectedReport.id;
    let requestURL = environment.API_URL + "/SystemReport/GetSystemReportById/" + id;

    this.http.get<SystemReport_s[]>(requestURL
    ).subscribe((result: any) => {
      this.systemReport = result;
    }, error => console.error(error));
  }

  onChange(report: SystemReport_s) {
    this.selectedReport = report;
  }

  executeReport(): void {
    let API_URL = environment.API_URL + "/SystemReport/ExecuteSystemReportById";

    let body = new ExecuteSystemReportByIdRequest();
    body.reportId = this.selectedReport.id;
    let inputArray: InputExecute[] = [];

    this.systemReport.inputStructure.forEach(function (input) {
      let inputObject = new InputExecute();

      if (input.dataType.toLocaleLowerCase() == "datetime") {
        let date = moment(input.value);
        inputObject.value = date.toISOString();
      } else if (input.dataType.toLocaleLowerCase() == "boolean") {
        inputObject.value = input.value ? "true" : "false";
      } else {
        inputObject.value = String(input.value);
      }

      inputObject.dataType = input.dataType;
      inputObject.name = input.name;
      inputObject.order = input.order;

      inputArray.push(inputObject);

    }); 


    body.inputObject = inputArray;

    this.http.post<ExecuteSystemReportByIdResponse>(API_URL, body).subscribe({
      next: data => {
        this.reportResponse = data;
      },
      error: error => {
        console.error('There was an error!', error.message);
      }
    })

  }
  title = 'DynamicReportViewer';

}


