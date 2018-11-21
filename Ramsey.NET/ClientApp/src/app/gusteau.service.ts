import { Injectable } from '@angular/core'

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GusteauService {
  constructor(private http: HttpClient) {
  }

  async getRecipes(ingredients: string[]): Promise<Recipe[]> {
    const url = window.location.origin + '/recipe/suggest';

    return await this.http.post<Recipe[]>(url, ingredients).toPromise();
  }
}

export interface Recipe {
  recipeID: string;
  name: string;

  ingredients: string[];
  directions: string[];

  desc: string;

  source: string;
  image: string;
}

