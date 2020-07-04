using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender="All")
        {
            String username = Thread.CurrentPrincipal.Identity.Name;
            try {
                    using (sampleEntities entities = new sampleEntities())
                    {
                        switch (username.ToLower())
                        {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender == "male").ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender == "female").ToList());
                        default:
                            return Request.CreateResponse(HttpStatusCode.BadRequest);

                        }
                    }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (sampleEntities entities = new sampleEntities())
            {
                var entity=entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "not found");

            }
        }
        public HttpResponseMessage Post(Employee employee)
        {
            try
            {
                using (sampleEntities entities = new sampleEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + "/"+employee.ID.ToString());
                return message;
            }
            catch(Exception ex)
            {
               return  Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (sampleEntities entities = new sampleEntities())
                {

                    var entity = (entities.Employees.FirstOrDefault(e => e.ID == id));
                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, "deleted");
                        return message;
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.NotFound, "cannot be found");
                        return message;
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public HttpResponseMessage Put(int id,Employee employee)
        {
            try
            {
                using (sampleEntities entities=new sampleEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if(entity!=null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "not found");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
