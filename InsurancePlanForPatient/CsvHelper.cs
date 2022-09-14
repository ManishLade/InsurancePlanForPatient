using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePlanForPatient
{
    internal class CsvHelperUtility
    {
        /// <summary>        
        /// /// Gets the patient list from input CSV.        
        /// </summary> 
        /// <param name="inputFilePath">The input file path.</param>        
        /// <returns></returns>       
        public static IEnumerable<T> GetDataFromInputCsv<T>(string inputFilePath)
        {
            if (File.Exists(inputFilePath))
            {
                using (var reader = new StreamReader(inputFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    try
                    {
                        var records = csv.GetRecords<T>();
                        var result = records.ToList();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"CSV file is not valid");
                        return null;
                    }
                }
            }
            Console.WriteLine($"CSV file does not exist on given path: {inputFilePath}");
            return null;
        }

        public static void WriteOutPutCsv(string outputFilePath, List<InsurancePlanForPatientData> plans)
        {
            using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<InsurancePlanForPatientData>();
                csv.NextRecord();

                csv.WriteRecords(plans);

                writer.Flush();
                Console.WriteLine("output csv successfully written");
            }
        }
    }
}
