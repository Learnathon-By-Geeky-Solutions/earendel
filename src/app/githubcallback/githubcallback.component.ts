import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-github-callback',
  template: `
    <div class="container">
      <h2>GitHub Callback</h2>
      <p *ngIf="code">Received Code: {{ code }}</p>
      <p *ngIf="!code">No code received</p>
    </div>
  `,
  styles: [
    `
      .container {
        margin: 40px;
        text-align: center;
      }
    `,
  ],
})
export class GithubcallbackComponent implements OnInit {
  code: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // Option 1: Snapshot (simpler, works if route doesn't change dynamically)
    this.code = this.route.snapshot.queryParamMap.get('code');
    console.log(this.code);
  }
}
