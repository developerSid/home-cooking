namespace House.Imod.Cooking.Core

open System
open FsToolkit.ErrorHandling

type Text = string
type Text2 = private Text2 of Text
type Text5 = private Text5 of Text
type Text7 = private Text7 of Text
type TrimmedText7 = private TrimmedText7 of Text7
type TrimmedText5 = private TrimmedText5 of Text5
type Text30 = private Text30 of Text
type TrimmedText30 = private TrimmedText30 of Text30
type Text50 = private Text50 of Text
type TrimmedText50 = private TrimmedText50 of Text50
type Text300 = private Text300 of Text
type TrimmedText300 = private TrimmedText300 of Text300
type Text1000 = private Text1000 of Text

module Text =
  let nullSafe text = if isNull text then None else Some text
  let trimmed (text: Text) = Ok(text.Trim())
  let trim (text: Text) = text.Trim()

  let lower (text: Text) =
    match nullSafe text with
    | None -> String.Empty
    | Some text -> text.ToLower()

  let ofNullOrBlank str =
    match Option.ofNull str with
    | None -> None
    | Some s ->
      if String.IsNullOrEmpty s || String.IsNullOrWhiteSpace s then
        None
      else
        Some s

  let bridge (text: Text) : string = text

  let bridgeMaybe (text: Text option) : string =
    match text with
    | None -> null
    | Some text -> text

[<RequireQualifiedAccess>]
module Text1000 =
  let construct nullOrEmptyError tooLongError str =
    ConstrainedType.constructString nullOrEmptyError tooLongError Text1000 1000 str

  let constructMaybe tooLongError str =
    str |> ConstrainedType.constructStringMaybe tooLongError Text1000 1000


type TrimmedText1000 = private TrimmedText1000 of Text1000

[<RequireQualifiedAccess>]
module Text2 =
  let create nullOrEmptyError tooLongError text =
    ConstrainedType.constructString nullOrEmptyError tooLongError Text2 2 text

[<RequireQualifiedAccess>]
module Text5 =
  let create nullOrEmptyError tooLongError str =
    str |> ConstrainedType.constructString nullOrEmptyError tooLongError Text5 5

[<RequireQualifiedAccess>]
module Text7 =
  let create nullOrEmptyError tooLongError str =
    str |> ConstrainedType.constructString nullOrEmptyError tooLongError Text7 7

[<RequireQualifiedAccess>]
module TrimmedText5 =
  let create nullOrEmptyError tooLongError (text: Text) =
    match Text5.create nullOrEmptyError tooLongError (text.Trim()) with
    | Ok value -> Ok(TrimmedText5 value)
    | Error constrainedTypeError -> Error constrainedTypeError

[<RequireQualifiedAccess>]
module Text30 =
  let construct nullOrEmptyError tooLongError str =
    str |> ConstrainedType.constructString nullOrEmptyError tooLongError Text30 30

[<RequireQualifiedAccess>]
module TrimmedText30 =
  let construct nullOrEmptyError tooLongError (text: Text option) =
    match text with
    | None -> Error nullOrEmptyError
    | Some text ->
      match text |> Text.trim |> Text30.construct nullOrEmptyError tooLongError with
      | Ok value -> Ok(TrimmedText30 value)
      | Error constrainedTypeError -> Error(constrainedTypeError)

[<RequireQualifiedAccess>]
module Text50 =
  let construct nullOrEmptyError tooLongError str =
    ConstrainedType.constructString nullOrEmptyError tooLongError Text50 50 str

  let constructMaybe tooLongError str =
    str |> ConstrainedType.constructStringMaybe tooLongError Text50 50

  let createLimited
    createStringLimitedWasNullOrEmpty
    createStringLimitedWasTooLong
    createStringLimitedWasTooShort
    minLength
    str
    =
    ConstrainedType.concstructStringLimited
      createStringLimitedWasNullOrEmpty
      createStringLimitedWasTooLong
      createStringLimitedWasTooShort
      Text50
      50
      minLength
      str

[<RequireQualifiedAccess>]
module TrimmedText50 =
  let construct nullOrEmptyError tooLongError (text: Text option) =
    match text with
    | None -> Error nullOrEmptyError
    | Some text ->
      match text |> Text.trim |> Text50.construct nullOrEmptyError tooLongError with
      | Ok value -> Ok(TrimmedText50 value)
      | Error constrainedTypeError -> Error(constrainedTypeError)

  let constructMaybe tooLongError (text: Text option) =
    match text with
    | None -> Ok None
    | Some text ->
      match text |> Text.trim |> Text50.constructMaybe tooLongError with
      | Ok value ->
        match value with
        | Some value -> Ok(Some(TrimmedText50 value))
        | None -> Ok None
      | Error constrainedTypeError -> Error(constrainedTypeError)

  let createLimited
    createStringLimitedWasNullOrEmpty
    createStringLimitedWasTooLong
    createStringLimitedWasTooShort
    minLength
    (text: Text)
    =
    match
      Text50.createLimited
        createStringLimitedWasNullOrEmpty
        createStringLimitedWasTooLong
        createStringLimitedWasTooShort
        minLength
        text
    with
    | Ok value -> Ok(TrimmedText50 value)
    | Error err -> Error(err)


  let unwrap (TrimmedText50(Text50 text)) = text

  let unwrapMaybe =
    function
    | Some(TrimmedText50(Text50 text)) -> text
    | None -> null

[<RequireQualifiedAccess>]
module Text300 =
  let construct nullOrEmptyError tooLongError str =
    ConstrainedType.constructString nullOrEmptyError tooLongError Text300 300 str

  let constructMaybe tooLongError str =
    str |> ConstrainedType.constructStringMaybe tooLongError Text300 300

[<RequireQualifiedAccess>]
module TrimmedText300 =
  let construct nullOrEmptyError tooLongError (text: Text option) =
    match text with
    | None -> Error nullOrEmptyError
    | Some text ->
      match text |> Text.trim |> Text300.construct nullOrEmptyError tooLongError with
      | Ok value -> Ok(TrimmedText300 value)
      | Error constrainedTypeError -> Error(constrainedTypeError)

  let constructMaybe tooLongError (text: Text option) =
    match text with
    | None -> Ok None
    | Some text ->
      match text |> Text.trim |> Text300.constructMaybe tooLongError with
      | Ok value ->
        match value with
        | Some value -> Ok(Some(TrimmedText300 value))
        | None -> Ok None
      | Error constrainedTypeError -> Error(constrainedTypeError)


  let unwrap (TrimmedText300(Text300 text)) = text

  let unwrapMaybe =
    function
    | None -> null
    | Some(TrimmedText300(Text300 text)) -> text

[<RequireQualifiedAccess>]
module TrimmedText1000 =
  let construct nullOrEmptyError tooLongError (text: Text option) =
    match text with
    | None -> Error nullOrEmptyError
    | Some text ->
      match text |> Text.trim |> Text1000.construct nullOrEmptyError tooLongError with
      | Ok value -> Ok(TrimmedText1000 value)
      | Error constrainedTypeError -> Error(constrainedTypeError)

  let constructMaybe tooLongError (text: Text option) =
    match text with
    | None -> Ok None
    | Some text ->
      match text |> Text.trim |> Text1000.constructMaybe tooLongError with
      | Ok value ->
        match value with
        | Some value -> Ok(Some(TrimmedText1000 value))
        | None -> Ok None
      | Error constrainedTypeError -> Error(constrainedTypeError)

  let unwrap (TrimmedText1000(Text1000 text)) = text

  let unwrapMaybe =
    function
    | Some(TrimmedText1000(Text1000 text)) -> text
    | None -> null
