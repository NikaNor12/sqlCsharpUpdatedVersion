using System;
using System.IO;
using Microsoft.Data.SqlClient;

namespace sqlCsharp
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            DataImporter importer = new DataImporter();
            importer.ImportDataFromTextFile();
        }
    }
}
