using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

public class RegressionConsoleProgressReporter : IProgress<RunDetail<RegressionMetrics>>
{
    public void Report(RunDetail<RegressionMetrics> value)
    {
        // We may or may not have validation metrics. We won't if the run was aborted.
        if (value.ValidationMetrics != null)
        {
            double mae = value.ValidationMetrics.MeanAbsoluteError;

            Console.WriteLine($"{value.TrainerName} ran in {value.RuntimeInSeconds:0.00} seconds with mean absolute error of {mae}");
        }
        else
        {
            Console.WriteLine($"{value.TrainerName} ran in {value.RuntimeInSeconds:0.00} seconds but did not complete. Time likely expired.");
        }
    }
}
