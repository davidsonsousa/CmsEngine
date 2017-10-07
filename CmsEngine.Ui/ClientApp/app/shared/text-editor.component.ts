import {
  Component,
  OnDestroy,
  AfterViewInit,
  EventEmitter,
  Input,
  Output,
  forwardRef,
  NgZone
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


declare var tinymce: any;
const noop = () => { }; // does nothing. Signals that no operation is required

@Component({
  selector: 'cms-text-editor',
  templateUrl: 'text-editor.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => TextEditorComponent),
    multi: true
  }]
})
export class TextEditorComponent implements AfterViewInit, OnDestroy, ControlValueAccessor  {
  @Input() elementId: string;
  @Input() name: string;

  editor;

  constructor(private ngZone: NgZone) { }

  // using '_value' because variable 'value' is already defined by get()/set()
  private _value: string = '';

  // Callback registered via registerOnChange (ControlValueAccessor)
  private onChangeCallback: (_: any) => void = noop;
  // Callback registered via registerOnTouched (ControlValueAccessor)
  private onTouchedCallback: () => void = noop;

  ngAfterViewInit() {
    tinymce.init({
      selector: '#' + this.elementId,
      plugins: ['link', 'table'],
      // skin_url: '/tinymce/skins/lightgray',
      setup: editor => {
        this.editor = editor;
        editor.on('blur', (e) => {
          const content = editor.getContent();
          this.value = content;

          // This tells ng to update the model (div in main area) with new HTML in editor pane
          // See:  https://community.tinymce.com/communityQuestion?id=90661000000IetUAAS
          // and:  https://blog.thoughtram.io/angular/2016/02/01/zones-in-angular-2.html
          this.ngZone.run(() => { });
        });
      },
    });
    if (this.value) {
      tinymce.activeEditor.setContent(this.value, { format: 'raw' });
    }
  }

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

  writeValue(v: any) {
    if (v) {
      this.value = v;
      if (tinymce.activeEditor) {
        tinymce.activeEditor.setContent(this.value, { format: 'raw' });
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      tinymce.get(this.elementId).getDoc().designMode = 'Off';
    } else {
      tinymce.get(this.elementId).getDoc().designMode = 'On';
    }
  }

  ngOnDestroy() {
    tinymce.remove(this.editor);
  }
}
