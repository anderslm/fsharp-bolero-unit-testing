# fsharp-bolero-unit-testing
This is a demonstration of hos to test the `update` function in a `Bolero` client.

To run this demo: `# dotnet run -p src/FSharp.Bolero.Server`

To run tests: `# dotnet test`

## How it works
The `update` and `init` function in `Bolero` is expected to return a model and a command of type `Cmd<'msg>`. 
The command is not testable and is, by definition, a side effect. So we have to invert the dependency to `Cmd<'msg>` and create an adapter function that goes from `SomethingTestable -> Cmd<'msg>`.

In this example I have created a new type called `Effect` and changed `update` and `init` to return `Model * Effect` instead of `Model * Cmd<'msg>`. 
I've also created a new function `effectAdapter` that takes an `Effect` and gives a `Cmd<'msg>`.
`effectAdapter` is used to adapt `init` and `update` from `Model * Effect` to `Model * Cmd<'msg>`. 

Now `init` and `update` is testable.
