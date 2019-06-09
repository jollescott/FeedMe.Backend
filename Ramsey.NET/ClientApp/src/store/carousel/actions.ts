import { CarouselActionTypes } from "./types";
import { SearchMode } from "../../misc/enums";

export function goForward() : CarouselActionTypes{
    return {
        'type': 'GO_FORWARD'
    }
}

export function goBack(): CarouselActionTypes{
    return {
        'type': 'GO_BACK'
    }
}

export function goHome(): CarouselActionTypes{
    return{
        'type': 'GO_HOME'
    }
}

export function setMode(mode: SearchMode): CarouselActionTypes{
    return{
        'type': 'SET_MODE',
        'mode': mode
    }
}