import React from 'react';
import '../App.css';
import { IRecipe, IIngredient } from '../store/types';
import { AppState } from '../store';
import { connect } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { AnyAction } from 'redux';
import { goForward } from '../store/carousel/actions';
import {
  Card,
  CardActionArea,
  CardMedia,
  Typography,
  CardContent,
  CircularProgress,
  Button,
} from '@material-ui/core';
import { refreshRecipeCount, loadRecipe } from '../store/recipes/actions';
import { SearchMode } from '../misc/enums';
import { searchRecipesI, searchRecipesT } from '../store/search/actions';

interface IRecipeListProps {
  results: IRecipe[];
  loading: boolean;
  error: string;
  recipeCount: number;
  goForward: () => void;
  loadCount: () => void;
  openRecipe: (recipeId: string) => void;
  currentSearchMode: SearchMode;

  query: string;
  ingredients: IIngredient[];

  searchIngredients: (ingredients: IIngredient[], start: number) => void;
  searchText: (query: string, start: number) => void;
}

interface IRecipeListState {
  scrollY: number;
}


// En komponent som listar alla recept i en list eller ett grid. När man trycker på ett recept så kommer receptet att hämtass från serverm och funktionen goForward() kommer att köras.
class RecipeListPage extends React.Component<IRecipeListProps, IRecipeListState> {
  public container: HTMLDivElement | null;

  constructor(props: Readonly<IRecipeListProps>) {
    super(props);

    this.container = null;

    this.openRecipe = this.openRecipe.bind(this);
    this.loadMore = this.loadMore.bind(this);

    this.state = {
      scrollY: 0,
    };
  }

  public componentDidUpdate() {
    if (this.container !== null) {
      this.container.scrollTop = this.state.scrollY;
    }
  }

  public render() {
    return (
      <div className="fullDiv" ref={(x) => (this.container = x)}>
        {this.props.loading && this.props.results.length === 0 ? (

          // När recepten laddar:
          <div className="centerdDiv">
            <div className="verticalAlignment">
              <CircularProgress color="primary" className="loadingIndicator" />
              <h2>Letar bland {this.props.recipeCount} recept!</h2>
            </div>
          </div>
        ) : (
            <div className="gridListContainer">

              {/* Loopa igenom alla recepten och skapa "Cards" */}
              {this.props.results.map((recipe, index) => (
                <div className="gridListItemContainer" key={index}>
                  <Card className="gridListItem">
                    <CardActionArea
                      onClick={() => this.openRecipe(recipe.recipeId)}
                    >
                      <CardMedia
                        className="cardImage"
                        component="img"
                        image={recipe.image}
                        title={recipe.name}
                      />
                      <CardContent className="cardText">
                        <Typography
                          gutterBottom={true}
                          variant="h6"
                          noWrap={true}
                        >
                          {recipe.name}
                        </Typography>
                        <Typography component="p">
                          {this.coverageMessage(recipe)}
                        </Typography>
                      </CardContent>
                    </CardActionArea>
                  </Card>
                </div>
              ))}

              {/* Knappen och laddningsbaren för att ladda in fler recept */}
              {this.props.results.length !== 0 && this.props.results.length % 25 === 0 && (
                <div className="gridListItemContainer" key={-1}>
                  <div className="loadMoreRecipesContainer">
                    <div className="centerdDiv">

                      {this.props.loading ?
                        <CircularProgress color="primary" className="loadingIndicator" />
                        :
                        <Button variant="extendedFab" color="primary" onClick={() => this.loadMore()}>Ladda Fler!</Button>
                      }
                    </div>
                  </div>
                </div>
              )}
            </div>
          )}
      </div>
    );
  }

  // En funktion som returerar en string som berättar hut många % av ingredienserna man har
  private coverageMessage(recipe: IRecipe): string {
    if (recipe.coverage === undefined) { // returera en tom string om det inte finns information om coverage
      return '';
    } else {
      return (
        'Du har ' +
        Math.round(recipe.coverage * 100) +
        '% av alla ingrediensser'
      );
    }
  }

  // Funktionen som laddar in fler recept
  private loadMore() {
    if (this.container !== null) {
      const scroll = this.container.scrollHeight;

      // Spara den possition man har scrollat till
      this.setState({
        scrollY: scroll
      });
    }

    // Ladda in fler recept
    const start = this.props.results.length;
    this.props.currentSearchMode === SearchMode.Ingredients ? this.props.searchIngredients(this.props.ingredients, start)
      : this.props.searchText(this.props.query, start);
  }


  // Funktion som hämtar ett recept och går vidrare till nästa sida
  private openRecipe(recipeID: string) {
    this.props.openRecipe(recipeID);
    this.props.goForward();
  }
}

const mapStateToProps = (state: AppState) => {
  return {
    results: state.search.results,
    loading: state.search.loading,
    error: state.search.error,
    recipeCount: state.recipes.recipeCount,
    currentSearchMode: state.carousel.currentSearchMode,

    ingredients: state.ingredients.ingredients,
    query: state.search.query
  };
};

const mapDispatchToProps = (dispatch: ThunkDispatch<any, any, AnyAction>) => {
  return {
    goForward: () => dispatch(goForward()),
    loadCount: () => dispatch(refreshRecipeCount()),
    openRecipe: (recipeId: string) => dispatch(loadRecipe(recipeId)),

    searchIngredients: (ingredients: IIngredient[], start: number) => dispatch(searchRecipesI(ingredients, start)),
    searchText: (query: string, start: number) => dispatch(searchRecipesT(query, start))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(RecipeListPage);
