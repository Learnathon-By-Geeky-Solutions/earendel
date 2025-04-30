import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-animated-input',
  standalone: true,
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule],
  template: `
    <div class="w-full">
      <input
        class="w-full px-4 py-2 rounded border focus:border-primary focus:ring-1 focus:ring-primary focus:outline-none"
        [type]="type"
        [placeholder]="placeholder"
        [name]="name"
        [disabled]="disabled"
        [attr.autocomplete]="autocomplete"
        [(ngModel)]="value"
        (focus)="onFocusEvent()"
        (input)="onInputChange($event)"
        (keyup)="onInputChange($event)"
      />
      <p *ngIf="error" class="mt-1 text-sm text-red-600">{{ error }}</p>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }
    input {
      border-color: #ccc;
      transition: all 0.2s ease-in-out;
    }
    input:focus {
      border-color: #ff731d;
      box-shadow: 0 0 0 1px #ff731d;
    }
  `],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AnimatedInputComponent),
      multi: true
    }
  ]
})
export class AnimatedInputComponent implements ControlValueAccessor {
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() name: string = '';
  @Input() error: string = '';
  @Input() autocomplete: string = '';
  @Output() focus = new EventEmitter<void>();
  @Output() input = new EventEmitter<Event>();

  value: string = '';
  disabled: boolean = false;

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  onFocusEvent(): void {
    this.onTouched();
    this.focus.emit();
  }

  onInputChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.onChange(this.value);
    
    // Emit input event for parent components to handle
    this.input.emit(event);
  }

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
} 