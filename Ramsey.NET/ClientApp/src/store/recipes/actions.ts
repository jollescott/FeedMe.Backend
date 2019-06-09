import { RecipeActionTypes } from "./types";
import { IRecipe } from "../types";
import { ThunkAction } from "redux-thunk";
import axios from 'axios';
import { AppState } from "..";
import { Action } from "redux";

//#region Search

export function loadRecipeStart(): RecipeActionTypes{
    return {
        type: 'LOAD_RECIPE_START',
    }
}

export function loadRecipeSuccess(recipe: IRecipe) : RecipeActionTypes{
    return {
        recipe,
        type: 'LOAD_RECIPE_SUCCESS'
    }
}

export function loadRecipeFailure(error: string) : RecipeActionTypes{
    return {
        error,
        type: 'LOAD_RECIPE_FAILURE'
    }
}

export const loadRecipe = (recipeId: string): ThunkAction<void, AppState, null, Action<string>> => async dispatch => {
    dispatch(loadRecipeStart());
    axios.post<IRecipe>(`https://api.feedmeapp.se/v2/recipe/retrieve?id=${recipeId}`)
        .then(resp => {
            if(resp.status === 200){
                dispatch(loadRecipeSuccess(resp.data));
            } else{
                dispatch(loadRecipeFailure(resp.statusText));
            }
        })
        .catch(e => {
            dispatch(loadRecipeFailure(e));
        });
}

//#endregion

//#region Count

export function loadCountStart(): RecipeActionTypes{
    return {
        type: 'LOAD_COUNT_START',
    }
}

export function loadCountSuccess(count: number) : RecipeActionTypes{
    return {
        count,
        type: 'LOAD_COUNT_SUCCESS'
    }
}

export function loadCountFailure(error: string) : RecipeActionTypes{
    return {
        error,
        type: 'LOAD_COUNT_FAILURE'
    }
}

export const refreshRecipeCount = (): ThunkAction<void, AppState, null, Action<string>> => async dispatch => {
    axios.post<any>(`https://api.feedmeapp.se/v2/meta/recipeCount`)
        .then(resp => {
            if(resp.status === 200){
                dispatch(loadCountSuccess(resp.data));
            } else{
                dispatch(loadCountFailure(resp.statusText));
            }
        })
        .catch(e => {
            dispatch(loadRecipeFailure(e));
        });
}

//#endregion