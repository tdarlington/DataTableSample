using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Model;

namespace Data
{
    public static class Contacts //: IContacts
    {

        private static ContactsData ReadContactDocument()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Data.data.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                ContactsData result = JsonConvert.DeserializeObject<ContactsData>(json);
                return result;
            }
        }

        public static ContactsData GetAllContacts()
        {
            ContactsData dbData = ReadContactDocument();

            return dbData;
        }

        //[Obsolete("GetContactsPaged is deprecated, collection functions are performed at presentation layer.")]
        public static IQueryable<Contact> GetContactsPaged(string criteria, string orderByColumn, string orderByDirection, int initialPage, int pageSize, out int recordCount, out int recordsFiltered)
        {
            ContactsData dbData = ReadContactDocument();
            recordCount = dbData.Data.Count();
            IEnumerable<Contact> contacts = dbData.Data.Select(d=>d);

            if (!string.IsNullOrEmpty(criteria))
            {
                contacts = contacts.Where(x => x.FirstName.ToUpper().Contains(filter.ToUpper()));
            }

            recordsFiltered = contacts.Count();

            contacts = contacts.OrderBy(x => x.FirstName)  //update to reflect direction/column
                .Skip(initialPage * pageSize)
                .Take(pageSize);

            return contacts.AsQueryable<Contact>();
        }
        private static IEnumerable<Contact> OrderResults<Contact>(IEnumerable<Contact> data, string orderByColumn, string orderByDirection)
        {
            if (String.IsNullOrEmpty(orderByColumn))
                return data;

            var prop = typeof(Contact).GetProperty(orderByColumn);
            return orderByDirection == "asc" ? data.OrderBy(x => prop.GetValue(x, null)) : data.OrderByDescending(x => prop.GetValue(x, null));
        }
    }
}
