namespace BashSoft.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using IO;
    using Static_data;

    public static class StudentRepository
    {
        public static bool isDataInitialized = false;
        // Dictionary<course, Dictionary<student, List<grades>>>
        private static Dictionary<string, Dictionary<string, List<int>>> studentsByCourse;

        public static void InitializeData(string fileName)
        {
            if (!isDataInitialized)
            {
                OutputWriter.WriteMessageOnNewLine("Reading data...");
                studentsByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
                ReadData(fileName);
            }
            else
            {
                OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataAlreadyInitialisedException);
            }
        }

        private static void ReadData(string fileName)
        {
            string path = SessionData.currentPath;
            if (Directory.Exists(path))
            {
                path += @"\" + fileName;
                string pattern = @"([A-Z][A-Za-z\#\+]*_[A-Z][a-z]{2}_201[4-6])\s+([A-Z][a-z]{0,3}[0-9]{2}_[0-9]{2,4})\s+(\d+)";
                Regex regex = new Regex(pattern);
                string[] allInputLines = File.ReadAllLines(path);
                for (int line = 0; line < allInputLines.Length; line++)
                {
                    if (!string.IsNullOrEmpty(allInputLines[line])
                        && regex.IsMatch(allInputLines[line]))
                    {
                        Match currentMatch = regex.Match(allInputLines[line]);
                        string courseName = currentMatch.Groups[1].Value;
                        string userName = currentMatch.Groups[2].Value;
                        int studentScoreOnTask;
                        bool hasParsedScore = int.TryParse(currentMatch.Groups[3].Value,
                            out studentScoreOnTask);
                        if (hasParsedScore && studentScoreOnTask >= 0 && studentScoreOnTask <= 100)
                        {
                            if (!studentsByCourse.ContainsKey(courseName))
                            {
                                studentsByCourse.Add(courseName,
                                    new Dictionary<string, List<int>>());
                            }
                            if (!studentsByCourse[courseName].ContainsKey(userName))
                            {
                                studentsByCourse[courseName].Add(userName, new List<int>());
                            }
                            studentsByCourse[courseName][userName].Add(studentScoreOnTask);
                        }
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
            }
            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private static bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (studentsByCourse.ContainsKey(courseName))
                {
                    return true;
                }
            }
            else
            {
                OutputWriter.DisplayException
                    (ExceptionMessages.DataNotInitializedExceptionMessage);
            }
            return false;
        }

        private static bool IsQueryForStudentPossible(string courseName, string studentName)
        {
            if (IsQueryForCoursePossible(courseName)
                && studentsByCourse[courseName].ContainsKey(studentName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException
                    (ExceptionMessages.InexistingStudentInDataBase);
            }
            return false;
        }

        public static void GetStudentScoresFromCourse(string courseName, string studentName)
        {
            if (IsQueryForStudentPossible(courseName, studentName))
            {
                OutputWriter.PrintStudent(new KeyValuePair<string, List<int>>
                    (studentName, studentsByCourse[courseName][studentName]));
            }
        }

        public static void GetStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine(string.Format("{0}:", courseName));
                foreach (var studentGradesEntry in studentsByCourse[courseName])
                {
                    OutputWriter.PrintStudent(studentGradesEntry);
                }
            }
        }

        public static void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }
            }
            RepositoryFilters.FilterAndTake(studentsByCourse[courseName], givenFilter, studentsToTake.Value);
        }

        public static void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = studentsByCourse[courseName].Count;
                }
            }
            RepositorySorters.OrderAndTake(studentsByCourse[courseName], comparison, studentsToTake.Value);
        }
    }
}
