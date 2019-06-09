import { IIngredient } from "../types";
import { IngredientActionTypes } from "./types";
import { ThunkAction } from "redux-thunk";
import { AppState } from "..";
import { Action } from "redux";
import axios from 'axios';

export function addIngredient(ingredient: IIngredient) : IngredientActionTypes{
    return {
        type: 'ADD_INGREDIENT',
        ingredient
    }
}

export function removeIngredient(ingredient: IIngredient) : IngredientActionTypes{
    return {
        type: 'REMOVE_INGREDIENT',
        ingredient
    }
}

export function searchStart(): IngredientActionTypes{
    return {
        type: 'SEARCH_START',
    }
}

export function searchSuccess(recipes: IIngredient[]) : IngredientActionTypes{
    return {
        recipes,
        type: 'SEARCH_SUCCESS'
    }
}

export function searchFailure(error: string) : IngredientActionTypes{
    return {
        error,
        type: 'SEARCH_FAILURE'
    }
}

export const searchIngredients = (search: string): ThunkAction<void, AppState, null, Action<string>> => async dispatch => {
    dispatch(searchStart());
    axios.post<IIngredient[]>(`https://api.feedmeapp.se/v2/ingredient/suggest?search=${search}`)
        .then(resp => {
            if(resp.status === 200) {
                dispatch(searchSuccess(resp.data));
            }
            else{
                dispatch(searchFailure(resp.statusText));
            }
        })
        .catch(e => {
            dispatch(searchFailure(e));
        });
}