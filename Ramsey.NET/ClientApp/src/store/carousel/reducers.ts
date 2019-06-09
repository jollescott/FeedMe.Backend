import { ICarouselState, CarouselActionTypes } from "./types";
import { SearchMode } from "../../misc/enums";

const initialState: ICarouselState = {
    pageIndex: 0,
    currentSearchMode: SearchMode.Ingredients
};

export function carouselReducer(state = initialState, action: CarouselActionTypes): ICarouselState {
    switch (action.type) {
        case 'GO_BACK':
            return {
                ...state,
                pageIndex: state.pageIndex - 1
            }
        case 'GO_FORWARD':
            return {
                ...state,
                pageIndex: state.pageIndex + 1
            }
        case 'GO_HOME':
            return {
                ...state,
                pageIndex: 0
            }
        case 'SET_MODE':
            return {
                ...state,
                currentSearchMode: action.mode
            }
        default:
            return state;
    }
}