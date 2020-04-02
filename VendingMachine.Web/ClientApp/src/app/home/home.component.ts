import { Component, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VendingMachineClient, VendingMachineData, ESellProductStatus } from '../services/VendingMachineService';
import { Observable, BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private serviceClient: VendingMachineClient;
  public machineData: VendingMachineData;
  public coinToInsert: number;
  private alerts: string[];
  private behaviorSubjectQueue: BehaviorSubject<string[]> = new BehaviorSubject<string[]>(this.alerts);

  close(alert: string) {
    this.alerts.splice(this.alerts.indexOf(alert), 1);
  }

  getInstance() {
    return this.behaviorSubjectQueue;
  }

  constructor(vendingMachineClient: VendingMachineClient) {
    this.serviceClient = vendingMachineClient;
    this.alerts = new Array<string>();

    this.refreshData();
  }

  onPurchaseProduct(productName: string) {
    this.serviceClient.sellProduct(productName).subscribe(result => {
      if (result.status == ESellProductStatus.Success) {
        this.alerts.push("Purchased " + productName);
        if (result.change) {
          this.alerts.push("Received change: " + JSON.stringify(result.change));
        }
      }
      switch (result.status) {
        case ESellProductStatus.InsufficientFunds:
          this.alerts.push("Sorry, you need to insert more coins for this item")
          break;
        case ESellProductStatus.OutOfChange:
          this.alerts.push("Sorry, we're out of change. You can still choose an item for the exact amount you inserted")
          break;
        case ESellProductStatus.SoldOut:
          this.alerts.push("Sorry, we're out of this item. Please choose something else")
          break;
        default:
      }
      this.refreshData();
    })
    
    
  }
  onInsertCoin(coinDenomination: number) {
    this.serviceClient.insertCoin(coinDenomination).subscribe(result =>
    {
      if (result) {
        this.refreshData();
        this.alerts.push("Inserted coin " + coinDenomination);
      }
    }, error => {
        if (error.response === "false") {
          this.alerts.push("Sorry, that is not a valid coin denomination. Please choose a euro coin");
        };
    });    
  }
  onReturnCoins() {
    this.serviceClient.returnCoins().subscribe(result => {
      this.refreshData();
      this.alerts.push("Returned coins: " + JSON.stringify(result));
    })
    
  }

  private refreshData() {
    this.serviceClient.getMachineState().subscribe(data => {
      this.machineData = data;
    });
  }
}
