import { IRecipe } from "../types";

export interface ISearchState{
    results: IRecipe[],
    query: string,
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
    recipes: IRecipe[],
    fresh: boolean
}

export const SEARCH_FAILURE = 'SEARCH_FAILURE';

interface ISearchFailureAction {
    type: typeof SEARCH_FAILURE,
    error: string
}

export const SET_QUERY = 'SET_QUERY';

interface ISetQueryAction{
    type: typeof SET_QUERY,
    query: string
}

export const SEARCH_CLEAR = 'SEARCH_CLEAR';

export interface ISearchClearAction {
    type: typeof SEARCH_CLEAR;    
}

export type SearchActionTypes = ISearchStartAction | ISearchSuccessAction | ISearchFailureAction | ISearchClearAction | ISetQueryAction;
