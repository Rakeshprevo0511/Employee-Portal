using dotnet_core_MVC.Data;
using dotnet_core_MVC.DTO;
using dotnet_core_MVC.KafkaServces;
using dotnet_core_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_core_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
        //[HttpPost("SendKafkaMessage")]
        //public async Task<IActionResult> SendKafkaMessage([FromBody] KafkaMessageModel model)
        //{
        //    await _kafkaProducer.SendMessageAsync(model.Message);
        //    return Ok(new { success = true, message = "Kafka message sent." });
        //}
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return Ok();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(user);
        }
        public IActionResult Create_emp(int? id)
        {
            Employee employee = new Employee();
            if (id.HasValue)
            {
                employee = _context.Employee.Find(id);
                if (employee == null)
                {
                    return NotFound(); // Return 404 if employee not found
                }

            }
            return Ok(employee);
        }
        [HttpPost]
        public IActionResult ADD_EMP(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.Id > 0) // If Id exists, update the existing record
                {
                    var existingEmployee = _context.Employee.Find(employee.Id);
                    if (existingEmployee != null)
                    {
                        existingEmployee.EmpName = employee.EmpName;
                        existingEmployee.Position = employee.Position;
                        existingEmployee.Location = employee.Location;
                        existingEmployee.Age = employee.Age;
                        existingEmployee.Salary = employee.Salary;

                        _context.Employee.Update(existingEmployee);
                    }
                }
                else
                {
                    _context.Employee.Add(employee);
                }
                _context.SaveChanges();
                return RedirectToAction("Employee");
            }
            return Ok(employee);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Test()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [Authorize]
        public IActionResult Employee(int? id)
        {
            List<Employee> employees;
            if (id.HasValue)
            {
                employees = _context.Employee.Where(e => e.Id == 1).ToList();
                ViewBag.DisableViewButton = true;
            }
            else
            {
                ViewBag.DisableViewButton = false;
                employees = _context.Employee.ToList();
            }

            return Ok(employees);
        }
        [Authorize]
        [HttpGet("employees")]
        public IActionResult Employeelist(int? id)
        {
            if (id.HasValue)
            {
                var employee = _context.Employee
        .Where(e => e.Id == id.Value)
        .Select(e => new
        {
            e.Id,
            e.EmpName,
            e.Position,
            e.Location,
            e.Age,
            e.Salary,
            e.Email,
            e.PhoneNumber,
            e.Username,
            // Include related course data
            Courses = _context.EmployeeCourses
                .Where(ec => ec.EmployeeId == e.Id)
                .Select(ec => new
                {
                    ec.CourseId,
                    ec.Status,
                    ec.CompletionDate,
                    CourseName = _context.Courses
                        .Where(c => c.CourseId == ec.CourseId)
                        .Select(c => c.CourseName)
                        .FirstOrDefault()
                }).ToList()
        })
        .FirstOrDefault();
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found." });
                }

                return Ok(new { success = true, employee });
            }
            else
            {
                var employees = _context.Employee.ToList();
                if (employees == null || employees.Count == 0)
                {
                    return NotFound(new { message = "No employees found." });
                }

                return Ok(new { success = true, employees });
            }
        }
        [Authorize]
        [HttpPost("SaveEmp")]
        public async Task<IActionResult> SaveEmp([FromBody] Employee empData)
        {
            var existingEmp = await _context.Employee.FindAsync(empData.Id);

            if (existingEmp == null)
            {
                // Insert new employee
                _context.Employee.Add(empData);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Employee added successfully." });
            }
            else
            {
                // Update existing employee
                existingEmp.EmpName = empData.EmpName;
                existingEmp.Position = empData.Position;
                existingEmp.Location = empData.Location;
                existingEmp.Age = empData.Age;
                existingEmp.Salary = empData.Salary;
                existingEmp.Email = empData.Email;
                existingEmp.PhoneNumber = empData.PhoneNumber;
                existingEmp.Username = empData.Username;
                existingEmp.Password = empData.Password;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Employee updated successfully." });
            }
        }

        [HttpGet("GetCourse_List")]
        public async Task<IActionResult> Getcourse_list(int? id)
        {
            var courselist = await _context.Courses.ToListAsync();
            return Ok(new { success = true, data = courselist });
        }

        [HttpPost("Save_empCourse")]
        public async Task<IActionResult> SaveEmpCourse([FromBody] EmployeeCourse empCourse)
        {
            var existingEmpCourse = await _context.EmployeeCourses
                .FirstOrDefaultAsync(ec => ec.EmployeeId == empCourse.EmployeeId && ec.CourseId == empCourse.CourseId);

            if (existingEmpCourse == null)
            {
                // Insert new employee-course record
                _context.EmployeeCourses.Add(empCourse);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Course enrollment added successfully." });
            }
            else
            {
                // Update existing employee-course record
                existingEmpCourse.CompletionDate = empCourse.CompletionDate;
                existingEmpCourse.Status = empCourse.Status;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Course enrollment updated successfully." });
            }
        }
    }

}
