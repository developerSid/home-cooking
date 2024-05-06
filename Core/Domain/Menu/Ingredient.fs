namespace House.Imod.Cooking.Core.Domain.Menu

open House.Imod.Cooking.Core
open House.Imod.Cooking.Core.Error
open FsToolkit.ErrorHandling

type IngredientId = SortableIdentifier

type Ingredient =
  private
    { Id: IngredientId
      Description: TrimmedText1000 }

module Ingredient =
  let construct description =
    description
    |> TrimmedText1000.construct MenuError.IngredientRequired MenuError.IngredientDescriptionTooLong
    |> Result.map (fun description ->
      { Id = SortableIdentifier.newId ()
        Description = description })

  let destruct (ingredient: Ingredient) = (ingredient.Id, ingredient.Description)
