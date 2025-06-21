using AutoMapper;
using dotnet_core_MVC.Data;
using dotnet_core_MVC.DTO;
using dotnet_core_MVC.Hubs;
using dotnet_core_MVC.Models;
using dotnet_core_MVC.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;




namespace dotnet_core_MVC.Controllers
{
    [ApiController]
    [Route("Apinew/[controller]")]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context )
        {
            _context = context;           
        }
       
        [HttpGet("GetEmp")]
        public IActionResult Employeelist(int? id)
        {
            List<Employee> employees = new List<Employee>();
            if (id.HasValue)
            {
                var employee = _context.Employee
                                .Where(e => e.Id == id.Value)
                                .Select(e => new
                                {
                                    e.Id,
                                    e.EmpName,
                                    e.Email,
                                    e.Salary,
                                    CTC = e.Salary * 12,    // Annual Salary
                                    PF = e.Salary * 0.10m,  // 10% PF
                                    HRA = e.Salary * 0.20m, // 20% House Rent Allowance
                                    DA = e.Salary * 0.05m,  // 5% Dearness Allowance
                                    Gross = e.Salary + (e.Salary * 0.20m) + (e.Salary * 0.05m), // Salary + HRA + DA
                                    NetPay = (e.Salary + (e.Salary * 0.20m) + (e.Salary * 0.05m)) - (e.Salary * 0.10m) // Gross - PF
                                })
                                .ToList();
                return Ok(new { success = true, employees = employee });
            }
            else
            {
                employees = _context.Employee.ToList();
                return Ok(new { success = true, employees });
            }         
        }
        [HttpPost("empDelete")]
        public IActionResult DeleteUser(int id)
        {
            var user =_context.Employee.FirstOrDefault(e => e.Id == id);
            if (user != null)
            {
                _context.Employee.Remove(user);
                _context.SaveChanges();
            }
            return Ok(new { id = id });
        }
    }
    public class MessageDto
    {
        public string User { get; set; }
        public string Text { get; set; }
    }
}
