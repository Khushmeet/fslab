#load "packages/FsLab/FsLab.fsx"
#r "System.Xml.Linq.dll"

open FSharp.Data
open Deedle
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle

type WorldData = XmlProvider<"http://api.worldbank.org/countries/indicators/NY.GDP.PCAP.CD?date=2010:2010">

let indUrl = "http://api.worldbank.org/countries/indicators/"
let getData year indicator = 
    let query =
       [("per_page","1000");
        ("date",sprintf "%d:%d" year year)]
    let data = Http.RequestString(indUrl + indicator, query) 
    let xml = WorldData.Parse(data)
    let orNaN value =
        defaultArg (Option.map float value) nan 
    series [ for d in xml.Datas -> d.Country.Value, orNaN d.Value ]


let wb = WorldBankData.GetDataContext()
let data = wb.Countries.World.Indicators
let code = data.``CO2 emissions (kt)``.IndicatorCode
let co2000 = getData 2000 code
let co2010 = getData 2010 code

Chart.Geo(co2000)

// Percent Change
let change = (co2010 - co2000) / co2000 * 100.0

Chart.Geo(change)

let codes =
    ["CO2", data.``CO2 emissions (metric tons per capita)``
     "Life", data.``Life expectancy at birth, total (years)``
     "Growth", data.``GDP per capita growth (annual %)``
     "Pop", data.``Population growth (annual %)``
     "GDP", data.``GDP per capita (current US$)`` ]

let world =
    frame [for name, ind in codes -> name, getData 2010 ind.IndicatorCode]