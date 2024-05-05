namespace House.Imod.Cooking.Core.Domain

open House.Imod.Cooking.Core

type IngredientId = SortableIdentifier

type Ingredient = {
  id: IngredientId
  order: PositiveNumber
  name: TrimmedText50
}
