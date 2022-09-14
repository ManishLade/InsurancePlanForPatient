// See https://aka.ms/new-console-template for more information
using InsurancePlanForPatient;
using System.Collections.ObjectModel;

var path = Directory.GetCurrentDirectory();

var patients = CsvHelperUtility.GetDataFromInputCsv<Patients>(path + "\\PatientMeds.csv");
var insurancePlans = CsvHelperUtility.GetDataFromInputCsv<InsurancePlans>(path + "\\InsurancePlans.csv");

var insurancePlanForPatient = new List<InsurancePlanForPatientData>();
var patientGroups = patients.GroupBy(x => x.PatientId).ToList();
foreach (var patientGroup in patientGroups)
{
    var patientId = patientGroup.Key;
    long totalCost = 0;
    Dictionary<string, long> planDetails = new Dictionary<string, long>();

    foreach (var patient in patientGroup)
    {
        var numberOfDays = (DateTime.Now - patient.StartDate).Days;

        var totalDaysOfSupply = (long)numberOfDays / patient.DaysSupply;

        totalCost += totalDaysOfSupply * patient.Cost;
    }
    foreach (var plan in insurancePlans)
    {
        long totalPayable = 0;
        var minimumeExpense = plan.Premium + plan.MaximumOutOfPocketExpenses;
        if (plan.Deductible < totalCost)
        {
            var payableAmount = (totalCost * plan.Coinsurance) / 100;
            totalPayable = minimumeExpense + payableAmount;

            planDetails.Add(plan.InsurancePlanName, totalPayable);
        }
    }
    var efficientPlan = planDetails.OrderBy(x => x.Value).FirstOrDefault();
    insurancePlanForPatient.Add(new InsurancePlanForPatientData
    {
        PatientId = patientId,
        Cost = efficientPlan.Value,
        RecommendedInsurancePlan = efficientPlan.Key
    });
    Console.WriteLine(patientId + " has total cost as " + totalCost);
}
var outputPath = Path.Combine(path, "EfficientInsurancePlans.csv");
CsvHelperUtility.WriteOutPutCsv(outputPath, insurancePlanForPatient);