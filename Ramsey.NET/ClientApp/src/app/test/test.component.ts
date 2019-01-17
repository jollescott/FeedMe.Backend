import { Component, Pipe, PipeTransform } from '@angular/core'
import { RamseyService, Recipe, Ingredient } from '../services/ramsey.service';

@Component({
  selector: 'test-editor',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent {
  search: string = '';
  recipes: Recipe[];

  ingredients: Ingredient[] = new Array<Ingredient>();
  selected: Ingredient[] = new Array<Ingredient>();

  constructor(private ramsey: RamseyService) {

  }

  onIngredientSelect(suggestion: Ingredient): void {
    const index = this.selected.indexOf(suggestion, 0);
    if (index > -1) {
      this.selected.splice(index, 1);
    }
  }

  onSearchSelect(suggestion: Ingredient): void {
    this.selected.push(suggestion);
  }

  findIngredients(): void {
    this.ramsey.getIngredients(this.search).then(data => {
      this.ingredients = data;
    });
  }

  findRecipe(): void {
    if (this.ingredients.length > 0) {
      this.ramsey.getRecipes(this.selected).then(data => {
        this.recipes = data;
      })
    }
  }

  viewRecipe(id: string) {
    window.open(window.location.origin + "/recipe/retrieve?id=" + id)
  }
}
