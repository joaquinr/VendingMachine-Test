import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VendingMachineClient, VendingMachineData } from '../services/VendingMachineService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private serviceClient: VendingMachineClient;
  public machineData: VendingMachineData;

  constructor(vendingMachineClient: VendingMachineClient) {
    this.serviceClient = vendingMachineClient;
    this.refreshData();
    console.log(this.machineData);
  }

  private refreshData() {
    this.serviceClient.getMachineState().subscribe(data => {
      this.machineData = data;
    });
  }
}
