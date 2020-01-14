import { Component, OnInit } from '@angular/core';
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';
// import { User } from 'oidc-client';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [Location, {provide: LocationStrategy, useClass: PathLocationStrategy}],
})
export class AppComponent {
  constructor(
    location: Location,
    private authService: AuthService,
    private httpClient: HttpClient) {
    location.replaceState('/');
  }

  callAPI() {
    this.httpClient.get(environment.apiURL)
      .subscribe(
        data => {
          console.log('SUCCESS: ' + JSON.stringify(data))
          console.log(this.authService);
        },
        error => console.log('ERROR: ' + JSON.stringify(error))
      );
  }

  signout() {
    this.authService.signout();
  }
}
