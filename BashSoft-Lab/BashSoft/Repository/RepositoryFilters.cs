namespace BashSoft.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IO;
    using Static_data;
    public static class RepositoryFilters
    {
        public static void FilterAndTake(Dictionary<string, List<int>> wantedData,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                FilterAndTake(wantedData, x => x >= 5, studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(wantedData, x => x < 5 && x >= 3.5, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(wantedData, x => x < 3.5, studentsToTake);
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidStudentsFilter);
            }
        }
        private static void FilterAndTake(Dictionary<string, List<int>> wantedData,
           Predicate<double> givenFilter, int studentsToTake)
        {
            int printedCount = 0;
            foreach (var student_points in wantedData)
            {
                if (printedCount == studentsToTake)
                {
                    break;
                }
                double averageScore = student_points.Value.Average();
                double perpercentageOfFullfillment = averageScore / 100;
                double mark = perpercentageOfFullfillment * 4 + 2;
                if (givenFilter(mark))
                {
                    OutputWriter.PrintStudent(student_points);
                    printedCount++;
                }
            }
        }
    }
}