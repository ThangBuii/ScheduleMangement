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
            DataService service = new DataService(_context);
            if (csvFile != null && csvFile.Length > 0)
            {
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
                        for (int i=0;i<records.Count();i++)
                        {
                            
                            string message = service.AddDataToDatabase(records[i]);
                            messages.Add(message);
                        }

                        ViewData["Messages"] = messages;
                    }
                }
            }

        }

        public async void OnPostAdd(IFormFile csvFile)
        {
            DataService service = new DataService(_context);
            if (csvFile != null && csvFile.Length > 0)
            {
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
                        for (int i = 0; i < records.Count(); i++)
                        {
                            string message = service.AddDataToDatabase(records[i]);
                            messages.Add(message);
                        }

                        ViewData["Messages"] = messages;
                    }
                }
            }

        }
    }
}
