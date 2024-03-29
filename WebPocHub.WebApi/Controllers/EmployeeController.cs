using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPocHu.Dal;
using WebPocHub.Models;
using WebPocHub.WebApi.DTO;

namespace WebPocHub.WebApi.Controllers
{ 
    // Create a Controller Go to controller folder -> new -> Controller -> api -> empty -> normal 
    // ctor for shortcut add constructor 
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("PublicPolicy")]
    public class EmployeeController : ControllerBase
    {
        private readonly ICommonRepository<Employee> _employeeRepository;   
        private readonly IMapper _mapper;
        public EmployeeController(ICommonRepository<Employee> repository, IMapper mapper)
        {
            _employeeRepository = repository;
            _mapper = mapper;
        }
        [HttpGet]  // Get ALl API 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee, Hr")]
        public ActionResult<IEnumerable<EmployeeDto>> Get()
        { 
            var employees = _employeeRepository.GetAll();
            if (employees.Count <= 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees)); 
        }
        [HttpGet("{id:int}")] // Get by Id 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee, Hr")]
        public ActionResult<EmployeeDto> GetDetails( int id)
        {
            var employee = _employeeRepository.GetDetails(id);
            return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDto>(employee)); 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Employee, Hr")]
        public ActionResult Create(NewEmployeeDto employee)
        {
            var employeeModel = _mapper.Map<Employee>(employee);
            _employeeRepository.Insert(employeeModel);
            var result = _employeeRepository.SaveChanges(); 
            if(result > 0)
            {
                var employeeDetails = _mapper.Map<EmployeeDto>(employeeModel); 
                return CreatedAtAction("GetDetails", new { id = employeeDetails.EmployeeId }, employeeDetails);    
            }
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Employee,Hr")]
        public ActionResult Update(Employee employee)
        {
            var employeeModel = _mapper.Map<Employee>(employee);
            _employeeRepository.Update(employeeModel);   
            var result = _employeeRepository.SaveChanges(); 
            if(result > 0)
            {
                return NoContent(); 
            }
            return BadRequest(); 
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Hr")]
        public ActionResult<Employee> Delete(int id)
        {
            var employee = _employeeRepository.GetDetails(id); 
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                _employeeRepository.Delete(employee);
                _employeeRepository.SaveChanges(); 
                return NoContent();
            }
        }
    }
}
