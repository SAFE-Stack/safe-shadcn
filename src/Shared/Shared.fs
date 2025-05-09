namespace Shared

(*
    In a typical SAFE Stack application, this file would contain types shared between
    client and server. For this example, we're focusing only on the client-side with
    Fable, Feliz, and ShadCN UI components.
*)

open System

type Persona = {
    Name: string;
    Avatar: string;
    Description: string
}

type FakerUser = {
    Firstname: string
    Lastname: string
    Username: string
    Email: string
    Image: string
}

type FakerUserResponse = {
    Status: string
    Total: int
    Data: FakerUser list
}
