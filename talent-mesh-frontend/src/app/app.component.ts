import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import './styles/shared.css';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, 
    ],
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'frontend';
}
