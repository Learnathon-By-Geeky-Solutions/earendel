import {
  Component,
  Input,
  Output,
  EventEmitter,
  SimpleChanges,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatDialogModule, MatDialog } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { Job } from "../services/job.service";
import { SkillService } from "../../admin-dashboard/services/skill.service";
import { catchError, forkJoin, map, of } from "rxjs";

@Component({
  selector: "app-cover-letter-dialog",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  template: `
    <h2 mat-dialog-title>Add Cover Letter</h2>
    <mat-dialog-content>
      <p>Please write a cover letter for your job application:</p>
      <mat-form-field appearance="outline" style="width: 100%">
        <mat-label>Cover Letter</mat-label>
        <textarea
          matInput
          [(ngModel)]="coverLetter"
          rows="8"
          placeholder="Tell us why you're a good fit for this position..."
        ></textarea>
      </mat-form-field>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button
        mat-raised-button
        color="primary"
        [mat-dialog-close]="coverLetter"
      >
        Submit Application
      </button>
    </mat-dialog-actions>
  `,
})
export class CoverLetterDialogComponent {
  coverLetter = "";
}

@Component({
  selector: "app-job-details-modal",
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    FormsModule,
  ],
  template: `
    <div *ngIf="isOpen" class="modal-overlay" (click)="close()">
      <div class="modal-content" (click)="$event.stopPropagation()">
        <div class="modal-header">
          <div>
            <h2>{{ job.name }}</h2>
            <p class="posted-date">Posted {{ job.createdOn | date }}</p>
          </div>
          <button class="close-btn" (click)="close()">
            <mat-icon>close</mat-icon>
          </button>
        </div>

        <div class="modal-body">
          <div class="info-grid">
            <div class="info-item">
              <mat-icon>work</mat-icon>
              <div>
                <label>Experience Level</label>
                <span>{{ job.experienceLevel }}</span>
              </div>
            </div>
            <div class="info-item">
              <mat-icon>location_on</mat-icon>
              <div>
                <label>Location</label>
                <span>{{ job.location }}</span>
              </div>
            </div>
            <div class="info-item">
              <mat-icon>business</mat-icon>
              <div>
                <label>Job Type</label>
                <span>{{ job.jobType }}</span>
              </div>
            </div>
          </div>

          <div class="section">
            <h3>Description</h3>
            <p>{{ job.description }}</p>
          </div>

          <div class="section">
            <h3>Requirements</h3>
            <p>{{ job.requirments }}</p>
          </div>

          <div class="section" *ngIf="job.requiredSkillIds?.length">
            <h3>Required Skills</h3>
            <div *ngIf="loadingLookups">Loading skills…</div>
            <div class="skills-list" *ngIf="!loadingLookups">
              <span *ngFor="let name of skillNames" class="skill-tag">{{
                name
              }}</span>
            </div>
          </div>

          <div class="section" *ngIf="job.requiredSubskillIds?.length">
            <h3>Required Subskills</h3>
            <div *ngIf="loadingLookups">Loading subskills…</div>
            <div class="skills-list" *ngIf="!loadingLookups">
              <span *ngFor="let name of subskillNames" class="skill-tag">{{
                name
              }}</span>
            </div>
          </div>
        </div>

        <div class="modal-footer">
          <button mat-raised-button color="primary" (click)="apply()">
            Apply Now
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 24px;
        z-index: 1000;
      }

      .modal-content {
        background: white;
        border-radius: 12px;
        width: 100%;
        max-width: 800px;
        max-height: 90vh;
        display: flex;
        flex-direction: column;
      }

      .modal-header {
        padding: 24px;
        border-bottom: 1px solid #eee;
        display: flex;
        justify-content: space-between;
        align-items: start;

        h2 {
          font-size: 24px;
          font-weight: 600;
          margin: 0 0 8px;
          color: #333;
        }

        .posted-date {
          font-size: 14px;
          color: #666;
          margin: 0;
        }
      }

      .close-btn {
        background: none;
        border: none;
        color: #666;
        cursor: pointer;
        padding: 4px;
        line-height: 1;

        &:hover {
          color: #333;
        }
      }

      .modal-body {
        padding: 24px;
        overflow-y: auto;
      }

      .info-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 24px;
        margin-bottom: 32px;
      }

      .info-item {
        display: flex;
        gap: 12px;
        align-items: start;

        mat-icon {
          font-size: 20px;
          color: #666;
        }

        label {
          display: block;
          font-size: 12px;
          color: #666;
          margin-bottom: 4px;
        }

        span {
          font-size: 14px;
          color: #333;
          font-weight: 500;
        }
      }

      .section {
        margin-bottom: 24px;

        h3 {
          font-size: 18px;
          font-weight: 600;
          margin: 0 0 16px;
          color: #333;
        }

        p {
          font-size: 14px;
          line-height: 1.6;
          color: #666;
          margin: 0;
        }
      }

      .skills-list {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
      }

      .skill-tag {
        padding: 6px 12px;
        background: #f5f5f5;
        border-radius: 16px;
        font-size: 12px;
        color: #666;
      }

      .modal-footer {
        padding: 16px 24px;
        border-top: 1px solid #eee;
        display: flex;
        justify-content: flex-end;
      }
    `,
  ],
})
export class JobDetailsModalComponent {
  @Input() isOpen = false;
  @Input() job!: Job;
  @Output() closeModal = new EventEmitter<void>();
  @Output() applyForJob = new EventEmitter<{ job: Job; coverLetter: string }>();

  skillNames: string[] = [];
  subskillNames: string[] = [];
  loadingLookups = false;

  constructor(private dialog: MatDialog, private skillService: SkillService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes["job"] && this.job) {
      this.loadLookups();
    }
  }

  private loadLookups() {
    this.loadingLookups = true;
    this.skillNames = [];
    this.subskillNames = [];

    const skillCalls = (this.job.requiredSkillIds || []).map((id) =>
      this.skillService.skillDetailsData({ id }).pipe(
        map((page) => page.items[0]?.name ?? `(unknown ${id})`),
        catchError(() => of(`(unknown ${id})`))
      )
    );

    const subskillCalls = (this.job.requiredSubskillIds || []).map((id) =>
      this.skillService.subskillDetailsData({ id }).pipe(
        map((page) => page.items[0]?.name ?? `(unknown ${id})`),
        catchError(() => of(`(unknown ${id})`))
      )
    );

    forkJoin({
      skills: skillCalls.length ? forkJoin(skillCalls) : of([]),
      subskills: subskillCalls.length ? forkJoin(subskillCalls) : of([]),
    }).subscribe(
      ({ skills, subskills }) => {
        this.skillNames = skills;
        this.subskillNames = subskills;
        this.loadingLookups = false;
      },
      () => {
        // on any error, at least stop the spinner
        this.loadingLookups = false;
      }
    );
  }

  close() {
    this.closeModal.emit();
  }

  apply() {
    const dialogRef = this.dialog.open(CoverLetterDialogComponent, {
      width: "600px",
    });

    dialogRef.afterClosed().subscribe((coverLetter) => {
      if (coverLetter) {
        this.applyForJob.emit({ job: this.job, coverLetter });
      }
    });
  }
}
