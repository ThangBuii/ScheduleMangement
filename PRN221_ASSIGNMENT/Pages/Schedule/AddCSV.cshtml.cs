using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;
using System.Globalization;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class AddModel : PageModel
    {
        private ScheduleManagementContext _context;

        public AddModel(ScheduleManagementContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async void OnPostImport(IFormFile csvFile)
        {
            ScheduleService service = new ScheduleService(_context);
            DataService dataService = new DataService(_context);
            if (csvFile != null && csvFile.Length > 0)
            {

                if (Path.GetExtension(csvFile.FileName).ToLower() != ".csv")
                {
                    ViewData["Messages"] = new List<string> { "Uploaded file is not a CSV file." };
                    return;
                }
                // Specify the path where you want to save the uploaded XML file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", csvFile.FileName);

                // Save the uploaded XML file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await csvFile.CopyToAsync(stream);
                }

                using (var streamReader = new StreamReader(filePath))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {


                        var records = csvReader.GetRecords<CsvData>().ToList();
                        service.DeleteAllData();
                        List<string> messages = new List<string>();

                        foreach (var record in records)
                        {
                            // Check if the CSV record has all required fields
                            if (string.IsNullOrEmpty(record.Class) || string.IsNullOrEmpty(record.Subject) ||
                                string.IsNullOrEmpty(record.Room) || string.IsNullOrEmpty(record.Teacher) ||
                                string.IsNullOrEmpty(record.TimeSlot))
                            {
                                messages.Add("CSV file is missing one or more required fields.");
                                ViewData["Messages"] = messages;
                                return;
                            }

                            // Check if the CSV record has more fields than needed
                            if (record.GetType().GetProperties().Length > 5)
                            {
                                messages.Add("CSV file contains more fields than needed.");
                                ViewData["Messages"] = messages;
                                return;
                            }
                        }

                        for (int i = 0; i < records.Count(); i++)
                        {
                            string message = dataService.AddDataToDatabase(records[i]);
                            messages.Add($"Line {i + 1}: " + message);
                        }

                        ViewData["Messages"] = messages;
                    }
                }
            }
            else
            {
                ViewData["Messages"] = new List<string> { "No file uploaded." };
            }

        }

        public async void OnPostAdd(IFormFile csvFile)
        {
            DataService service = new DataService(_context);
            if (csvFile != null && csvFile.Length > 0)
            {

                if (Path.GetExtension(csvFile.FileName).ToLower() != ".csv")
                {
                    ViewData["Messages"] = new List<string> { "Uploaded file is not a CSV file." };
                    return;
                }
                // Specify the path where you want to save the uploaded XML file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", csvFile.FileName);

                // Save the uploaded XML file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await csvFile.CopyToAsync(stream);
                }

                using (var streamReader = new StreamReader(filePath))
                {
                    using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        

                        var records = csvReader.GetRecords<CsvData>().ToList();
                        List<string> messages = new List<string>();

                        foreach (var record in records)
                        {
                            // Check if the CSV record has all required fields
                            if (string.IsNullOrEmpty(record.Class) || string.IsNullOrEmpty(record.Subject) ||
                                string.IsNullOrEmpty(record.Room) || string.IsNullOrEmpty(record.Teacher) ||
                                string.IsNullOrEmpty(record.TimeSlot))
                            {
                                messages.Add("CSV file is missing one or more required fields.");
                                ViewData["Messages"] = messages;
                                return;
                            }

                            // Check if the CSV record has more fields than needed
                            if (record.GetType().GetProperties().Length > 5)
                            {
                                messages.Add("CSV file contains more fields than needed.");
                                ViewData["Messages"] = messages;
                                return;
                            }
                        }

                        for (int i = 0; i < records.Count(); i++)
                        {
                            string message = service.AddDataToDatabase(records[i]);
                            messages.Add($"Line {i+1}: " + message);
                        }

                        ViewData["Messages"] = messages;
                    }
                }
            }
            else
            {
                ViewData["Messages"] = new List<string> { "No file uploaded." };
            }

        }
    }
}
