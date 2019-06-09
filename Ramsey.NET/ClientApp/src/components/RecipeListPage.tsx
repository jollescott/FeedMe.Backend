import React from 'react';
import '../App.css';
import RecipeList from '../components/RecipeList';


export default class RecipeListPage extends React.Component
{
  // Den här sidan visar bara den aktuella receptlistan
  public render() {
    return (
      <div className="page">
        {/* Content */}
        <div className="pageContent">
          <div className="usablePage">
            <RecipeList />
          </div>
        </div>
      </div>
    );
  }
}
