module Tests

open Xunit
open FsUnit.Xunit
open FsUnit.CustomMatchers
open FSharp.Bolero.Client.Main

let model, effect = init()

[<Fact>]
let ``Fetches data when initialized`` () =
    effect
    |> function
        | FetchDataEffect msg ->
            msg [] |> should be (ofCase<@DataFetched@>)
        | _ -> failwith "Expected an effect that fetches data."

[<Fact>]
let ``Can set the page`` () =
    let model, effect = update (SetPage SomePage) model
    
    model.Page
    |> should be (ofCase<@SomePage@>)
    effect
    |> should be (ofCase<@NoEffect@>)
    
[<Fact>]
let ``Fetches data when told to`` () =
    let _, effect = update FetchData model
    
    effect
    |> function
        | FetchDataEffect msg ->
            msg [] |> should be (ofCase<@DataFetched@>)
        | _ -> failwith "Expected an effect that fetches data."
        
[<Fact>]
let ``When data is fetched it is set on the model`` () =
    let data = [ { Data = "SomeData" } ]
    let model, _ = update (DataFetched data) model
    
    model.Data
    |> should equal data