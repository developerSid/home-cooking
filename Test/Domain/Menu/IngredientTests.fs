module House.Imod.Cooking.Test.Domain.Menu.IngredientTests

open House.Imod.Cooking.Core
open House.Imod.Cooking.Core.Domain.Menu
open Xunit
open FsUnit.Xunit
open Bogus

[<Fact>]
let ``constructs a valid ingredient`` () =
  let loremDescription = Faker().Lorem.Sentence()
  let ingredientResult = Ingredient.construct (Some loremDescription)

  match ingredientResult with
  | Ok(ingredient) ->
    let id, description = Ingredient.destruct ingredient

    id
    |> SortableIdentifier.toString
    |> should greaterThan SortableIdentifier.START_ID_STRING

    description |> TrimmedText1000.unwrap |> should equal loremDescription
  | failure -> failwith $"Expected Ok ingredient got {failure}"
