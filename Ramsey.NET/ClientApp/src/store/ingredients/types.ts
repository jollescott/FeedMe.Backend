import { IIngredient } from "../types";

export interface IIngredientState{
    ingredients: IIngredient[],
    results: IIngredient[],
    loading: boolean,
    error: string
}

export const SEARCH_START = 'SEARCH_START';

interface ISearchStartAction {
    type: typeof SEARCH_START;
}

export const SEARCH_SUCCESS = 'SEARCH_SUCCESS';

interface ISearchSuccessAction {
    type: typeof SEARCH_SUCCESS,
    recipes: IIngredient[]
}

export const SEARCH_FAILURE = 'SEARCH_FAILURE';

interface ISearchFailureAction {
    type: typeof SEARCH_FAILURE,
    error: string
}

export const ADD_INGREDIENT = 'ADD_INGREDIENT';

interface IAddIngredientAction {
    type: typeof ADD_INGREDIENT,
    ingredient: IIngredient
}

export const REMOVE_INGREDIENT = 'REMOVE_INGREDIENT';
interface IRemoveIngredientAction {
    type: typeof REMOVE_INGREDIENT,
    ingredient: IIngredient
}

export type IngredientActionTypes = ISearchStartAction | ISearchSuccessAction | ISearchFailureAction
                                    | IAddIngredientAction | IRemoveIngredientAction;