import { Component } from '@angular/core';
import {Employee} from "../../Models/employee";
import {EmployeeService} from "../../services/employee.service";

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent {
 public employees:  Employee[]=[];
 constructor(private  employeeService: EmployeeService){

 }
 ngOninit():void{
   this.employeeService.getAllEmployees().subscribe({
     next(position) {
       console.log('Current Position: ', position);
     },
     error(msg) {
       console.log('Error Getting Location: ', msg);
     }
   });
 }

}
