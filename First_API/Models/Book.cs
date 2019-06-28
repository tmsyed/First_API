using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace First_API.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string BookName { get; set; }
        public string ISBN { get; set; }

        //public Book(int ID, string BookName, string ISBN)
        //{
        //    this.ID = ID;
        //    this.BookName = BookName;
        //    this.ISBN = ISBN;
        //}

        public override string ToString()
        {
            string result = "Book ID: " + this.ID + " Book Name: " + this.BookName + " Book ISBN: " + this.ISBN;
            return result;
        }
    }
}
