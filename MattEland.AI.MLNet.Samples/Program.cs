using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

// Create a Machine Learning Context
MLContext context = new MLContext();

// Load our CSV file into data
IDataView data = context.Data.LoadFromTextFile<CarInfo>(
    path:@"data\car_data.csv", 
    separatorChar:',', 
    hasHeader: true, 
    allowQuoting:true);

// Get a Debug-Friendly View of things
var debugPreview = data.Preview();


// Configure our experiment
RegressionExperimentSettings settings = new RegressionExperimentSettings()
{
    OptimizingMetric = RegressionMetric.MeanAbsoluteError,
    MaxExperimentTimeInSeconds = 1,
};

// Create our Experiment (doesn't run it or train it)
RegressionExperiment experiment = context.Auto().CreateRegressionExperiment(settings);


// Track the Progress
RegressionConsoleProgressReporter progressReporter = new();

// Run the experiment and wait synchronously for it to complete
ExperimentResult<RegressionMetrics> result = 
    experiment.Execute(trainData: data,
                       labelColumnName: nameof(CarInfo.MSRP), 
                       progressHandler: progressReporter);

// Grab common variables from the result
IEnumerable<RunDetail<RegressionMetrics>> runs = result.RunDetails;
RunDetail<RegressionMetrics> bestRun = result.BestRun;


// Evaluation metrics are always stored in ValidationMetrics but differ by the algorithm used
RegressionMetrics metrics = bestRun.ValidationMetrics;

// R Squared measures how close the fit is ranging from -1.00 to 1.00 with 1.00 being perfect.
Console.WriteLine($"RSquared: {metrics.RSquared}");

// MAE = Average Absolute Error (difference between predicted and actual). 0 is ideal.
Console.WriteLine($"Mean Absolute Error: {metrics.MeanAbsoluteError}");

// MSE = MAE but squared error rates. Exaggerates large differences. 0 is ideal.
Console.WriteLine($"Mean Squared Error: {metrics.MeanSquaredError}");

// RMSE = Root Mean Squared Error. The square root of MSE. 0 is ideal.
Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");



// Create a Prediction Pipeline
PredictionEngine<CarInfo, CarPrediction> predictor 
    = context.Model.CreatePredictionEngine<CarInfo, CarPrediction>(
        transformer: bestRun.Model, 
        inputSchema: data.Schema);

// Make some predictions!
CarInfo fakeCar = new()
{
    CityMPG = 12, HighwayMPG = 40, Cylinders = 4, DrivenWheels = "rear wheel drive",
    EngineFuelType = "regular unleaded", Horsepower = 160, Make = "Audi", MarketCategory = "Compact", Model = "2022"
};
CarPrediction prediction = predictor.Predict(fakeCar);

// Write the model out to disk
ITransformer model = bestRun.Model;
DataViewSchema schema = data.Schema;

context.Model.Save(model, schema, "MyModel.zip");

// Load the model from disk
ITransformer loadedModel = context.Model.Load("MyModel.zip", out DataViewSchema _);


Console.WriteLine("All done!");