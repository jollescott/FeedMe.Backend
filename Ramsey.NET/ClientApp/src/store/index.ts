import { combineReducers } from 'redux';
import { searchReducer } from './search/reducers'
import { ingredientReducer } from './ingredients/reducers';
import { recipesReducer } from './recipes/reducers';
import { carouselReducer } from './carousel/reducers';

export const rootReducer = combineReducers({
  search: searchReducer,
  ingredients: ingredientReducer,
  recipes: recipesReducer,
  carousel: carouselReducer
});

export type AppState = ReturnType<typeof rootReducer>

