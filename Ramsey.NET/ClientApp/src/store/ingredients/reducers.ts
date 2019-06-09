import { IIngredientState, IngredientActionTypes } from "./types";
import { IIngredient } from "../types";

const initialState: IIngredientState = {
    error: '',
    loading: false,
    results: [],
    ingredients: []
}

export function ingredientReducer(state = initialState, action: IngredientActionTypes) : IIngredientState {
    switch(action.type){
        case 'SEARCH_START':
            return {
                ...state,
                loading: true
            };
        case 'SEARCH_FAILURE':
            return {
                ...state,
                loading: false,
                error: action.error
            };
        case 'SEARCH_SUCCESS':
            return {
                ...state,
                results: action.recipes,
                loading: false
            }
        case 'ADD_INGREDIENT':
            const aIngredients = new Array<IIngredient>(...state.ingredients);
            const exists = aIngredients.filter(x => x.ingredientId === action.ingredient.ingredientId);

            if(exists.length <= 0) {
                aIngredients.push(action.ingredient);
            }

            return {
                ...state,
                ingredients: aIngredients
            }
        case 'REMOVE_INGREDIENT':
            const rIngredients = new Array<IIngredient>(...state.ingredients);
            rIngredients.splice(rIngredients.indexOf(action.ingredient), 1);
        
            return {
                ...state,
                ingredients: rIngredients
            }
        default: 
            return state;
    }
}