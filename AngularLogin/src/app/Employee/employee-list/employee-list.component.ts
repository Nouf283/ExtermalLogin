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
   this.employeeService.getAllEmployees().subscribe((response) => {
     //Code will execute when back-end will respond
     console.log(response);
     this.employees = response;
   });
 }

}
