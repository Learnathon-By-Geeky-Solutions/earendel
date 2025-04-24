// import { CommonModule } from '@angular/common';
// import {
//   Component,
//   ElementRef,
//   AfterViewInit,
//   ViewChild,
//   OnInit,
//   OnDestroy,
// } from '@angular/core';
// import { FormsModule } from '@angular/forms';
// import { Subscription } from 'rxjs';
// import { languageTemplate, supportedLanguages } from '../Language';
// import { HomeService } from '../shared/services/home.service';

// @Component({
//   selector: 'app-monaco-editor',
//   // templateUrl: './code.component.html',
//   template: `
//     <div class="editor-container">
//       <div class="controls">
//         <select
//           [(ngModel)]="selectedLanguage"
//           (change)="changeLanguage()"
//           class="language-dropdown"
//         >
//           <option *ngFor="let lang of supportedLanguages" [value]="lang.id">
//             {{ lang.name }}
//           </option>
//         </select>
//         <button class="run-button" (click)="runCode()">Run</button>
//       </div>

//       <div class="editor-output">
//         <div #editorContainer class="editor"></div>

//         <div class="output">
//           <div class="input-section">
//             <h3>Input:</h3>
//             <textarea
//               [(ngModel)]="userInput"
//               class="input-field"
//               rows="4"
//             ></textarea>
//           </div>

//           <div class="output-section">
//             <h3>Output:</h3>
//             <pre>{{ output }}</pre>
//           </div>
//         </div>
//       </div>
//     </div>
//   `,
//   // styleUrls: ['./code.component.css'],
//   styles: [
//     `
//       .editor-container {
//         display: flex;
//         flex-direction: column;
//         width: 100%;
//         height: 100%;
//         padding: 10px;
//       }

//       .controls {
//         display: flex;
//         justify-content: space-between;
//         align-items: center;
//         margin-bottom: 10px;
//         padding: 10px;
//         background-color: #1e1e1e;
//         border-radius: 5px;
//       }

//       .language-dropdown {
//         padding: 8px 12px;
//         border: 1px solid #ccc;
//         border-radius: 5px;
//         font-size: 14px;
//         background-color: #2d2d2d;
//         color: white;
//         outline: none;
//       }

//       .run-button {
//         padding: 10px 20px;
//         font-size: 14px;
//         font-weight: bold;
//         color: white;
//         background-color: #007acc;
//         border: none;
//         border-radius: 5px;
//         cursor: pointer;
//         transition: background-color 0.3s ease;
//       }

//       .run-button:hover {
//         background-color: #005a9e;
//       }

//       .editor-output {
//         display: flex;
//         flex-wrap: wrap; /* Allow items to wrap to new lines */
//         gap: 10px;
//         height: 100%; /* Full height */
//         padding-top: 10px;
//       }

//       .editor {
//         flex: 3 1 60%; /* The editor takes more space on larger screens */
//         border: 1px solid #ccc;
//         border-radius: 5px;
//       }

//       .output {
        // flex: 1 1 35%; /* The output takes less space on larger screens */
        // display: flex;
        // flex-direction: column;
        // background-color: #1e1e1e;
        // border: 1px solid #ccc;
        // border-radius: 5px;
        // color: white;
        // padding: 10px;
        // overflow-y: auto;
//       }

//       .input-section,
//       .output-section {
//         flex: 1;
//         margin-bottom: 10px;
//       }

//       .input-section h3,
//       .output-section h3 {
//         color: #dcdcdc;
//         font-size: 16px;
//       }

//       .input-field {
//         width: 100%;
//         padding: 10px;
//         margin: 5px 0;
//         background-color: #2d2d2d;
//         color: white;
//         font-family: 'Courier New', Courier, monospace;
//         font-size: 14px;
//         border: 1px solid #ccc;
//         border-radius: 5px;
//         outline: none;
//         resize: vertical;
//       }

//       pre {
//         background-color: #2d2d2d;
//         padding: 10px;
//         border-radius: 5px;
//         white-space: pre-wrap;
//         word-wrap: break-word;
//         font-size: 14px;
//         overflow-x: auto;
//       }

//       @media screen and (max-width: 768px) {
//         .editor-container {
//           padding: 5px;
//         }

//         .controls {
//           flex-direction: column; /* Stack controls on top of each other */
//           gap: 10px;
//           align-items: stretch;
//         }

//         .editor-output {
//           flex-direction: column; /* Stack the editor and output vertically */
//           gap: 15px;
//         }

