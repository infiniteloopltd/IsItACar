using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace CarClassifierCore
{
    internal class CarDetection
    {


        public static bool IsItACar(string imagePath)
        {
            string virtualEnvPython = @"C:\research\python\pytorch\Vehicle-Detection\myenv\Scripts\python.exe";

            string commandOutput = ExecuteCommand(
                @"C:\research\python\pytorch\Vehicle-Detection",
                virtualEnvPython,
                $"detect.py --weights runs/train/exp12/weights/best.pt --source {imagePath}"
            );

            Console.WriteLine(commandOutput);

            return IsPositiveDetection(commandOutput);
        }

        static bool IsPositiveDetection(string output)
        {
            string pattern = @"\d+\s+(Car|Truck|Bus)";
            Regex regex = new Regex(pattern, RegexOptions.Multiline);
            return regex.IsMatch(output);
        }
        static string ExecuteCommand(string workingDirectory, string command, string arguments)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    var outputBuilder = new StringBuilder();
                    var errorBuilder = new StringBuilder();

                    // Attach event handlers to capture output asynchronously
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            outputBuilder.AppendLine(e.Data);
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                            errorBuilder.AppendLine(e.Data);
                    };

                    process.Start();

                    // Begin asynchronous reading of the streams
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // Combine standard output and error streams
                    string output = outputBuilder.ToString();
                    string error = errorBuilder.ToString();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        return $"Error:\n{error}\nOutput:\n{output}";
                    }

                    return output;
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

    }
}
