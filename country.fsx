#load "packages/FsLab/FsLab.fsx"

open FSharp.Data
open XPlot.GoogleCharts

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


// Temprature of all the countries
let worldTemp =
    [ for c in wb.Countries ->
        let place = c.CapitalCity + "," + c.Name
        printfn "Getting temperature in: %s" place 
        c.Name, tempForCity place ]


// Creates a Geo Map with countries and their temperature
// (opens in a browser)
Chart.Geo(worldTemp)

// .Net array in F#
let colors = [| "#80E000";"#E0C000";"#E07B00";"#E02800" |] 
let values = [| -30;-10;+10;+30 |]
let axis = ColorAxis(values=values, colors=colors)

// Decorated Chart
worldTemp
|> Chart.Geo
|> Chart.WithOptions(Options(colorAxis=axis)) 
|> Chart.WithLabel "Temp"