//         .editor {
//           flex: 1 1 100%; /* Full width on small screens */
//           height: 250px; /* Limit the editor height */
//         }

//         .output {
//           flex: 1 1 100%; /* Full width on small screens */
//           height: 200px; /* Limit the output height */
//         }

//         .input-field {
//           font-size: 13px; /* Smaller font on mobile */
//           padding: 8px;
//         }

//         .run-button {
//           font-size: 13px; /* Smaller button on mobile */
//           padding: 8px 15px;
//         }
//       }

//       @media screen and (max-width: 480px) {
//         .controls {
//           font-size: 12px; /* Smaller font for very small screens */
//         }

//         .run-button {
//           font-size: 12px; /* Smaller button text */
//           padding: 6px 12px;
//         }

//         .input-field {
//           font-size: 12px; /* Smaller font for input */
//         }

//         pre {
//           font-size: 12px; /* Smaller output font */
//         }
//       }
//     `,
//   ],
//   standalone: true,
//   imports: [FormsModule, CommonModule],
// })
// export class CodeComponent implements AfterViewInit, OnInit, OnDestroy {
//   @ViewChild('editorContainer', { static: true }) editorContainer!: ElementRef;

//   userNotification: string | null = null;
//   private userSub: Subscription | undefined;

//   private editorInstance: any;
//   private readonly isReceivingUpdate = false;
//   public output: string = '';
//   public userInput: string = '';
//   public selectedLanguage: string = 'javascript';

//   public supportedLanguages = supportedLanguages;

//   public languageTemplate = languageTemplate;

//   constructor(
//     // private codeService: CodeService,
//     private homeService: HomeService
//   ) {}

//   ngOnInit(): void {}

//   ngAfterViewInit() {
//     this.initializeMonacoEditor();
//     // this.codeService.startConnection();
//     // this.codeService.listenForCodeUpdates((updatedCode, language) => {
//     //   if (
//     //     this.editorInstance &&
//     //     updatedCode !== this.editorInstance.getValue()
//     //   ) {
//     //     const currentPosition = this.editorInstance.getPosition();
//     //     this.editorInstance.setValue(updatedCode);
//     //     this.selectedLanguage = language;
//     //     this.editorInstance.setPosition(currentPosition);
//     //   }
//     // });
//   }

//   private initializeMonacoEditor() {
//     import('monaco-editor')
//       .then((monaco) => {
//         this.editorInstance = monaco.editor.create(
//           this.editorContainer.nativeElement,
//           {
//             value: `// Write your code here\nconsole.log('Hello, World!');`,
//             language: this.selectedLanguage,
//             theme: 'vs-dark',
//             automaticLayout: true,
//           }
//         );

//         this.editorInstance.onDidChangeModelContent(() => {
//           if (!this.isReceivingUpdate) {
//             const updatedCode = this.editorInstance.getValue();
//             // this.codeService.sendCodeUpdate(updatedCode, this.selectedLanguage);
//           }
//         });
//       })
//       .catch((err) => {
//         console.error('Error loading Monaco Editor:', err);
//       });
//   }

//   // changeLanguage() {
//   //   if (this.editorInstance) {
//   //     const model = this.editorInstance.getModel();
//   //     if (model) {
//   //       import('monaco-editor').then((monaco) => {
//   //         monaco.editor.setModelLanguage(model, this.selectedLanguage);
//   //       });
//   //     }
//   //   }
//   // }

//   changeLanguage() {
//     if (this.editorInstance) {
//       const model = this.editorInstance.getModel();
//       if (model) {
//         import('monaco-editor').then((monaco) => {
//           // Update the language in the Monaco editor
//           monaco.editor.setModelLanguage(model, this.selectedLanguage);

//           // Get the default template for the selected language
//           const defaultTemplate =
//             this.languageTemplate[this.selectedLanguage] || '';

//           // Set the editor's value to the new template
//           this.editorInstance.setValue(defaultTemplate);
//         });
//       }
//     }
//   }

//   runCode() {
//     const lang = this.selectedLanguage;

//     const language = this.supportedLanguages.filter((lan) => lan.id == lang);
//     console.log(language);
//     const body = {
//       source_code: this.editorInstance.getValue(),
//       language_id: language[0]?.language_id, // C++ language ID for Judge0 API
//       stdin: this.userInput, // Input for the program
//     };
//     this.getOutput(body);
//   }
//   getOutput(body: any) {
//     this.homeService.codePost(body).subscribe(
//       (data) => {
//         console.log('code output', data);
//         this.output = data.stdout;
//       },
//       (error: any) => {
//         console.log('Somthing went wrong ', error);
//       }
//     );
//   }
//   ngOnDestroy(): void {
//     if (this.editorInstance) {
//       this.editorInstance.dispose();
//     }
//   }
// }








