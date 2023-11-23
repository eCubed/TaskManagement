import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainNavComponent } from "./components/main-nav/main-nav.component";
import { HttpClient } from '@angular/common/http';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    standalone: true,
    imports: [CommonModule, RouterOutlet, MainNavComponent]
})
export class AppComponent implements OnInit {
  constructor(private http: HttpClient) {}

  ngOnInit(){
    this.getForecasts()
  }


  getForecasts() {
    this.http.get<WeatherForecast[]>('api/weatherforecast').subscribe({
      next: (result: any) => {

      },
      error: (error: any) => {

      }
    });

  }

  title = 'taskmanagement.webapp.client';
}
