import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-rubrics-config',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="rubrics-panel text-white p-4 rounded-3">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h5 class="mb-0">Configure rubrics</h5>
        <button class="btn-close btn-close-white opacity-75"></button>
      </div>
      <p class="text-muted small mb-4">
        Adjust depth on each skill to be tested & overall round duration here
      </p>

      <div class="mb-4">
        <label class="d-flex justify-content-between align-items-center mb-2">
          <span>Duration</span>
          <div class="duration-selector">
            1 hr
            <i class="bi bi-chevron-down ms-1"></i>
          </div>
        </label>
      </div>

      <div class="mb-4">
        <h6 class="mb-3">Must have rubrics</h6>
        <div class="rubric-item mb-3">
          <div class="d-flex justify-content-between align-items-center mb-3">
            <div class="d-flex align-items-center">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" checked />
              </div>
              <span class="ms-2">JavaScript</span>
            </div>
            <i class="bi bi-chevron-down"></i>
          </div>
          <div class="rubric-subitems ms-4">
            <div class="rubric-subitem mb-3">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" checked />
                <label class="form-check-label">
                  <div>Asynchronous programming</div>
                  <small class="text-muted"
                    >Promises, Async/await, Callbacks, Observables, Generators
                    etc.</small
                  >
                </label>
              </div>
            </div>
            <div class="rubric-subitem mb-3">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" checked />
                <label class="form-check-label">
                  <div>Redux</div>
                  <small class="text-muted"
                    >Understanding of redux fundamentals</small
                  >
                </label>
              </div>
            </div>
            <div class="rubric-subitem mb-3">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" checked />
                <label class="form-check-label">
                  <div>Browser: Documents, Events and Interfaces</div>
                  <small class="text-muted"
                    >Browser APIs, Events & event loop, DOM manipulation & tree,
                    Forms</small
                  >
                </label>
              </div>
            </div>
            <div class="rubric-subitem">
              <div class="form-check">
                <input class="form-check-input" type="checkbox" checked />
                <label class="form-check-label">
                  <div>JavaScript fundamentals</div>
                  <small class="text-muted"
                    >Operators, Objects, Data types, Advanced working with
                    functions, Variable scope, closures, hoisting, polyfills,
                    Currying, ES7/8/9 etc.</small
                  >
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .rubrics-panel {
        width: 480px;
        background-color: #212529;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
      }
      .duration-selector {
        padding: 4px 12px;
        background-color: rgba(255, 255, 255, 0.1);
        border-radius: 4px;
        font-size: 14px;
      }
      .form-check-input {
        width: 18px;
        height: 18px;
        background-color: transparent;
        border: 2px solid #0d6efd;
        margin-top: 3px;
      }
      .form-check-input:checked {
        background-color: #0d6efd;
        border-color: #0d6efd;
      }
      .form-check-label {
        font-size: 14px;
        padding-left: 4px;
      }
      .rubric-subitem small {
        font-size: 12px;
        display: block;
        margin-top: 2px;
        color: rgba(255, 255, 255, 0.5);
      }
      .text-muted {
        color: rgba(255, 255, 255, 0.5) !important;
      }
    `,
  ],
})
export class RubricsConfigComponent {}
