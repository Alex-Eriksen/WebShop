import { Component, OnInit } from '@angular/core';
import { StaticOrderResponse } from 'src/app/_models/order';
import { DirectOrderResponse } from 'src/app/_models/order/DirectOrderResponse';
import { StaticTransactionResponse } from 'src/app/_models/transaction';
import { TransactionService } from 'src/app/_services/transaction.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

	orders: StaticOrderResponse[] = [];


	order: DirectOrderResponse = {
		orderID: 0,
		customer: {
		  customerID: 0,
		  accountID: 0,
		  firstName: "",
		  lastName: "",
		  phoneNumber: "",
		  countryID: 1,
		  zipCode: 2600,
		  gender: "",
		  created_At: new Date(),
		},
		orderTotal: 0,
		transactions: []

	};

  constructor(private transactionService: TransactionService) { }


  ngOnInit(): void {
    this.transactionService.GetAllOrders().subscribe(data => this.orders = data);
  }

  public edit(order: StaticOrderResponse): void
  {
	  this.transactionService.GetOrder(order.orderID).subscribe(data => {
		this.order.customer = {
			customerID: data.customer.customerID,
			accountID: data.customer.accountID,
			firstName: data.customer.firstName,
			lastName: data.customer.lastName,
			phoneNumber: data.customer.phoneNumber,
			countryID: data.customer.countryID,
			zipCode: data.customer.zipCode,
			gender: data.customer.gender,
			created_At: data.customer.created_At,
		},
		this.order.orderID = data.orderID,
		this.order.orderTotal = data.orderTotal,
		this.order.transactions = data.transactions;
	  })
  }

  public cancel(): void
  {
	  this.order = {
		orderID: 0,
		customer: {
		  customerID: 0,
		  accountID: 0,
		  firstName: "",
		  lastName: "",
		  phoneNumber: "",
		  countryID: 1,
		  zipCode: 2600,
		  gender: "",
		  created_At: new Date(),
		},
		orderTotal: 0,
		transactions: []
	};
  }
}
