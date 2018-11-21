import { Component, Pipe, PipeTransform } from '@angular/core'
import { GusteauService, Recipe } from '../gusteau.service'

@Component({
  selector: 'test-editor',
  templateUrl: './test.component.html'
})
export class TestComponent {
  search: string = '';
  recipes: Recipe[];

  ingredients: string[] = new Array<string>();

  constructor(private gusteau: GusteauService) {

  }

  onIngredientSelect(suggestion: string): void {
    const index = this.ingredients.indexOf(suggestion, 0);
    if (index > -1) {
      this.ingredients.splice(index, 1);
    }
  }

  addIng(): void {
    this.ingredients.push(this.search);
    this.search = "";
  }

  findRecipe(): void {
    if (this.ingredients.length > 0) {
      this.gusteau.getRecipes(this.ingredients).then(data => {
        this.recipes = data;
      })
    }
  }
}
