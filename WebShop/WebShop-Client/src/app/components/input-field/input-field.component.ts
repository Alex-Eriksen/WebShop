import { Component, EventEmitter, Input, OnInit, Output, ViewChild, ViewContainerRef } from '@angular/core';

@Component({
  selector: 'input-field',
  templateUrl: './input-field.component.html',
  styleUrls: ['./input-field.component.css']
})
export class InputFieldComponent implements OnInit
{
	@Input() label: string = 'NO LABEL';
	@Input() required: boolean = true;
	@Input() type: string = 'text';

	@Input() valueBind: any = {};
	@Output() valueBindChange: EventEmitter<any> = new EventEmitter<any>();

	@Output() isValidChange: EventEmitter<boolean> = new EventEmitter<boolean>();

	public errorMessage: string = '';

	constructor() { }

	ngOnInit(): void { }

	public validate(valueElement: HTMLInputElement): void
	{
		if (this.required)
		{
			if (typeof (valueElement.value) === 'string')
			{
				if (valueElement.value === '')
				{
					this.errorMessage = `${this.label} må ikke være tom.`;
					this.isValidChange.emit(false);
					return;
				}
			}
			else if (typeof (valueElement.value) === 'number')
			{
				if (valueElement.value === null)
				{
					this.errorMessage = `${this.label} må ikke være tom.`;
					this.isValidChange.emit(false);
					return;
				}

				if (valueElement.value === undefined)
				{
					this.errorMessage = `${this.label} må ikke være tom.`;
					this.isValidChange.emit(false);
					return;
				}
			}
		}
		this.errorMessage = '';
		this.isValidChange.emit(true);
	}
}
