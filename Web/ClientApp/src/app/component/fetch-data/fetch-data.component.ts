import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private identityService: IdentityService
  ) {
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast')
      .subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
