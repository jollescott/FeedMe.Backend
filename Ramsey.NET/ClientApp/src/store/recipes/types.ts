import { IRecipe } from "../types";

export interface IRecipeState{
    recipe: IRecipe | undefined,
    recipeCount: number,
    loading: boolean,
    error: string
}

//#region Recipes

export const LOAD_RECIPE_START = 'LOAD_RECIPE_START';

interface ILoadRecipeStartAction {
    type: typeof LOAD_RECIPE_START;
}

export const LOAD_RECIPE_SUCCESS = 'LOAD_RECIPE_SUCCESS';

interface ILoadRecipeSuccessAction {
    type: typeof LOAD_RECIPE_SUCCESS,
    recipe: IRecipe
}

export const LOAD_RECIPE_FAILURE = 'LOAD_RECIPE_FAILURE';

interface ILoadRecipeFailureAction {
    type: typeof LOAD_RECIPE_FAILURE,
    error: string
}

//#endregion

//#region Count

export const LOAD_COUNT_START = 'LOAD_COUNT_START';

interface ILoadCountStartAction{
    type: typeof LOAD_COUNT_START;
}

export const LOAD_COUNT_SUCCESS = 'LOAD_COUNT_SUCCESS';

interface ILoadCountSuccessAction{
    type: typeof LOAD_COUNT_SUCCESS;
    count: number;
}

export const LOAD_COUNT_FAILURE = 'LOAD_COUNT_FAILURE';

interface ILoadCountFailureAction{
    type: typeof LOAD_COUNT_FAILURE;
    error: string;
}

//#endregion

export type RecipeActionTypes = ILoadRecipeStartAction | ILoadRecipeSuccessAction | ILoadRecipeFailureAction | 
                                ILoadCountStartAction | ILoadCountSuccessAction | ILoadCountFailureAction;