import { IRecipeState, RecipeActionTypes } from "./types";

const initialState: IRecipeState = {
    error: '',
    recipeCount: 0,
    loading: false,
    recipe: undefined
}

export function recipesReducer(state = initialState, action: RecipeActionTypes) : IRecipeState {
    switch(action.type){
        case 'LOAD_RECIPE_START':
            return {
                ...state,
                loading: true
            };
        case 'LOAD_RECIPE_FAILURE':
            return {
                ...state,
                loading: false,
                error: action.error
            };
        case 'LOAD_RECIPE_SUCCESS':
            return {
                ...state,
                recipe: action.recipe,
                loading: false
            }
        case 'LOAD_COUNT_START':
            return {
                ...state,
                loading: true
            }
        case 'LOAD_COUNT_SUCCESS':
            return {
                ...state,
                recipeCount: action.count
            }
        case 'LOAD_COUNT_FAILURE': 
            return {
                ...state,
                error: action.error
            }
        default: 
            return state;
    }
}