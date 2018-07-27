//import { Observable } from "rxjs/Observable";
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
//import { of } from "rxjs/observable/of";
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { FeatureFunding } from "../models/featurefunding";
import { FundingUser } from "../models/fundinguser";

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable()
export class FundingService {
  private _baseUrl: string;
  public isError: boolean = false;
  public statusCode: number;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
  }
  addFunding(entity: FundingUser): Observable<FundingUser> {
    return this.http.post<FundingUser>(this._baseUrl + 'api/'
      , entity
      , httpOptions)
      .pipe(
      tap((entity: FundingUser) => console.log(`added FundingUser `)),
      catchError(this.handleError<FundingUser>('addFunding'))
      );
  }
  getFeatureFunding(id: number): Observable<FeatureFunding> {
    return this.http.get<FeatureFunding>(this._baseUrl + 'api/fundings/' + id)
      .pipe(
        tap(lookUp => console.log(`fetched-item`, lookUp)),
      catchError(this.handleError<FeatureFunding>(`getFeatureFunding and id=${id}`))
      );
  }
  getFeatureFundings(): Observable<FeatureFunding[]> {
    return this.http.get<FeatureFunding[]>(this._baseUrl + 'api/fundings/')
      .pipe(
        tap(lookUp => console.log(`fetched-items`, lookUp)),
        catchError(this.handleError<FeatureFunding[]>(`getFeatureFunding `))
      );
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
