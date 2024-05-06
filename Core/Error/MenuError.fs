namespace House.Imod.Cooking.Core.Error


type MenuError =
  | IngredientRequired
  | IngredientDescriptionTooLong of TooLong
