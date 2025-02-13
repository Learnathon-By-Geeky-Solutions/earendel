import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface FooterLink {
  text: string;
  href: string;
}

interface FooterSection {
  title: string;
  links: FooterLink[];
}

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
    <!-- Navigation -->
    <nav class="nav-container">
      <div class="nav-content">
        <a href="/" class="logo">TalentMesh</a>

        <div class="nav-links">
          <a href="/features">Features</a>
          <a href="/clients">Our Clients</a>
          <a href="/pricing">Pricing</a>
        </div>

        <div class="nav-auth">
          <a href="/login" class="sign-in">Sign In</a>
          <a routerLink="/register" class="register-btn">Register</a>
        </div>
      </div>
    </nav>

    <!-- Hero Section -->
    <section class="hero-section">
      <div class="hero-content">
        <h1>Streamline Your Tech Hiring with TalentMest</h1>
        <p>
          Connect with top-tier tech talent through our innovative
          interview-as-a-service platform. Simplify your hiring process today.
        </p>

        <div class="email-signup">
          <input
            type="email"
            placeholder="Enter your work email"
            [(ngModel)]="workEmail"
          />
          <button class="get-started-btn">
            Get Started
            <i class="bi bi-arrow-right"></i>
          </button>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features-section">
      <h2>Why Choose TalentMest?</h2>

      <div class="features-grid">
        <div class="feature-card">
          <div class="icon">
            <i class="bi bi-globe"></i>
          </div>
          <h3>Global Talent Pool</h3>
          <p>
            Access a diverse range of skilled interviewers from around the
            world.
          </p>
        </div>

        <div class="feature-card">
          <div class="icon">
            <i class="bi bi-clock"></i>
          </div>
          <h3>Time-Efficient</h3>
          <p>
            Reduce your hiring time by up to 50% with our streamlined process.
          </p>
        </div>

        <div class="feature-card">
          <div class="icon">
            <i class="bi bi-check-circle"></i>
          </div>
          <h3>Quality Assured</h3>
          <p>
            Our rigorous vetting process ensures you only interact with top-tier
            candidates.
          </p>
        </div>
      </div>
    </section>

    <!-- Trusted Companies Section -->
    <section class="trusted-section">
      <h2>Trusted by Industry Leaders</h2>
      <div class="company-logos">
        <div class="logo-row">
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/11/BS-logo1.png"
            alt="Brain Station 23"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Cefalo_logo.jpg"
            alt="Cefalo"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Vivasoft_logo.jpg"
            alt="Vivasoft"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Enosis_logo.jpg"
            alt="Enosis"
            class="company-logo"
          />
        </div>
        <div class="logo-row">
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/BracIT_logo.jpg"
            alt="BracIT"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Orbirax_logo.jpg"
            alt="Orbitax"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Inument_logo.jpg"
            alt="Inument"
            class="company-logo"
          />
          <img
            src="https://learnathon.geeky.solutions/wp-content/uploads/2024/10/Craftsmen_logo.jpg"
            alt="Craftsmen"
            class="company-logo"
          />
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section">
      <h2>Ready to Transform Your Hiring Process?</h2>
      <p>
        Join TalentMest today and experience the future of tech recruitment.
      </p>
      <button class="get-started-btn">
        Get Started Now
        <i class="bi bi-arrow-right"></i>
      </button>
    </section>

    <!-- Footer -->
    <footer class="footer">
      <div class="footer-content">
        <div class="footer-grid">
          <div class="footer-section">
            <h3>Product</h3>
            <ul>
              <li><a href="/features">Features</a></li>
              <li><a href="/pricing">Pricing</a></li>
              <li><a href="/faq">FAQ</a></li>
            </ul>
          </div>

          <div class="footer-section">
            <h3>Company</h3>
            <ul>
              <li><a href="/about">About Us</a></li>
              <li><a href="/careers">Careers</a></li>
              <li><a href="/contact">Contact</a></li>
            </ul>
          </div>

          <div class="footer-section">
            <h3>Resources</h3>
            <ul>
              <li><a href="/blog">Blog</a></li>
              <li><a href="/case-studies">Case Studies</a></li>
              <li><a href="/webinars">Webinars</a></li>
            </ul>
          </div>

          <div class="footer-section">
            <h3>Legal</h3>
            <ul>
              <li><a href="/privacy">Privacy Policy</a></li>
              <li><a href="/terms">Terms of Service</a></li>
              <li><a href="/cookie-policy">Cookie Policy</a></li>
            </ul>
          </div>
        </div>

        <div class="footer-bottom">
          <p>&copy; 2024 TalentMesh. All rights reserved.</p>
        </div>
      </div>
    </footer>
  `,
  styles: [
    `
      :host {
        display: block;
      }

      .nav-container {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        background: white;
        border-bottom: 1px solid #eee;
        z-index: 1000;
      }

      .nav-content {
        max-width: 1200px;
        margin: 0 auto;
        padding: 1rem 2rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }

      .logo {
        font-size: 1.25rem;
        font-weight: 600;
        color: #000;
        text-decoration: none;
      }

      .nav-links {
        display: flex;
        gap: 2rem;

        a {
          color: #000;
          text-decoration: none;
          font-size: 0.875rem;

          &:hover {
            color: #666;
          }
        }
      }

      .nav-auth {
        display: flex;
        gap: 1rem;
        align-items: center;
      }

      .sign-in {
        color: #000;
        text-decoration: none;
        font-size: 0.875rem;

        &:hover {
          color: #666;
        }
      }

      .register-btn {
        background: #000;
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 4px;
        text-decoration: none;
        font-size: 0.875rem;

        &:hover {
          background: #333;
        }
      }

      .hero-section {
        padding: 8rem 2rem 4rem;
        text-align: center;
        background: #fff;
      }

      .hero-content {
        max-width: 800px;
        margin: 0 auto;
      }

      h1 {
        font-size: 3rem;
        font-weight: 700;
        margin-bottom: 1.5rem;
        line-height: 1.2;
      }

      .hero-content p {
        font-size: 1.125rem;
        color: #666;
        margin-bottom: 2rem;
        line-height: 1.6;
      }

      .email-signup {
        display: flex;
        gap: 1rem;
        max-width: 500px;
        margin: 0 auto;

        input {
          flex: 1;
          padding: 0.75rem 1rem;
          border: 1px solid #ddd;
          border-radius: 4px;
          font-size: 0.875rem;

          &:focus {
            outline: none;
            border-color: #000;
          }
        }
      }

      .get-started-btn {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        background: #000;
        color: white;
        border: none;
        padding: 0.75rem 1.5rem;
        border-radius: 4px;
        font-size: 0.875rem;
        cursor: pointer;

        &:hover {
          background: #333;
        }

        i {
          font-size: 1rem;
        }
      }

      .features-section {
        padding: 4rem 2rem;
        text-align: center;
      }

      h2 {
        font-size: 2rem;
        font-weight: 600;
        margin-bottom: 3rem;
      }

      .features-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        gap: 2rem;
        max-width: 1200px;
        margin: 0 auto;
      }

      .feature-card {
        padding: 2rem;
        border: 1px solid #eee;
        border-radius: 8px;
        transition: transform 0.2s ease;

        &:hover {
          transform: translateY(-5px);
        }

        .icon {
          width: 48px;
          height: 48px;
          background: #f8f9fa;
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          margin: 0 auto 1.5rem;

          i {
            font-size: 1.5rem;
          }
        }

        h3 {
          font-size: 1.25rem;
          font-weight: 600;
          margin-bottom: 1rem;
        }

        p {
          color: #666;
          line-height: 1.6;
          font-size: 0.875rem;
        }
      }

      .trusted-section {
        padding: 4rem 2rem;
        text-align: center;
        background: #f8f9fa;
      }

      .company-logos {
        max-width: 1200px;
        margin: 0 auto;
      }

      .logo-row {
        display: flex;
        justify-content: center;
        align-items: center;
        gap: 2rem;
        margin-bottom: 2rem;

        &:last-child {
          margin-bottom: 0;
        }
      }

      .company-logo {
        height: 40px;
        object-fit: contain;
        filter: grayscale(100%);
        opacity: 0.7;
        transition: all 0.3s ease;

        &:hover {
          filter: grayscale(0);
          opacity: 1;
        }
      }

      .cta-section {
        padding: 6rem 2rem;
        text-align: center;
        background: #fff;

        h2 {
          margin-bottom: 1rem;
        }

        p {
          color: #666;
          margin-bottom: 2rem;
          font-size: 1.125rem;
        }

        .get-started-btn {
          margin: 0 auto;
        }
      }

      .footer {
        background: #f8f9fa;
        padding: 4rem 2rem 2rem;
      }

      .footer-content {
        max-width: 1200px;
        margin: 0 auto;
      }

      .footer-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 2rem;
        margin-bottom: 3rem;
      }

      .footer-section {
        h3 {
          font-size: 1rem;
          font-weight: 600;
          margin-bottom: 1rem;
        }

        ul {
          list-style: none;
          padding: 0;
          margin: 0;
        }

        li {
          margin-bottom: 0.5rem;
        }

        a {
          color: #666;
          text-decoration: none;
          font-size: 0.875rem;

          &:hover {
            color: #000;
          }
        }
      }

      .footer-bottom {
        text-align: center;
        padding-top: 2rem;
        border-top: 1px solid #eee;

        p {
          color: #666;
          font-size: 0.875rem;
          margin: 0;
        }
      }

      @media (max-width: 768px) {
        .nav-links {
          display: none;
        }

        .hero-section {
          padding: 6rem 1rem 3rem;
        }

        h1 {
          font-size: 2rem;
        }

        .email-signup {
          flex-direction: column;
        }

        .features-grid {
          grid-template-columns: 1fr;
          padding: 0 1rem;
        }

        .logo-row {
          flex-wrap: wrap;
          gap: 1rem;
        }

        .company-logo {
          height: 30px;
        }

        .footer-grid {
          grid-template-columns: 1fr;
          text-align: center;
        }
      }
    `,
  ],
})
export class LandingComponent {
  workEmail = '';
}
