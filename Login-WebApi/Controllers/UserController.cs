using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage Post([FromBody] UserRegisterRequest request)
        {
            // Validate user input
            if (ModelState.IsValid)
            {
                // register request
                DAL.Register(request.UserName, request.FirstName, request.LastName, request.Email, request.UserPassword);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                HttpResponseMessage badRequestMsg = new HttpResponseMessage();
                //HttpResponseMessage badRequestMsg =  new HttpResponseMessage(HttpStatusCode.BadRequest);
                //return this.Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
