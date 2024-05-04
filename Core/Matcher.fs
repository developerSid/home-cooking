namespace House.Imod.Cooking.Core

open System.Text.RegularExpressions

type Matcher = private Matcher of Regex

type MatchResult =
  {
    Groups: GroupCollection
    Captures: CaptureCollection
    Value: Text
  }

module Matcher =

  let create regex = Matcher(Regex regex)

  let createIgnoreCase regex =
    Matcher(Regex(regex, RegexOptions.IgnoreCase))

  let tryMatch (Matcher matcher) txt =
    let result = matcher.Match txt

    if result.Success then
      Some
        {
          Groups = result.Groups
          Captures = result.Captures
          Value = txt
        }
    else
      None

  let tryMatches (Matcher matcher) notMatchesError (txt: Text) =
    let result = matcher.Match txt

    if result.Success then
      Ok txt
    else
      Error(notMatchesError txt)

  let tryMatchDo (Matcher matcher) doIfMatches (txt: Text) =
    let result = matcher.Match txt

    if result.Success then
      Some(
        doIfMatches
          {
            Groups = result.Groups
            Captures = result.Captures
            Value = txt
          }
      )
    else
      None

  let matches (Matcher matcher) txt = (matcher.Match txt).Success