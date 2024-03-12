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

        [BindProperty]
        public string CSVfile { get; set; }

        public AddModel(ScheduleManagementContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            DataService service = new DataService(_context);
            using (var streamReader = new StreamReader(CSVfile))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<CsvData>().ToList();

                    foreach(CsvData record in records)
                    {
                        if (service.ValidateData(record))
                        {
                            service.AddDataToDatabase(record);
                        }
                    }
                }
            }

        }
    }
}
