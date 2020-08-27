import { Injectable } from "@angular/core";
import { Config } from "../config";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class IdentityService {
  private accessToken$ = new BehaviorSubject<string>(null);
  private accessTokenExpirationTimestamp: number;

  constructor(
    private httpClient: HttpClient
  ) {
    const accessToken = localStorage.getItem(Config.localStorageAccessTokenKey);
    if (accessToken) {
      this.accessToken$ = new BehaviorSubject<string>(accessToken);
    }
    this.accessToken$.subscribe(token => {
      localStorage.setItem(Config.localStorageAccessTokenKey, token);
    });
  }

  public logIn(username: string, password: string) {
    const body = new HttpParams()
      .set("username", username)
      .set("password", password)
      .set("client_id", "client")
      .set("client_secret", "secret")
      .set("scope", "web.all")
      .set("grant_type", "password");

    //TODO: Error handling.
    this.httpClient.post(Config.identityServerUrl + "connect/token",
      body.toString(),
      {
        headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
      }).subscribe(response => {
        this.accessToken$.next(response["access_token"]);
        this.accessTokenExpirationTimestamp = Date.now() + parseInt(response["expires_in"]) * 1000;
      }, error => {
        console.log(error);
      });
  }

  public getAccessToken() {
    if (this.isAccessTokenValid()) {
      return this.accessToken$.value;
    }
    return null;
  }

  public isAccessTokenValid() {
    return this.accessTokenExpirationTimestamp && Date.now() < this.accessTokenExpirationTimestamp;
  }
}
