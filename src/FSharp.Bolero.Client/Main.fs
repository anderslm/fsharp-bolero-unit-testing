module FSharp.Bolero.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting.Client
open Bolero.Templating.Client

type Page =
    | [<EndPoint "/">] Home
    | SomePage

type SomeData = { Data : string }

type Model =
    { Page : Page
      Data : SomeData list }

type Message =
    | SetPage of Page
    | FetchData
    | DataFetched of SomeData list
    
type Effect =
    | FetchDataEffect of (SomeData list -> Message)
    | NoEffect

let init _ =
    { Page = Home
      Data = [] }
    , FetchDataEffect DataFetched

let update message model =
    match message with
    | SetPage p -> { model with Page = p }, NoEffect
    | FetchData -> model, FetchDataEffect DataFetched
    | DataFetched data -> { model with Data = data }, NoEffect

let router = Router.infer SetPage (fun model -> model.Page)

type Main = Template<"wwwroot/main.html">

let view model dispatch =
    Main()
        .Datas(forEach model.Data (fun d -> p [] [ text d.Data ]))
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        
        let effectAdapter effect =
            match effect with
            | NoEffect -> Cmd.none
            | FetchDataEffect msg ->
                Cmd.OfAsync.perform (fun () -> async {
                    //TODO: Fetch from actual I/O
                    return [{ Data = "Data 1" }
                            { Data = "Data 2" }]
                }) () msg
        
        let update message model =
            let model, effect = update message model
            model, effectAdapter effect
        
        let init a =
            let model, effect = init a
            model, effectAdapter effect
        
        Program.mkProgram init update view
        |> Program.withRouter router