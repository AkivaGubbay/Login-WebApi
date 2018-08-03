using PlayGroundTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlayGroundTutorial.Controllers
{
    public class PeopleController : ApiController
    {
        public static List<Person> people = new List<Person>();
        public static bool wasCalledOnce = false; 
        public PeopleController()
        {
            if(wasCalledOnce == false)
            {
                people.Add(new Person { FirstName = "Andrew", LastName = "McGrath", Id = 1 });
                people.Add(new Person { FirstName = "Tom", LastName = "Bellchambers", Id = 2 });
                people.Add(new Person { FirstName = "Darcy", LastName = "Parish", Id = 3 });
                wasCalledOnce = true;
            }
            
        }

        // GET: api/People
        public List<Person> Get()
        {
            return people;
        }

        // GET: api/People/5
        public Person Get(int id)
        {
            return people.First(x => x.Id == id);
        }

        // POST: api/People
        public void Post([FromBody]Person value)
        {
            people.Add(value);
        }

        //// PUT: api/People/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/People/5
        public void Delete(int id)
        {
        }
    }
}
