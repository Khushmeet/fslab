#load "packages/FsLab/FsLab.fsx"

open FSharp.Data

let wb = WorldBankData.GetDataContext()

let capital = wb.Countries.India.CapitalCity
let region = wb.Countries.India.Region
let indicators = wb.Countries.India.Indicators.``CO2 emissions (kt)``

// CO2 emissions for all the years till 2013, converting it to a sequence
indicators
|> List.ofSeq

// Type provider to access JSON
type Weather = JsonProvider<"http://api.openweathermap.org/data/2.5/weather?q=Chandigarh&appid={api_key}">

// Raw JSON
let weather = Weather.GetSample()
printfn "%s" weather.Name

// Function that takes city as param and return its temp
let tempForCity city =
    let url = "http://api.openweathermap.org/data/2.5/weather?appid={api_key}&q="
    let w = Weather.Load(url+city)
    w.Main.Temp

// Function call
tempForCity "Chandigarh"
