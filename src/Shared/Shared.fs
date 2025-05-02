namespace Shared

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
