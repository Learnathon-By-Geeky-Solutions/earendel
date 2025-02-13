import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [CommonModule],
  template: `
    <nav class="fixed top-0 left-0 right-0 bg-white z-50">
      <div
        class="max-w-[1440px] mx-auto px-6 h-[72px] flex items-center justify-between"
      >
        <a href="/" class="text-[22px] font-bold">TalentMesh</a>

        <div class="hidden md:flex items-center space-x-10">
          <a href="/features" class="text-[15px] text-gray-800 hover:text-black"
            >Features</a
          >
          <a href="/clients" class="text-[15px] text-gray-800 hover:text-black"
            >Our Clients</a
          >
          <a href="/pricing" class="text-[15px] text-gray-800 hover:text-black"
            >Pricing</a
          >
        </div>

        <div class="hidden md:flex items-center space-x-4">
          <a href="/login" class="text-[15px] text-gray-800 hover:text-black"
            >Sign In</a
          >
          <a
            href="/register"
            class="px-4 py-2 bg-[#18181B] text-white text-[15px] rounded-[4px] hover:bg-black transition-colors"
          >
            Register
          </a>
        </div>

        <button class="md:hidden" aria-label="Menu">
          <svg
            class="w-6 h-6"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M4 6h16M4 12h16M4 18h16"
            />
          </svg>
        </button>
      </div>
    </nav>
  `,
})
export class NavigationComponent {}
