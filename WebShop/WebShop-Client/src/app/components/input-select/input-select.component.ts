import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Pair } from 'src/app/_models/_misc/pair';

@Component({
  selector: 'input-select',
  templateUrl: './input-select.component.html',
  styleUrls: ['./input-select.component.css']
})
export class InputSelectComponent implements OnInit
{
	@Input() label: string = 'NO LABEL';
	@Input() required: boolean = true;

	@Input() valueBind: any = {};
	@Output() valueBindChange: EventEmitter<any> = new EventEmitter<any>();
	@Output() isValidChange: EventEmitter<boolean> = new EventEmitter<boolean>();

	@Input() options: Pair[] = [];

	public errorMessage: string = '';

  	constructor() { }

	ngOnInit(): void { }

	public validate(valueElement: HTMLSelectElement): void
	{
		if (this.required)
		{
			if (valueElement.value == '0')
			{
				this.errorMessage = `${this.label} må ikke være tom.`;
				this.isValidChange.emit(false);
				return;
			}

			if (valueElement.value === '')
			{
				this.errorMessage = `${this.label} må ikke være tom.`;
				this.isValidChange.emit(false);
				return;
			}
		}
		this.errorMessage = '';
		this.isValidChange.emit(true);
	}
}
