using System;

namespace PA5Test4
{
    public class Transaction
    {
        public Guid transactionID{get; set;}
        public string customerEmail{get; set;} = "";
        public Guid movieID{get; set;}
        public string movieTitle{get; set;} = "";
        public string movieGenre{get; set;} = "";
        public DateOnly rentalDate{get; set;}
        public DateOnly returnDate{get; set;}
        public string transactionStanding{get;set;}="";
        public decimal movieRating{get; set;}
    }
}
