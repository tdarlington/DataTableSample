using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace Presentation.Models
{
    public class ContactsSearchResponse
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public IList<Contact> Data { get; set; }

    }
}