import { Injectable } from "@angular/core";
import { Config } from "../config";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

@Injectable()
export class IdentityService {
  public accessToken: string;
  public refreshToken: string;

  constructor(
    private httpClient: HttpClient
  ) {
    this.getAccessTokenForUser("asd2ws", "wojtek123");
  }

  private getAccessTokenForUser(username: string, password: string) {
    const body = new HttpParams()
      .set("username", username)
      .set("password", password)
      .set("client_id", "client")
      .set("client_secret", "secret")
      .set("scope", "web.all")
      .set("grant_type", "password");

    this.httpClient.post(Config.identityServerUrl + "connect/token",
      body.toString(),
      {
        headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
      }).subscribe(response => {
      const test = response;
    }, error => {
      const test2 = error;
    });
  }

}
