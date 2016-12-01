// Referencing the script
#load "packages/FsLab/Themes/DefaultWhite.fsx"
#load "packages/FsLab/FsLab.fsx"

// Referencing the libraries
open Deedle
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle

let wb = WorldBankData.GetDataContext()
let ind = wb.Countries.India.Indicators
let data = series ind.``Gross enrolment ratio, tertiary, both sexes (%)``