import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  AfterViewInit,
  ViewChild,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { languageTemplate, supportedLanguages } from '../Language';
import { HomeService } from '../shared/services/home.service';

@Component({
  selector: 'app-monaco-editor',
  // templateUrl: './code.component.html',
  template: `
    <div class="editor-container">
      <div class="controls">
        <select
          [(ngModel)]="selectedLanguage"
          (change)="changeLanguage()"
          class="language-dropdown"
        >
          <option *ngFor="let lang of supportedLanguages" [value]="lang.id">
            {{ lang.name }}
          </option>
        </select>
        <button class="run-button" (click)="runCode()">Run</button>
      </div>

      <div class="editor-output">
        <div #editorContainer class="editor"></div>

        <div class="output">
          <div class="input-section">
            <h3>Input:</h3>
            <textarea
              [(ngModel)]="userInput"
              class="input-field"
              rows="4"
            ></textarea>
          </div>

          <div class="output-section">
            <h3>Output:</h3>
            <pre>{{ output }}</pre>
          </div>
        </div>
      </div>
    </div>
  `,
  // styleUrls: ['./code.component.css'],
  styles: [
    `
      .editor-container {
        height: 100%;
        padding: 0;
      }

      // .editor-output {
      //   height: calc(100% - 60px);
      // }

      .editor-output {
        height: calc(100% - 60px);
        position: relative; /* Add this */
      }

      .editor {
        height: 70% !important;
        position: relative !important;
        // top: 0;
        // left: 0;
        // right: 0;
        // bottom: 0;
        width: 100% !important;
      }

      // .editor {
      //   height: 60% !important;
      // position: absolute !important; /* Add this */
      // top: 0;
      // left: 0;
      // right: 0;
      // bottom: 0;
      // width: 100% !important;
      // }

      .output {
        height: 40% !important;
        flex: 1 1 45%; /* The output takes less space on larger screens */
        display: flex;
        flex-direction: column;
        background-color: #1e1e1e;
        border: 1px solid #ccc;
        border-radius: 5px;
        color: white;
        padding: 10px;
        overflow-y: auto;
      }

      .controls {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 10px;
        padding: 10px;
        background-color: #1e1e1e;
        border-radius: 5px;
      }

      .language-dropdown {
        padding: 8px 12px;
        border: 1px solid #ccc;
        border-radius: 5px;
        font-size: 14px;
        background-color: #2d2d2d;
        color: white;
        outline: none;
      }

      .run-button {
        padding: 10px 20px;
        font-size: 14px;
        font-weight: bold;
        color: white;
        background-color: #007acc;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
      }

      .run-button:hover {
        background-color: #005a9e;
      }

      .input-section,
      .output-section {
        flex: 1;
        margin-bottom: 10px;
      }

      .input-section h3,
      .output-section h3 {
        color: #dcdcdc;
        font-size: 16px;
      }

      .input-field {
        width: 100%;
        padding: 10px;
        margin: 5px 0;
        background-color: #2d2d2d;
        color: white;
        font-family: 'Courier New', Courier, monospace;
        font-size: 14px;
        border: 1px solid #ccc;
        border-radius: 5px;
        outline: none;
        resize: vertical;
      }

      pre {
        background-color: #2d2d2d;
        padding: 10px;
        border-radius: 5px;
        white-space: pre-wrap;
        word-wrap: break-word;
        font-size: 14px;
        overflow-x: auto;
      }

      @media (max-width: 768px) {
        .editor {
          height: 50vh !important;
          position: relative !important;
        }
        .output {
          height: 30vh !important;
        }
      }

      @media screen and (max-width: 768px) {
        .editor-container {
          padding: 5px;
        }

        .controls {
          flex-direction: column; /* Stack controls on top of each other */
          gap: 10px;
          align-items: stretch;
        }

        .editor {
          flex: 1 1 100%; /* Full width on small screens */
          height: 250px; /* Limit the editor height */
          direction: ltr !important; /* Force left-to-right */
          unicode-bidi: bidi-override !important;
        }

        .output {
          flex: 1 1 100%; /* Full width on small screens */
          height: 200px; /* Limit the output height */
        }

        .input-field {
          font-size: 13px; /* Smaller font on mobile */
          padding: 8px;
        }

        .run-button {
          font-size: 13px; /* Smaller button on mobile */
          padding: 8px 15px;
        }
      }

      @media screen and (max-width: 480px) {
        .controls {
          font-size: 12px; /* Smaller font for very small screens */
        }

        .run-button {
          font-size: 12px; /* Smaller button text */
          padding: 6px 12px;
        }

        .input-field {
          font-size: 12px; /* Smaller font for input */
        }

        pre {
          font-size: 12px; /* Smaller output font */
        }
      }
    `,
  ],
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class CodeComponent implements AfterViewInit, OnInit, OnDestroy {
  @ViewChild('editorContainer', { static: true }) editorContainer!: ElementRef;

  userNotification: string | null = null;
  private userSub: Subscription | undefined;

  private editorInstance: any;
  private readonly isReceivingUpdate = false;
  public output: string = '';
  public userInput: string = '';
  public selectedLanguage: string = 'javascript';

  public supportedLanguages = supportedLanguages;

  public languageTemplate = languageTemplate;

  constructor(
    // private codeService: CodeService,
    private homeService: HomeService
  ) {}

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.initializeMonacoEditor();
    // this.codeService.startConnection();
    // this.codeService.listenForCodeUpdates((updatedCode, language) => {
    //   if (
    //     this.editorInstance &&
    //     updatedCode !== this.editorInstance.getValue()
    //   ) {
    //     const currentPosition = this.editorInstance.getPosition();
    //     this.editorInstance.setValue(updatedCode);
    //     this.selectedLanguage = language;
    //     this.editorInstance.setPosition(currentPosition);
    //   }
    // });
  }

  private initializeMonacoEditor() {
    import('monaco-editor')
      .then((monaco) => {
        this.editorInstance = monaco.editor.create(
          this.editorContainer.nativeElement,
          {
            value: `// Write your code here\nconsole.log('Hello, World!');`,
            language: this.selectedLanguage,
            theme: 'vs-dark',
            automaticLayout: true,
            lineNumbers: 'on',
            roundedSelection: false,
            scrollBeyondLastLine: false,
            readOnly: false,
            wordWrap: 'off',
            wrappingIndent: 'none',
            minimap: { enabled: false },
            fixedOverflowWidgets: true,
            // Force LTR settings
            glyphMargin: false,
            lineDecorationsWidth: 0,
            lineNumbersMinChars: 3,
          }
        );

        this.editorInstance.onDidChangeModelContent(() => {
          if (!this.isReceivingUpdate) {
            const updatedCode = this.editorInstance.getValue();
            // this.codeService.sendCodeUpdate(updatedCode, this.selectedLanguage);
          }
        });
      })
      .catch((err) => {
        console.error('Error loading Monaco Editor:', err);
      });
  }

  // changeLanguage() {
  //   if (this.editorInstance) {
  //     const model = this.editorInstance.getModel();
  //     if (model) {
  //       import('monaco-editor').then((monaco) => {
  //         monaco.editor.setModelLanguage(model, this.selectedLanguage);
  //       });
  //     }
  //   }
  // }

  changeLanguage() {
    if (this.editorInstance) {
      const model = this.editorInstance.getModel();
      if (model) {
        import('monaco-editor').then((monaco) => {
          // Update the language in the Monaco editor
          monaco.editor.setModelLanguage(model, this.selectedLanguage);

          // Get the default template for the selected language
          const defaultTemplate =
            this.languageTemplate[this.selectedLanguage] || '';

          // Set the editor's value to the new template
          this.editorInstance.setValue(defaultTemplate);
        });
      }
    }
  }

  runCode() {
    const lang = this.selectedLanguage;

    const language = this.supportedLanguages.filter((lan) => lan.id == lang);
    console.log(language);
    const body = {
      source_code: this.editorInstance.getValue(),
      language_id: language[0]?.language_id, // C++ language ID for Judge0 API
      stdin: this.userInput, // Input for the program
    };
    this.getOutput(body);
  }
  getOutput(body: any) {
    this.homeService.codePost(body).subscribe(
      (data) => {
        console.log('code output', data);
        this.output = data.stdout;
      },
      (error: any) => {
        console.log('Somthing went wrong ', error);
      }
    );
  }
  ngOnDestroy(): void {
    if (this.editorInstance) {
      this.editorInstance.dispose();
    }
  }
}
