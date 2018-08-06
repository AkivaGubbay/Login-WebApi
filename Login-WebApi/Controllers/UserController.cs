using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using LoginWebApi.Contracts;
using LoginWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;




namespace LoginWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public ActionResult<UserRegisterResponse> Post([FromBody] UserRegisterRequest request)
        {
            try
            {
                // Validate user input
                if (ModelState.IsValid)
                {

                    Validation validation = new Validation(request.UserName, request.Email, request.UserPassword);

                    if(validation.methodOfResponse == GlobalParmeters.RESPONSE_SERVERERROR)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    else if(validation.methodOfResponse == GlobalParmeters.RESPONSE_USERERROR)
                    {
                       return BadRequest(validation.errorMsg);
                    }
                    else
                    {
                        UserRegisterResponse response = DAL.Register(request.UserName, request.FirstName, request.LastName, request.Email, request.UserPassword);
                        return CreatedAtRoute("Get", new { id = response.UserName }, response);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ee)
            {
                //should be a server side error response (5XXX)
                return BadRequest(ee);
            }
        }
        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/userr/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
