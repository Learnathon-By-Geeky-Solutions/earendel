import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  template: `
    <footer class="py-20 bg-gray-50">
      <div class="max-w-[1200px] mx-auto px-6">
        <div class="grid grid-cols-2 md:grid-cols-4 gap-12">
          <div>
            <h3 class="text-[15px] font-semibold mb-6">Product</h3>
            <ul class="space-y-4">
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Features</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Pricing</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >FAQ</a
                >
              </li>
            </ul>
          </div>

          <div>
            <h3 class="text-[15px] font-semibold mb-6">Company</h3>
            <ul class="space-y-4">
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >About Us</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Careers</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Contact</a
                >
              </li>
            </ul>
          </div>

          <div>
            <h3 class="text-[15px] font-semibold mb-6">Resources</h3>
            <ul class="space-y-4">
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Blog</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Case Studies</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Webinars</a
                >
              </li>
            </ul>
          </div>

          <div>
            <h3 class="text-[15px] font-semibold mb-6">Legal</h3>
            <ul class="space-y-4">
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Privacy Policy</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Terms of Service</a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="text-[15px] text-gray-600 hover:text-gray-900"
                  >Cookie Policy</a
                >
              </li>
            </ul>
          </div>
        </div>

        <div class="mt-16 pt-8 border-t border-gray-200 text-center">
          <p class="text-[15px] text-gray-600">
            © 2025 TalentMesh. All rights reserved.
          </p>
        </div>
      </div>
    </footer>
  `,
})
export class FooterComponent {}
