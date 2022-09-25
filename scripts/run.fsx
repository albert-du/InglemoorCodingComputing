#!/usr/bin/dotnet fsi
#load "utils.fsx"
open Utils
open System
open System.IO
open System.IO.Compression
open System.Threading

while true do
    printfn "starting ..."

    cd ["bin"]

    let bin = Environment.CurrentDirectory

    let proc =
        try
            let file = 
                DirectoryInfo(".").EnumerateFiles()
                |> Seq.find (fun x -> x.Name = "InglemoorCodingComputing.dll")

            printfn $"EXECUTING: {file.FullName}"

            execP "dotnet" $" \"{file.FullName}\""
            |> Some
        with _ -> 
            None

    try
        cd []
        
        let mutable broken = false;
        while not broken && (match proc with None -> false | Some x -> not x.HasExited) do
            // check every 5 seconds
            Thread.Sleep 10000
            printfn "Checking for new version or crash"
            DirectoryInfo(".").EnumerateFiles()
            |> Seq.tryFind (fun x -> x.Name = "package.zip")
            |> function
            | Some zip -> 
                printfn "Updating"
                proc 
                |> Option.iter (fun proc -> proc.Kill())

                Directory.Delete(bin, true)
                cd ["bin"]

                ZipFile.ExtractToDirectory(zip.FullName, bin)

                File.Delete zip.FullName

                broken <- true;
            | _ -> 
                ()
    with _ ->
        ()
    printfn "process ended, waiting to restart"
    Thread.Sleep 5000
    printf "re"