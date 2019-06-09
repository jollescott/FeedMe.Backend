import { SearchActionTypes } from "./types";
import { ThunkAction } from "redux-thunk";
import axios from 'axios';
import {AppState} from '../index';
import { Action } from "redux";
import { IRecipe, IIngredient } from "../types";

//#region Search

export function searchStart(): SearchActionTypes{
    return {
        type: 'SEARCH_START',
    }
}

export function searchSuccess(recipes: IRecipe[], fresh: boolean = true) : SearchActionTypes{
    return {
        recipes,
        type: 'SEARCH_SUCCESS',
        fresh
    }
}

export function searchFailure(error: string) : SearchActionTypes{
    return {
        error,
        type: 'SEARCH_FAILURE'
    }
}

export function searchClear(): SearchActionTypes{
    return {
        type: 'SEARCH_CLEAR'
    }
}

export const searchRecipesT = (searchTerm: string, start: number = 0): ThunkAction<void, AppState, null, Action<string>> => async dispatch => {
    dispatch(searchStart());
    axios.post<IRecipe[]>(`https://api.feedmeapp.se/v2/recipe/text?search=${searchTerm}&start=${start}`)
        .then(resp => {
            if(resp.status === 200){
                dispatch(searchSuccess(resp.data, start === 0));
            } else{
                dispatch(searchFailure(resp.statusText));
            }
        })
        .catch(e => {
            dispatch(searchFailure(e));
        });
}

export const searchRecipesI = (ingredients: IIngredient[], start: number = 0): ThunkAction<void, AppState, null, Action<string>> => async dispatch => {
    dispatch(searchStart());
    axios.post<IRecipe[]>(`https://api.feedmeapp.se/v2/recipe/suggest?start=${start}`, ingredients)
        .then(resp => {
            if(resp.status === 200) {
                dispatch(searchSuccess(resp.data, start === 0));
            }
            else{
                dispatch(searchFailure(resp.statusText));
            }
        })
        .catch(e => {
            dispatch(searchFailure(e));
        });
}

export function setQuery(query: string): SearchActionTypes {
    return{
        type: 'SET_QUERY',
        query
    }
}

//#endregion
