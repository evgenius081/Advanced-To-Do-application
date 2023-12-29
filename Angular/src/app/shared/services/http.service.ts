import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(private http: HttpClient) {}

  get<T>(path: string, token: string): Observable<T> {
    return this.http.get<T>(path, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  post<T>(path: string, token: string, data: T): Observable<object> {
    return this.http.post(path, data, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  postWithoutToken<T>(path: string, data: T): Observable<object> {
    return this.http.post(path, data);
  }

  put<T>(path: string, token: string, data: object): Observable<T> {
    return this.http.put<T>(path, data, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  delete(path: string, token: string): Observable<object> {
    return this.http.delete(path, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
