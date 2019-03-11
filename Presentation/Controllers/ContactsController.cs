using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using Presentation.Models;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    public class ContactsController : ApiController
    {

        public IHttpActionResult Post(ContactsSearchRequest request)
        {

            var contactsList = Business.Contacts.GetAllContacts();
            var jsonContacts = JsonConvert.SerializeObject(contactsList);
            var responseData = JsonConvert.DeserializeObject<ContactsData>(jsonContacts);

            var response = Search(responseData.Data, request);

            return Ok(response);
        }

        protected static IList<Contact> PageResults<Contact>(IEnumerable<Contact> results, ContactsSearchRequest request)
        {
            var skip = request.Start;
            var pageSize = request.Length;
            var orderedResults = OrderResults(results, request);
            return pageSize > 0 ? orderedResults.Skip(skip).Take(pageSize).ToList() :
                  orderedResults.ToList();
        }

        private static IEnumerable<Contact> OrderResults<Contact>(IEnumerable<Contact> results, ContactsSearchRequest request)
        {
            if (request.Order == null) return results;
            var columnIndex = request.Order[0].Column;
            var sortDirection = request.Order[0].Dir;
            var columnName = request.Columns[columnIndex].Data;
            var prop = typeof(Contact).GetProperty(columnName);
            return sortDirection == "asc" ? results.OrderBy(x => prop.GetValue(x, null)) : results.OrderByDescending(x => prop.GetValue(x, null));
        }

        public static IEnumerable<Contact> FilterContacts(IEnumerable<Contact> data, string criteria)
        {
            if (String.IsNullOrEmpty(criteria))
            {
                return data;
            }
            criteria = criteria.ToLower();
            var results = data.Where(x => x.FirstName.ToLower().Contains(criteria)
                || x.LastName.ToLower().Contains(criteria)
                || x.CellNumber.ToLower().Contains(criteria)
                || x.Address.ToLower().Contains(criteria)
                );

            return results;
        }

        private static ContactsSearchResponse Search(ICollection<Contact> contacts, ContactsSearchRequest request)
        {
            var results = FilterContacts(contacts, request.Search.Value).ToList();
            var response = new ContactsSearchResponse
            {
                Data = PageResults(results, request),
                Draw = request.Draw,
                RecordsFiltered = results.Count,
                RecordsTotal = contacts.Count
            };
            return response;
        }
    }
}