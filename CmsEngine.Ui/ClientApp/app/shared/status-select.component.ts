import { Component, Input, forwardRef } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DocumentStatus } from '../models/index';

const noop = () => { }; // does nothing. Signals that no operation is required

@Component({
  selector: 'cms-status-select',
  templateUrl: 'status-select.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => StatusSelectComponent),
    multi: true
  }]
})
export class StatusSelectComponent implements ControlValueAccessor {
  @Input() elementId: string;
  @Input() name: string;
  @Input() selectedStatus: DocumentStatus;

  // using '_value' because variable 'value' is already defined by get()/set()
  private _value: string;

  // Callback registered via registerOnChange (ControlValueAccessor)
  private onChangeCallback: (_: any) => void = noop;
  // Callback registered via registerOnTouched (ControlValueAccessor)
  private onTouchedCallback: () => void = noop;

  // Bonus - see how the constants define
  // the values in the markup above
  status = DocumentStatus;

  get value(): any {
    return this._value;
  }

  @Input()
  set value(v: any) {
    if (v !== this._value) {
      this._value = v;
      this.onChangeCallback(v);
    }
  }

  writeValue(v: any): void {
    // Checks for zero since otherwise the status will never be 'Published'
    if (v || v === 0) {
      this.value = v;
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    throw new Error('Method not implemented.');
  }
}
