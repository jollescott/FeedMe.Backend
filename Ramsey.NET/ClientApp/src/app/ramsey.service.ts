import { Injectable } from '@angular/core'

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class RamseyService {
  constructor(private http: HttpClient) {
  }

  async getRecipes(ingredients: Ingredient[]): Promise<Recipe[]> {
    const url = window.location.origin + '/recipe/suggest';

    return await this.http.post<Recipe[]>(url, ingredients).toPromise();
  }

  async getIngredients(search: string): Promise<Ingredient[]> {
    const url = window.location.origin + '/ingredient/suggest?search=' + search;
    return await this.http.get<Ingredient[]>(url).toPromise();
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

export interface Ingredient {
  ingredientId: string;
  recipeParts: RecipePart[];
}

export interface RecipePart {
  ingredientId: string;
  recipeId: string;
}

