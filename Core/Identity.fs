namespace House.Imod.Cooking.Core

open KsuidDotNet
open House.Imod.Protocol

type SortableIdentifier = SortableIdentifier of string

type EntityId = SortableIdentifier

type RowId = SortableIdentifier

[<RequireQualifiedAccess>]
module SortableIdentifier =
  // base62{27}/ksuid regex
  let private ksuidPattern = Matcher.create "^[0-9A-Za-z]{27}$"
  let START_ID_STRING = "000000000000000000000000000"
  let private emptyKsuid = SortableIdentifier(START_ID_STRING)
  let START_ID = emptyKsuid

  let newId () = SortableIdentifier(Ksuid.NewKsuid())

  [<CompiledName("Empty")>]
  let empty () = emptyKsuid

  let toProtobuf (SortableIdentifier id) =
    let protobufKsuid = ProtocolKsuid()

    protobufKsuid.Value <- id.ToString()

    protobufKsuid

  let fromProtobuf (id: ProtocolKsuid) = SortableIdentifier id.Value

  let toString (SortableIdentifier id) = id

  let fromString invalidError id =
    if Matcher.matches ksuidPattern id then
      Ok(SortableIdentifier id)
    else
      Error(invalidError id)