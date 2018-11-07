import { Component } from '@angular/core'
import { GusteauService, Ingredient, Recipe } from '../gusteau.service'

@Component({
  selector: 'test-editor',
  templateUrl: './test.component.html'
})
export class TestComponent {
  search: string = '';
  suggestions: Ingredient[];
  recipes: Recipe[];

  ingredients: Ingredient[] = new Array<Ingredient>();

  constructor(private gusteau: GusteauService) {

  }

  searchChanged(event): void{
    this.gusteau.getSuggestions(this.search).then(data => {
      this.suggestions = data;
    });
  }

  onSuggestionSelect(suggestion: Ingredient): void {
    this.ingredients.push(suggestion);
  }

  onIngredientSelect(suggestion: Ingredient): void {
    const index = this.ingredients.indexOf(suggestion, 0);
    if (index > -1) {
      this.ingredients.splice(index, 1);
    }
  }

  findRecipe(): void {
    if (this.ingredients.length > 0) {
      this.gusteau.getRecipes(this.ingredients).then(data => {
        this.recipes = data;
      })
    }
  }
}
