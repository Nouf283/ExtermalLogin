import { Component } from '@angular/core';
import {Employee} from "../../Models/employee";

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent {
 employees:  Employee[]=[];

}
