import React from 'react';
import '../App.css';
import { SearchMode } from '../misc/enums';
import Button from '@material-ui/core/Button';
import { connect } from 'react-redux';
import { AppState } from '../store';
import { ThunkDispatch } from 'redux-thunk';
import { AnyAction } from 'redux';
import { goForward, setMode } from '../store/carousel/actions';


interface IStartProps {
  currentSearchMode: SearchMode;
  setMode: (mode: SearchMode) => void;
  goForward: () => void;
}
class StartPage extends React.Component<IStartProps> {
  public render() {
    return (
      <div className="page">

        <div className="pageContent">
          <div className="usablePage">
            <div className="centerdDiv">
              <div className="slimDiv">
                <h1 className="centerText">Sök recept med...</h1>
                <br/><br/>
                {this.renderButton(SearchMode.Ingredients, 'Ingredienser')}
                {this.renderButton(SearchMode.Name, 'Recept Namn')}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Renderar en knapp som byter till det specificerade SearchModet och byter sida när man trucker på den 
  private renderButton(searchMode: SearchMode, name: string) {
    return (
      <Button
        variant="contained"
        color={searchMode === this.props.currentSearchMode ? "primary" : "default"}
        onClick={() => this.nextPage(searchMode)}
        fullWidth={true}
        className="menuButton"
      >
        {name}
      </Button>
    );
  }

  // Gå till nästa sida
  private nextPage(searchMode: SearchMode): void {
    // sätt sök-mode till det valda modet
    this.props.setMode(searchMode);
    // gå till nästa sida
    this.props.goForward();
  }
}

const mapStateToProps = (state: AppState) => {
  return {
    currentSearchMode: state.carousel.currentSearchMode
  };
};

const mapDispatchToProps = (dispatch: ThunkDispatch<any, any, AnyAction>) => {
  return {
    goForward: () => dispatch(goForward()),
    setMode: (mode: SearchMode) => dispatch(setMode(mode))
  }
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(StartPage);