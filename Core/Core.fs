namespace House.Imod.Cooking.Core

open System
open System.Collections.Generic
open System.Reflection
open System.Security.Cryptography
open Microsoft.FSharp.Reflection

type Unknown = exn // alias exn (exception) to unknown as a stand-in for places we can't model at the moment
type Length = int
type Number = int64
type PositiveNumber = uint32
type Date = DateOnly
type PositionMargin = double
type NonZeroPositiveNumber = private NonZeroPositiveNumber of Number

[<AutoOpen>]
module Core =
  // https://stackoverflow.com/a/11696947/104021
  let inline isNull (x: ^T when ^T: not struct) = obj.ReferenceEquals(x, null)

  let inline maybe (field: string option) = // TODO change naming to optional instead of maybe
    match field with
    | None -> None
    | Some f ->
      match String.IsNullOrEmpty(f) || String.IsNullOrWhiteSpace(f) with
      | true -> None
      | false -> Some f

  let inline maybeObject<'O> (ob: 'O option) = // TODO change naming to optionalObject instead of maybeObject
    match ob with
    | None -> None
    | Some o ->
      match obj.ReferenceEquals(o, null) with
      | true -> None
      | false -> Some o

  let inline swap swapTo _ = swapTo

[<RequireQualifiedAccess>]
module Date =
  let construct error dateIn =
    try
      Ok(DateOnly.Parse dateIn)
    with :? FormatException ->
      Error(error dateIn)

  let constructFromDateTime (dateIn: DateTime) = Ok(DateOnly.FromDateTime(dateIn))

  let maybeConstructFromDateTimeNullable (dateTimeIn: System.Nullable<DateTime>) =
    match Option.ofNullable dateTimeIn with
    | None -> None
    | Some d -> Some(DateOnly.FromDateTime(d))

[<RequireQualifiedAccess>]
module ConstrainedType =
  let constructStringMaybe tooLongError ctor maxLen str =
    if String.IsNullOrWhiteSpace(str) then
      Ok None
    elif str |> String.length > maxLen then
      Error(tooLongError (str, maxLen, (String.length str)))
    else
      Ok(Some(ctor str))

  let constructString nullOrEmptyError tooLongError ctor maxLen str =
    match constructStringMaybe tooLongError ctor maxLen str with
    | Error error -> Error error
    | Ok str ->
      match str with
      | None -> Error(nullOrEmptyError)
      | Some str -> Ok(str)

  let concstructStringLimited
    createStringLimitedWasNullOrEmpty
    createStringLimitedWasTooLong
    createStringLimitedWasTooShort
    ctor
    maxLen
    minLen
    str
    =
    if String.IsNullOrWhiteSpace(str) || String.IsNullOrEmpty(str) then
      Error(createStringLimitedWasNullOrEmpty)
    elif str |> String.length > maxLen then
      Error(createStringLimitedWasTooLong maxLen)
    elif str |> String.length < minLen then
      Error(createStringLimitedWasTooShort minLen)
    else
      Ok(ctor str)

  let constructNonEmpty createStringWasNullOrEmpty text =
    if String.IsNullOrWhiteSpace(text) then
      Error createStringWasNullOrEmpty
    else
      Ok text // TODO write positive test

[<RequireQualifiedAccess>]
module NonZeroPositiveNumber =
  let create mustBeGreaterThanZero emptyNumberError wasNotAllDigits numberTooBig text =
    try
      let parsedNum = Int64.Parse(text)

      if parsedNum > 0 then
        Ok(NonZeroPositiveNumber parsedNum)
      else
        Error(mustBeGreaterThanZero text)
    with
    | :? ArgumentNullException -> Error(emptyNumberError)
    | :? FormatException -> Error(wasNotAllDigits text)
    | :? OverflowException -> Error(numberTooBig text)

  let value (NonZeroPositiveNumber num) = num

[<RequireQualifiedAccess>]
module DiscriminatedUnions =
  let toText<'a> (x: 'a) =
    match
      FSharpValue.GetUnionFields(x, typeof<'a>, BindingFlags.NonPublic ||| BindingFlags.Default ||| BindingFlags.Public)
    with
    | case, _ -> case.Name

  let fromText<'a> (s: string) =
    match
      FSharpType.GetUnionCases(typeof<'a>, BindingFlags.NonPublic ||| BindingFlags.Default ||| BindingFlags.Public)
      |> Array.filter (fun case -> case.Name = s)
    with
    | [| case |] ->
      Some(
        FSharpValue.MakeUnion(case, [||], BindingFlags.NonPublic ||| BindingFlags.Default ||| BindingFlags.Public)
        :?> 'a
      )
    | _ -> None

[<RequireQualifiedAccess>]
module List =
  let removeFirstElement list =
    match list with
    | [] -> None, []
    | first :: rest -> (Some first), rest

[<RequireQualifiedAccess>]
module RandomNumberGenerator =
  let generateRandomSalt saltSize =
    let salt = Array.zeroCreate<byte> saltSize
    let rng = RandomNumberGenerator.Create()

    rng.GetBytes(salt)

    salt

[<RequireQualifiedAccess>]
module Dictionary =
  let isEmpty (d: IDictionary<'K, 'V>) =
    match d with
    | null -> true
    | _ -> d.Count = 0

[<RequireQualifiedAccess>]
module Seq =
  [<CompiledName("Peek")>]
  let peek peeker source =
    peeker source

    source
