using Microsoft.ML.Data;

public class CarInfo
{
    [LoadColumn(0)]
    public string Make { get; set; }

    [LoadColumn(1)]
    public string Model { get; set; }

    [LoadColumn(2)]
    public float Year { get; set; }

    [LoadColumn(3)]
    public string EngineFuelType { get; set; }

    [LoadColumn(4)]
    public float Horsepower { get; set; }

    [LoadColumn(5)]
    public float Cylinders { get; set; }

    [LoadColumn(6)]
    public string Transmission { get; set; }

    [LoadColumn(7)]
    public string DrivenWheels { get; set; }

    [LoadColumn(8)]
    public float NumDoors { get; set; }

    [LoadColumn(9)]
    public string MarketCategory { get; set; }

    [LoadColumn(10)]
    public string VehicleSize { get; set; }

    [LoadColumn(11)]
    public string VehicleStyle { get; set; }

    [LoadColumn(12)]
    public float HighwayMPG { get; set; }

    [LoadColumn(13)]
    public float CityMPG { get; set; }

    [LoadColumn(14)]
    public float Popularity { get; set; }

    [LoadColumn(15)]
    public float MSRP { get; set; }
}