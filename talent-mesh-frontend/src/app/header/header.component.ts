import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  showDropdown = false;
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  closeDropdown() {
    this.showDropdown = false;
  }
}
