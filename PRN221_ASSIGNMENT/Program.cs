using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ScheduleManagementContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ScheduleManagement")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();


app.Run();