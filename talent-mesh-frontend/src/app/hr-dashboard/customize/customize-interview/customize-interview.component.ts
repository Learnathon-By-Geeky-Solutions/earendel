import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomizePanelComponent } from '../customize-panel/customize-panel.component';
import { RubricsConfigComponent } from '../rubrics-config/rubrics-config.component';

@Component({
  selector: 'app-customize-interview',
  standalone: true,
  imports: [
    CommonModule,
    CustomizePanelComponent,
    RubricsConfigComponent,
  ],
  template: `
    <div class="customize-page">

      <div class="content-area">
        <div class="d-flex">
          <app-customize-panel></app-customize-panel>
          <div class="flex-grow-1 position-relative">
            <app-rubrics-config
              class="position-fixed end-0 top-0 mt-5 me-4"
            ></app-rubrics-config>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .customize-page {
        min-height: 100vh;
        background-color: #1a1a1a;
      }
      .content-area {
        padding: 2rem;
      }
    `,
  ],
})
export class CustomizeInterviewComponent {}
