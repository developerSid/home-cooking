namespace House.Imod.Cooking.Core.Error

open System
open System.Text
open House.Imod.Cooking.Core

(* struggling with how to pass errors back to the client in a way that
   it can translate to something meaningful to the user. I really just
   want an enum, but the traditional string enumeration isn't possible
   in F#.
*)
type ErrorCode =
  | ProtobufEventNotSet
  | EventSequenceEmpty
  | UnableToDeleteMessageFromQueue
  | SortableIdUnableToBeParsed

[<RequireQualifiedAccess>]
module ErrorCode =
  let dtoify =
    DiscriminatedUnions.toText<ErrorCode>
    >> Text.toCharArray
    >> Array.fold
      (fun (acc: StringBuilder) current ->
        if (acc.Length > 0) then
          let previousChar = acc.Chars(acc.Length - 1)

          match (previousChar, current) with
          | p, c when Char.IsLower(p) && Char.IsUpper(c) -> acc.Append('.').Append(c)
          | _, c -> acc.Append(c)
        else
          acc.Append(current))
      (StringBuilder())
    >> string

  let parseDto = Text.remove '.' >> DiscriminatedUnions.fromText<ErrorCode>
