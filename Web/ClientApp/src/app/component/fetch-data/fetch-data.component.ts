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
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast',
      {
        headers: { 'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4RkUxRDZGQzY2RDNGRjUwMjQ4MUUxNjhBODFDMDg0IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE1OTg0NjMyODQsImV4cCI6MTU5ODQ2Njg4NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6IndlYiIsImNsaWVudF9pZCI6ImNsaWVudCIsInN1YiI6ImRiY2Q2N2JiLWUwY2EtNGUzZS04YzBmLTM3NTYyZTUxYzYyOSIsImF1dGhfdGltZSI6MTU5ODQ2MzI2OCwiaWRwIjoibG9jYWwiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiYWRtaW4iLCJtb2RlcmF0b3IiXSwianRpIjoiQTZCRjQzNkZFNDk0OEQ1ODNDQjA0NTFCMDQ2NjMwNkMiLCJpYXQiOjE1OTg0NjMyODQsInNjb3BlIjpbIndlYi5hbGwiXSwiYW1yIjpbInB3ZCJdfQ.Rt3JsTMHPOHWYt2VbauKa6eUoVdBIAVY3mnAwOSB-AyiZwSZ-GPSGpgnaeVi-d0FgPsk1pAZrKej31944MN3rtK1_snAk3lO4tJkh4kJmOM0eO_TK3RVOYznOEWEKNvMyTN4x9Oo0k7brFpQRTg6UoKkIa5Fldx_762pNpc38jztNXgP-zATFWI8t6vzx-3gAioXUMLNzw7QowSC_42k_G8F9nXPZjH7E_LQ64q0O_eRRnNvTRfJdfTG8ernCGSnsmCTeTrO6Dt7ngDFNzqIOiGH-KWGQn1qIC2fTtt-ZaMiS2S4m4FNcoSej8Qs8XP0-TjsKt5MRWsCnKikxGFe5Q' }
      })
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
