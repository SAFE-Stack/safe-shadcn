module Index

open Elmish
open SAFE
open Shared
open Fable.SimpleHttp
open Thoth.Json
open Fable.Core

type Model = { Personas: RemoteData<Persona list> }

type Msg =
    | GeneratePersonas of count: int
    | PersonasGenerated of Persona list
    | FetchFailed of string

let fakerUserDecoder: Decoder<FakerUserResponse> =
    Decode.object (fun get -> {
        Status = get.Required.Field "status" Decode.string
        Total = get.Required.Field "total" Decode.int
        Data =
            get.Required.Field
                "data"
                (Decode.list (
                    Decode.object (fun get -> {
                        Firstname = get.Required.Field "firstname" Decode.string
                        Lastname = get.Required.Field "lastname" Decode.string
                        Username = get.Required.Field "username" Decode.string
                        Email = get.Required.Field "email" Decode.string
                        Image = get.Required.Field "image" Decode.string
                    })
                ))
    })

let fetchFakerUsers count = async {
    try
        // Fetch users from FakerAPI
        let! userResponse =
            Http.request $"https://fakerapi.it/api/v1/users?_quantity={count}"
            |> Http.method GET
            |> Http.send

        // Process user response
        match userResponse.statusCode with
        | 200 ->
            match Decode.fromString fakerUserDecoder userResponse.responseText with
            | Ok userResponse ->
                let userList = userResponse.Data

                // Generate a random seed for each user to get different images
                let random = System.Random()

                let personas =
                    userList
                    |> List.map (fun user ->
                        // Generate a unique timestamp + random number to force browser to reload the image
                        let timestamp = System.DateTime.UtcNow.Ticks
                        let randomSeed = random.Next(1, 1000)

                        // Use Picsum Photos with a random seed and cache-busting query parameter
                        let imageUrl =
                            $"https://picsum.photos/seed/{user.Username}{randomSeed}/400/400?t={timestamp}"

                        {
                            Name = $"{user.Firstname} {user.Lastname} (@{user.Username})"
                            Avatar = imageUrl
                            Description = $"Email: {user.Email}"
                        })

                return PersonasGenerated personas
            | Error err -> return FetchFailed $"Failed to decode user response: {err}"
        | statusCode -> return FetchFailed $"User API error: {statusCode}"
    with ex ->
        return FetchFailed $"Request failed: {ex.Message}"
}

let init () =
    let initialModel = { Personas = NotStarted }
    let initialCmd = GeneratePersonas 6 |> Cmd.ofMsg

    initialModel, initialCmd

let update msg model =
    match msg with
    | GeneratePersonas count ->
        // Always start fresh when generating multiple personas
        { model with Personas = Loading None }, Cmd.OfAsync.perform fetchFakerUsers count (fun result -> result)
    | PersonasGenerated personas ->
        // Always replace with the new personas, don't append
        {
            model with
                Personas = Loaded personas
        },
        Cmd.none
    | FetchFailed errorMsg ->
        // If we have existing personas, keep them and just log the error
        match model.Personas with
        | Loaded _ -> model, Cmd.none
        | _ -> { model with Personas = NotStarted }, Cmd.none

open Feliz
open Feliz.Shadcn

[<ReactComponent>]
let CardComponent (title: string) (description: string) (avatarSrc: string) =
    Shadcn.card [
        prop.key avatarSrc
        prop.className "w-full shadow-lg"
        prop.children [
            Shadcn.cardHeader [
                prop.className "flex flex-row items-center gap-4"
                prop.children [
                    Shadcn.avatar [
                        prop.children [
                            Shadcn.avatarImage [
                                prop.key $"{title}"
                                prop.src avatarSrc
                                prop.alt "User avatar"
                                prop.style [
                                    style.width 40
                                    style.height 40
                                    style.objectFit.cover
                                    style.borderRadius 20
                                ]
                            ]
                            Shadcn.avatarFallback [
                                prop.children [
                                    Html.text (
                                        if String.length title > 0 then
                                            title.[0].ToString()
                                        else
                                            "?"
                                    )
                                ]
                            ]
                        ]
                    ]
                    Html.div [ Shadcn.cardTitle [ prop.children [ Html.text title ] ] ]
                ]
            ]
            Shadcn.cardContent [
                prop.className "pt-2"
                prop.children [
                    Html.img [
                        prop.className "w-full h-32 object-cover rounded-md mb-2"
                        prop.src avatarSrc
                        prop.alt "Profile image"
                    ]
                    Shadcn.badge [
                        badge.variant.secondary
                        prop.className
                            "bg-gray-100 text-sm text-muted-foreground w-full text-center justify-center mt-4"
                        prop.text "Click to learn more about this user."
                    ]
                ]
            ]
        ]
    ]

// Main view
let view model dispatch =
    Html.div [
        prop.className "py-10 text-center mt-8"
        prop.children [
            // Heading
            Html.h1 [ prop.className "text-4xl font-bold mb-6"; prop.text "Fake User Generator" ]

            // Button
            Shadcn.button [
                button.size.lg
                prop.className "mb-10"
                prop.children [ Html.text "Generate User Persona" ]
                prop.onClick (fun _ -> dispatch (GeneratePersonas 6))
            ]

            // Grid of cards
            Html.div [
                prop.className "grid grid-cols-1 lg:grid-cols-3 gap-6 mx-4"
                prop.children [
                    match model.Personas with
                    | NotStarted ->
                        Html.div [
                            prop.className "col-span-3 text-center"
                            prop.children [ Html.text "Click a button above to generate user personas" ]
                        ]
                    | Loading _ ->
                        Html.div [
                            prop.className "col-span-3 text-center"
                            prop.children [ Html.text "Loading personas..." ]
                        ]
                    | Loaded [] ->
                        Html.div [
                            prop.className "col-span-3 text-center"
                            prop.children [ Html.text "No personas found. Try again." ]
                        ]
                    | Loaded personas ->
                        // For multiple personas, just render them normally
                        for persona in personas do
                            CardComponent persona.Name persona.Description persona.Avatar
                ]
            ]
        ]
    ]
