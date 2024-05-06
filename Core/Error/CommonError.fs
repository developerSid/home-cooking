namespace House.Imod.Cooking.Core.Error

open System.Collections.Generic
open House.Imod.Cooking.Core

type ErrorMessage = Text
type MaxLength = Length
type MinLength = Length
type ProvidedLength = Length
type TooShort = Text * MinLength * ProvidedLength
type TooLong = Text * MaxLength * ProvidedLength

[<CLIMutable>]
type ErrorDtoContainer =
  { ErrorCode: ErrorCode
    Context: string option // if possible show the path to get to the property that caused the problem
    Parameters: string list }

[<RequireQualifiedAccess>]
module ErrorDtoContainer =
  let create errorCode context (parameters: IEnumerable<string> option) =
    match parameters with
    | None ->
      { ErrorCode = errorCode
        Context = context
        Parameters = List.Empty }
    | Some value ->
      { ErrorCode = errorCode
        Context = context
        Parameters = value |> List.ofSeq }

type ErrorDto =
  | NotFound of ErrorDtoContainer
  | BadRequest of ErrorDtoContainer
  | Internal of ErrorDtoContainer

module TooLong =
  let mapToErrorDto errorCode context (error: TooLong) =
    let provided, maxLength, providedLength = error
    let parameters = [ provided; string maxLength; string providedLength ]

    ErrorDto.BadRequest
      { ErrorCode = errorCode
        Context = context
        Parameters = parameters }

module TooShort =
  let mapToErrorDto errorCode context (error: TooShort) =
    let provided, minLength, providedLength = error
    let parameters = [ provided; string minLength; string providedLength ]

    ErrorDto.BadRequest
      { ErrorCode = errorCode
        Context = context
        Parameters = parameters }

[<RequireQualifiedAccess>]
module ErrorDto =
  let notFound errorCode context parameters =
    NotFound(ErrorDtoContainer.create errorCode context parameters)

  let internalError errorCode context parameters =
    Internal(ErrorDtoContainer.create errorCode context parameters)

  let badRequest errorCode context parameters =
    BadRequest(ErrorDtoContainer.create errorCode context parameters)

  let extractContainer =
    function
    | NotFound container
    | BadRequest container
    | Internal container -> container
