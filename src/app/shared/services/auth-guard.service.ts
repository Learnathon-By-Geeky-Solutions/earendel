import { Injectable, Component, NgModule } from '@angular/core';
import {
  Routes,
  RouterModule,
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';

/* =========================
   The All-In-One Guard
   ========================= */
@Injectable({
  providedIn: 'root',
})
export class AuthGuardService implements CanActivate {
  constructor(private router: Router) {}

  // Check if the user is logged in by looking for "loggedInUser" in sessionStorage
  isAuthenticated(): boolean {
    const userStr = sessionStorage.getItem('loggedInUser');
    if (!userStr) return false;
    try {
      const user = JSON.parse(userStr);
      return !!user.token; // returns true if token exists
    } catch (e) {
      return false;
    }
  }

  // Retrieve the user's roles from the stored "loggedInUser" JSON data in sessionStorage
  getUserRoles(): string[] {
    const userStr = sessionStorage.getItem('loggedInUser');
    if (!userStr) return [];
    try {
      const user = JSON.parse(userStr);
      return user.roles || [];
    } catch (e) {
      return [];
    }
  }

  // Determine a default dashboard route based on the user's roles
  getDashboardRoute(): string {
    const roles = this.getUserRoles();
    if (roles.includes('Admin')) {
      return '/admin-dashboard';
    } else if (roles.includes('HR')) {
      return '/hr-dashboard';
    } else if (roles.includes('Candidate') || roles.includes('Interviewer')) {
      return '/candidate-dashboard';
    }
    return '/';
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    // Check for public routes (e.g. login and register) using bracket notation for "public"
    if (route.data && route.data['public']) {
      if (route.data['redirectIfLoggedIn'] && this.isAuthenticated()) {
        this.router.navigate([this.getDashboardRoute()]);
        return false;
      }
      return true;
    }

    // For protected routes, if not authenticated, redirect to login.
    if (!this.isAuthenticated()) {
      this.router.navigate(['/login']);
      return false;
    }

    // If the route requires specific roles, verify that the user has at least one of them.
    const requiredRoles = route.data['roles'] as string[];
    if (requiredRoles && requiredRoles.length > 0) {
      const userRoles = this.getUserRoles();
      const hasRole = requiredRoles.some((role) => userRoles.includes(role));
      if (!hasRole) {
        this.router.navigate([this.getDashboardRoute()]);
        return false;
      }
    }

    return true;
  }
}
