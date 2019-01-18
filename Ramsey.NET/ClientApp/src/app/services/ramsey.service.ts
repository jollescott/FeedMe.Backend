import { Injectable } from '@angular/core'

import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class RamseyService {
  constructor(private http: HttpClient) {
  }

  async getRecipes(ingredients: Ingredient[]): Promise<Recipe[]> {
    const url = window.location.origin + '/v2/recipe/suggest';

    return await this.http.post<Recipe[]>(url, ingredients).toPromise();
  }

  async getIngredients(search: string): Promise<Ingredient[]> {
    const url = window.location.origin + '/v2/ingredient/suggest?search=' + search;
    return await this.http.get<Ingredient[]>(url).toPromise();
  }

  async reindex(): Promise<any> {
    const url = window.location.origin + '/admin/reindex';
    return await this.http.post(url, null).toPromise();
  }
}

export interface Recipe {
  recipeId: string;
  name: string;

  source: string;
  image: string;

  ingredients: string[];
  recipeParts: RecipePart[];

  coverage: number;
}

export interface Ingredient {
  ingredientId: string;
  role: IngredientRole;
  recipeParts: RecipePart[];
}

export enum IngredientRole {
  Include, Exclude 
}

export interface RecipePart {
  ingredientId: string;
  recipeId: string;

  unit: string;
  quantity: number;
}

