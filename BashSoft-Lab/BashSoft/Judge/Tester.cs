namespace BashSoft.Judge
{
    using System;
    using System.IO;
    using IO;
    using Static_data;

    public static class Tester
    {
        public static void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            OutputWriter.WriteMessageOnNewLine("Reading files...");
            try
            {
                string mismatchPath = GetMismatchPath(expectedOutputPath);
                string[] actualOutputLines = File.ReadAllLines(userOutputPath);
                string[] expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                bool hasMismatch;
                string[] mismatches = GetLinesWithPossibleMismathces(actualOutputLines,
                    expectedOutputLines, out hasMismatch);
                PrintOutput(mismatches, hasMismatch, mismatchPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch (FileNotFoundException)
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }

        }

        private static string GetMismatchPath(string expectedOutputPath)
        {
            int indexOf = expectedOutputPath.LastIndexOf('\\');
            string directoryPath = expectedOutputPath.Substring(0, indexOf);
            string finalPath = directoryPath + @"\Mismatches.txt";
            return finalPath;
        }

        private static string[] GetLinesWithPossibleMismathces(
           string[] actualOutputLines, string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;

            OutputWriter.WriteMessageOnNewLine("Comparing files...");

            int minOutputLines = actualOutputLines.Length;
            if (actualOutputLines.Length != expectedOutputLines.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.DisplayException(
                    ExceptionMessages.ComparisonOfFilesWithDifferentSizes);
            }
            string[] mismatches = new string[minOutputLines];

            for (int index = 0; index < minOutputLines; index++)
            {
                string actualLine = actualOutputLines[index];
                string expectedLine = expectedOutputLines[index];
                if (!expectedLine.Equals(actualLine))
                {
                    output = string.Format("Mismatch at line {0} -- expected: \"{1}\", actual: \"{2}\"",
                        index, expectedLine, actualLine);
                    output += Environment.NewLine;
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }
                mismatches[index] = output;
            }
            return mismatches;
        }

        private static void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchPath)
        {
            if (hasMismatch)
            {
                foreach (string mismatch in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(mismatch);

                }
                try
                {
                    File.WriteAllLines(mismatchPath, mismatches);
                }
                catch (DirectoryNotFoundException)
                {
                    OutputWriter.DisplayException(
                        ExceptionMessages.InvalidPath);
                }
                return;
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
            }
        }

    }
}
