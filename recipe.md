# Setting Up Feliz.Shadcn with SAFE

Integrating Feliz.Shadcn into your Elmish Land application is straightforward. The following example demonstrates how to set up a basic Elmish Land app that incorporates Shadcn components.

We wil be using the Feliz wrapper built by [reaptor](https://github.com/reaptor): [https://github.com/reaptor/feliz.shadcn](https://github.com/reaptor/feliz.shadcn)

## Step 1: Setup tailwind

> Note: When you use the SAFE template you will already have Tailwind installed by default. You can skip this step. 

Check out the following recipe here to install tailwind: [Add Tailwind](https://safe-stack.github.io/docs/recipes/ui/add-tailwind/)

## Step 2: Move `package.json` & `package-lock.json` to /src/Client

## Step 3: Configure import alias in tsconfig:

Create a file named tsconfig.json in `/src/Client` and add the following:

{
    "files": [],
    "compilerOptions": {
        "baseUrl": ".",
        "paths": {
            "@/*": [
                "./*"
            ]
        }
    }
}

## Step 3: Install shadcn/ui

> Note: ensure your node version is > 20.5.0

```
npx shadcn@latest init
```

You will be asked a few questions to configure components.json

## Step 4:  Add Feliz.Shadcn

Inside the `/src/Client` directory run:

```
dotnet add package Feliz.Shadcn
```

## Step 5: Start adding any shadcn component

Specify first which components you want to use:

```
npx shadcn@latest add button 
```

Then use it in Feliz:

```fsharp
open Feliz.Shadcn


let view = 
    Shadcn.button [
        prop.text "Button" ]

```

