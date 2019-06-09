
export interface IRecipePart {
    ingredientName: string;

    recipeId: string;
    ingredientId: string;

    unit: string;
    quantity: number;
}

export interface IRecipeTag {
    tagId: number;
    name: string;
    previewImage: string;
}

export interface IRecipe {
    recipeId: string;
    name: string;
    desc: string;
    source: string;
    owner: RecipeOwner;
    ownerLogo: string;
    image: string;
    coverage: number | undefined;

    ingredients: IIngredient[];
    directions: string[];
    recipeParts: IRecipePart[];
    tags: IRecipeTag[];
}

export enum RecipeOwner {
    ICA, Hemmets, ReceptSe, Tasteline
}

export function GetOwnerName(recipeOwner: RecipeOwner): string {
    switch (recipeOwner) {
        case RecipeOwner.ICA:
            return "Ica";
        case RecipeOwner.Hemmets:
            return "Hemmets Kokbok";
        case RecipeOwner.ReceptSe:
            return "Recept.se";
        case RecipeOwner.Tasteline:
            return "Tasteline";
    }
}


export enum IngredientRole {
    Include, Exclude
}

export interface IIngredient {
    ingredientId: number;
    role: IngredientRole;
    ingredientName: string;
}
