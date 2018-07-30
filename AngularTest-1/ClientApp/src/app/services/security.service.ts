import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map, tap, concat } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AppUserAuth } from './app-user-auth';
import { AppUser } from './app-user';

@Injectable()
export class SecurityService {
    securityObject: AppUserAuth = new AppUserAuth();
    public isError: boolean = false;
    public statusCode: number;
    private _baseUrl: string;
    tokenObject: TokenObject = new TokenObject();
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
       
        this._baseUrl = baseUrl;
    }

    login(entity: AppUser): Observable<AppUserAuth> {
        // Initialize security object
        this.resetSecurityObject();

        // Use object assign to update the current object
        // NOTE: Don't create a new AppUserAuth object
        //       because that destroys all references to object
     
        if (this.securityObject.userName !== "") {
            // Store token into local storage
            var allCookies = document.cookie;
            console.log(allCookies);
            localStorage.setItem("bearerToken",
                this.securityObject.accessToken);
        }

        return of<AppUserAuth>(this.securityObject);
    }

    logout(): void {
        this.resetSecurityObject();
    }
    getAccessToken(): Observable<string>  {
      return this.http.get<string>(this._baseUrl + 'api/access-token').pipe(
            tap(resp => {
                // Use object assign to update the current object
                // NOTE: Don't create a new AppUserAuth object
                //       because that destroys all references to object

                Object.assign(this.securityObject, resp);
              //let p = Object.assign({}, this.tokenObject, resp);
                //console.log(p.accessToken, 'Access Token');
                // Store into local storage
                localStorage.setItem("bearerToken",
                    this.securityObject.accessToken);
                
            }),
            catchError(this.handleError<string>('getAccessToken')));
    }
    resetSecurityObject(): void {
        this.securityObject.userName = "";
        this.securityObject.accessToken = "";
        this.securityObject.isAuthenticated = false;

        this.securityObject.isAuthenticated = false;
        this.securityObject.isAdmin = false;

        localStorage.removeItem("bearerToken");
    }

    /**
* Handle Http operation that failed.
* Let the app continue.
* @param operation - name of the operation that failed
* @param result - optional value to return as the observable result
*/
    private handleError<T>(operation = 'operation', result?: T) {
        return (err: HttpErrorResponse): Observable<T> => {
            this.isError = true;
            this.statusCode = err.status;
            // TODO: send the error to remote logging infrastructure
            console.error(err); // log to console instead

            // TODO: better job of transforming error for user consumption
            console.log(`${operation} failed: ${err.message}`);

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}

export class TokenObject {
    accessToken: any;
}
