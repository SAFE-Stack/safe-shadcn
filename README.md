# SAFE Template with ShadCN UI Components

This template can be used to generate a full-stack web application using the [SAFE Stack](https://safe-stack.github.io/) enhanced with [ShadCN UI](https://ui.shadcn.com/) components through the Feliz.Shadcn library. It was created using the dotnet [SAFE Template](https://safe-stack.github.io/docs/template-overview/) with additional integrations. If you want to learn more about the template why not start with the [quick start](https://safe-stack.github.io/docs/quickstart/) guide?

## ShadCN UI Integration

This template integrates ShadCN UI components with F# through the Feliz.Shadcn library, allowing you to:

- Use beautifully designed, accessible UI components
- Maintain type-safety with F# bindings
- Customize components through TailwindCSS
- Seamlessly work with the Elmish architecture

The components are located in `src/Client/components/ui/` and can be used in your F# code through the Feliz.Shadcn bindings. This integration provides a modern, responsive UI while maintaining the functional programming paradigm of F#.

### Example Usage with Feliz

```fsharp
// Import the required namespaces
open Feliz
open Feliz.Shadcn

// Using ShadCN components in F#
Shadcn.button [
    button.size.lg
    prop.className "mb-10"
    prop.children [ Html.text "Generate User Persona" ]
    prop.onClick (fun _ -> dispatch GeneratePersona)
]

// Using ShadCN card with other components
Shadcn.card [
    prop.className "w-full max-w-sm shadow-lg"
    prop.children [
        Shadcn.cardHeader [
            Shadcn.avatar [
                Shadcn.avatarImage [ prop.src "image.jpg" ]
                Shadcn.avatarFallback [ prop.text "User" ]
            ]
        ]
        Shadcn.cardContent [ ... ]
    ]
]
```

> Note: This example focuses exclusively on the client-side implementation. The server and shared components are included as part of the SAFE stack structure but contain minimal code.

## Install pre-requisites

You'll need to install the following pre-requisites in order to build SAFE applications

* [.NET SDK](https://www.microsoft.com/net/download) 8.0 or higher
* [Node 18](https://nodejs.org/en/download/) or higher
* [NPM 9](https://www.npmjs.com/package/npm) or higher

## Starting the application

To concurrently run the server and the client components in watch mode use the following command:

```bash
dotnet run
```

Then open `http://localhost:8080` in your browser.

The build project in root directory contains a couple of different build targets. You can specify them after `--` (target name is case-insensitive).

<!-- Tests have been removed from this example to focus on ShadCN and Feliz integration -->

Finally, there are `Bundle` and `Azure` targets that you can use to package your app and deploy to Azure, respectively:

```bash
dotnet run -- Bundle
dotnet run -- Azure
```

## Documentation

If you want to know more about the SAFE Stack and its components, visit the official [SAFE documentation](https://safe-stack.github.io/docs/).

You will find more documentation about the technologies used in this template at the following places:

* [Fable](https://fable.io/docs/) - F# to JavaScript compiler
* [Elmish](https://elmish.github.io/elmish/) - MVU pattern for F# applications
* [Feliz](https://zaid-ajaj.github.io/Feliz/) - React bindings for F#
* [TailwindCSS](https://tailwindcss.com/docs) - Utility-first CSS framework
* [ShadCN UI](https://ui.shadcn.com/) - Component library built with Radix UI and Tailwind
* [Saturn](https://saturnframework.org/) - Web development framework for F#
