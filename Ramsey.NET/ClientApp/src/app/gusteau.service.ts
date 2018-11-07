import { Injectable } from '@angular/core'

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GusteauService {
  constructor(private http: HttpClient) {
  }

  async getSuggestions(search: string): Promise<Ingredient[]> {
    const url = window.location.origin + '/ingredient/suggest?search=' + search;
    console.log(url);

    return await this.http.get<Ingredient[]>(url).toPromise();
  }

  async getRecipes(ingredients: Ingredient[]): Promise<Recipe[]> {
    const url = window.location.origin + '/recipe/suggest';

    return await this.http.post<Recipe[]>(url, ingredients).toPromise();
  }
}

export interface Ingredient {
  ingredientID: number;
  name: string;
  recipeParts: RecipePart[];
}

export interface RecipePart {
  recipePartID: number;

  ingredientID: number;
  recipeID: number;

  decimal: number;
}

export interface Recipe {
  recipeID: string;
  name: string;

  recipeParts: RecipePart[];
  directions: RecipeDirection[];
  categories: RecipeCategory[];

  fat: number;
  desc: number;
  rating: number;
  sodium: number;
}

export interface RecipeCategory {
  categoryID: number;
  name: string;
  recipeID: number;
}

export interface RecipeDirection {
  directionID: number;
  instruction: string;

  recipeID: number;
}
