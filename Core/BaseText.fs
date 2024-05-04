namespace House.Imod.Cooking.Core

open System
open SimpleBase

type Base58Text = private Base58Text of Text
type Base64Text = private Base64Text of Text

[<RequireQualifiedAccess>]
module Base58Text =
  let private encodingFlavor = Base58.Flickr
  let ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"
  let encode (ba: byte array) = encodingFlavor.Encode(ba) |> Base58Text
  let extract (Base58Text text) = text
  let decode = extract >> encodingFlavor.Decode


[<RequireQualifiedAccess>]
module Base64Text =
  let encode = Convert.ToBase64String >> Base64Text
  let extract (Base64Text text) = text
  let decode = extract >> Convert.FromBase64String
  let encodeExtract = encode >> extract

  let embed invalidError (text: Text) =
    try
      Convert.FromBase64String text |> ignore

      Ok(Base64Text text)
    with :? FormatException ->
      Error(invalidError text)