import { SearchMode } from "../../misc/enums";

export interface ICarouselState{
    pageIndex: number;
    currentSearchMode: SearchMode;
}

export const GO_FORWARD = 'GO_FORWARD';

interface IGoForwardAction {
    type: typeof GO_FORWARD;
}

export const GO_BACK = 'GO_BACK';

interface IGoBackAction {
    type: typeof GO_BACK
}

export const GO_HOME = 'GO_HOME';

interface IGoHomeAction {
    type: typeof GO_HOME
}

export const SET_MODE = 'SET_MODE';

interface ISetHomeAction{
    type: typeof SET_MODE;
    mode: SearchMode;
}

export type CarouselActionTypes = IGoForwardAction | IGoBackAction | IGoHomeAction | ISetHomeAction;