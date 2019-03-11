using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Linq;

namespace Business
{
    public static class Contacts //: IContacts
    {
        public static ContactsData GetAllContacts()
        {
            return Data.Contacts.GetAllContacts();
        }

        public static IQueryable<Contact> GetContactsPaged(string filter, string orderByColumn, string orderByDirection, int initialPage, int pageSize, out int recordCount, out int recordsFiltered)
        {
            return Data.Contacts.GetContactsPaged(filter, orderByColumn, orderByDirection, initialPage, pageSize, out recordCount, out recordsFiltered);
        }

    }
}
