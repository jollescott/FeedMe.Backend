import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { IRecipe, RecipeOwner } from '../store/types';
import { searchRecipesT } from '../store/search/actions';
import { SEARCH_START, SEARCH_SUCCESS } from '../store/search/types';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

const middlewares = [thunk]
const mockStore = configureMockStore(middlewares)
const mock = new MockAdapter(axios);

describe('async actions', () => {

  beforeEach(() => {
    mock.reset();
  })

  it('Load recipes from search query', () => {

    const recipes: IRecipe[] = [];

    recipes.push({
      recipeId: 'test',
      name: 'test',
      desc: 'A test recipe',
      directions: [],
      source: 'www.google.com',
      owner: RecipeOwner.Hemmets,
      ownerLogo: 'www.google.com',
      image: '',
      coverage: 1,
      ingredients: [],
      recipeParts: [],
      tags:[]
    });

    mock.onPost('/https://api.feedmeapp.se/v2/ingredient/suggest').reply(200, recipes);

    const expectedActions = [
      { type: SEARCH_START },
      { type: SEARCH_SUCCESS, recipes }
    ]
    const store = mockStore({ recipes: [] })

    return store.dispatch<any>(searchRecipesT('test')).then(() => {
      // return of async actions
      expect(store.getActions()).toEqual(expectedActions)
    })
  })
})