import React from 'react';
import '../App.css';
import { IRecipe, IRecipePart, GetOwnerName } from '../store/types';
import { AppState } from '../store';
import { connect } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { AnyAction } from 'redux';
import { goBack } from '../store/carousel/actions';
import { Card, CardMedia, CardContent, Typography, Divider, CardActionArea, CircularProgress } from '@material-ui/core';

interface IRecipeProps {
    recipe: IRecipe | undefined;
    error: string;
    loading: boolean;
    goBack: () => void;
}
interface IRecipeState {
    test: string;
}

class RecipePage extends React.Component<IRecipeProps, IRecipeState>
{
    [x: string]: any;
    constructor(props: Readonly<IRecipeProps>) {
        super(props);

        this.state = ({
            test: "test"
        });
    }

    public render() {
        return (
            <div className="page">
                {/* Content */}
                <div className="pageContent">
                    <div className="usablePage">
                        {this.props.loading ?  // Om receptet måste laddaa
                            <div className="fullDiv">
                                <div className="centerdDiv">
                                    <CircularProgress color="primary" className="loadingIndicator" />
                                </div>
                            </div>
                            :  // Om receptet är färdigladdat
                            this.renderRecipe()
                        }
                    </div>

                </div>
            </div>
        );
    }

    // Renderar resept sida
    private renderRecipe() {
        // Om receptet har hämtats korrekt
        if (this.props.recipe !== undefined && this.props.recipe != null) {
            const recipe = this.props.recipe;
            return (
                <div className="doubleColumnContainer">
                    
                    {/* Vänster sida */}
                    <div className="doubleColumnColumn">
                        <Card className="recipeInstructionCard">

                            {/* Recept bild */}
                            <CardMedia
                                component="img"
                                image={recipe.image}
                                title={recipe.name}
                            />

                            <CardContent>
                                {/* Recept namn */}
                                <Typography className="recipeNameText" gutterBottom={true} variant="h5">
                                    {recipe.name}
                                </Typography>

                                {/* Kort med information om sidan där receptet är hämtat från */}
                                <Card className="ownerInfoCard" onClick={() => window.open(recipe.source)}>
                                    <CardActionArea className="ownerInfoCardContent">
                                        <Typography variant="h6" className="centerText">
                                            {"Receptet är hämtat från " + GetOwnerName(recipe.owner)}
                                        </Typography>
                                        <br />
                                        <Divider variant="fullWidth" />
                                        <br />
                                        <div className="ownerInfoLower">
                                            <img src={recipe.ownerLogo} className="ownerLogo" />

                                            <Typography component="p" className="link">
                                                {this.formateLink(recipe.source)}
                                            </Typography>
                                        </div>
                                    </CardActionArea>
                                </Card>
                            </CardContent>
                        </Card>
                    </div>
                    
                    {/* Höger sida */}
                    <div className="doubleColumnColumn">

                        {/* Kort med alla ingredienser */}
                        <Card className="recipeInstructionCard">
                            <CardContent className="cardText">
                                <Typography gutterBottom={true} variant="h6">
                                    Ingredienser
                                </Typography>

                                <div className="recipeIngredientListLeft">
                                    <Divider variant="fullWidth" />
                                    {recipe.recipeParts.map((recipePart, index) => (  // Loopar igenom alla ingredienser
                                        <div key={index}>
                                            <div className="recipeIngredientListRow">
                                                <div className="recipeIngredientListLeftColumn">
                                                    <Typography component="p">
                                                        {this.formatIngredientQuantityAndUnit(recipePart)}
                                                    </Typography>
                                                </div>
                                                <div className="recipeIngredientListRightColumn">
                                                    <Typography component="p">
                                                        {recipePart.ingredientName}
                                                    </Typography>
                                                </div>
                                            </div>
                                            <Divider variant="fullWidth" />
                                        </div>
                                    ))}
                                </div>
                            </CardContent>
                        </Card>

                        {/* Alla kort som innehåller tillagningsinstruktioner */}
                        {recipe.directions.map((direction, index) => (
                            <Card className="recipeInstructionCard" key={index}>
                                <CardContent className="cardText">
                                    <Typography variant="h6">
                                        {"Steg " + (index + 1) + "."}
                                    </Typography>
                                    <Typography component="p">
                                        {this.formatRecipeDirection(direction)}
                                    </Typography>
                                </CardContent>
                            </Card>
                        ))}
                    </div>
                </div>
            );
        }
        // Om receptet inte hämtats ordentligt
        else {
            return (
                <div className="fullDiv">
                    <div className="centerdDiv">
                        <Card className="recipeInstructionCard">
                            <Typography variant="h6">
                                {"Kunde inte hämta receptet"}
                            </Typography>
                        </Card>
                    </div>
                </div>
            );
        }
    }


    // Funktion som tar bort "https//" från länkar
    private formateLink(link: string): string {
        link = link.replace("https://", "")
        return link;
    }


    // Funktion som ser till att en mängd och enhet ser bra ut
    private formatIngredientQuantityAndUnit(recipePart: IRecipePart): string {
        if (recipePart.quantity === 0 ||recipePart.unit === "odefinierad") {
            return "";
        }
        else if (recipePart.unit === recipePart.ingredientName) {
            return String(recipePart.quantity);
        }
        else {
            return recipePart.quantity + " " + recipePart.unit;
        }
    }

    // Funktion som ser till att en receptinstruktion ser bra ut
    private formatRecipeDirection(direction: string): string {
        direction = direction.trim();
        // Tar bort receptinstruktionsnummer om det finns.
        while (this.stringIsNumber(direction[0])) {
            direction = direction.slice(1, direction.length - 1);
        }
        return direction;
    }

    // Returerar true om stringen är ett nummer
    private stringIsNumber(value: string): boolean {
        return ((value != null) && !isNaN(Number(value)));
    }
}


const mapStateToProps = (state: AppState) => {
    return {
        recipe: state.recipes.recipe,
        error: state.recipes.error,
        loading: state.recipes.loading,
    };
};

const mapDispatchToProps = (dispatch: ThunkDispatch<any, any, AnyAction>) => {
    return {
        goBack: () => dispatch(goBack()),
    }
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RecipePage);
