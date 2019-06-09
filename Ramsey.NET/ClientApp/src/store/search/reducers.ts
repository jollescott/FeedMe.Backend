import { ISearchState, SearchActionTypes } from "./types";

const initialState: ISearchState = {
    error: '',
    query: '',
    loading: false,
    results: [],
}

export function searchReducer(state = initialState, action: SearchActionTypes): ISearchState {
    switch (action.type) {
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
            if(action.fresh){
                return {
                    ...state,
                    results: action.recipes,
                    loading: false
                }
            }
            else{
                const recipes = state.results;
                action.recipes.map(x => recipes.push(x));

                return{
                    ...state,
                    results: recipes,
                    loading: false
                }
            }
        case 'SET_QUERY':
            return {
                ...state,
                query: action.query
            }
        case 'SEARCH_CLEAR':
            return {
                ...state,
                results: []
            }
        default:
            return state;
    }
}