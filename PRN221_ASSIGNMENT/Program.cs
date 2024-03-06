using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ScheduleManagementContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ScheduleManagement")));

//tao ra app theo thong tin trong builder
var app = builder.Build();

//routing theo nguyen tac razor page


app.MapRazorPages();


app.Run();