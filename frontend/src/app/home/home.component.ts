import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HomeService } from '../shared/services/home.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  weatherData: any;

  constructor(private router: Router, private homeService: HomeService) {}

  ngOnInit(): void {
    this.getAllWeatherData();
  }
  getAllWeatherData() {
    this.homeService.weatherData().subscribe(
      (data) => {
        this.weatherData = data;
        console.log('weather', this.weatherData);
      },
      (error) => {
        console.log('Somthing went wrong ', error);
      }
    );
  }
}
