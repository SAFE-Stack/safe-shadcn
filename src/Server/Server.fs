module Server

(*
    In a typical SAFE Stack application, this file would contain server-side code.
    For this example, we're focusing only on the client-side with Fable, Feliz,
    and ShadCN UI components.
*)

open Saturn

let app = application {
    memory_cache
    use_static "public"
    use_gzip
}

[<EntryPoint>]
let main _ =
    run app
    0