using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Account
    {

        public int AccountNo { get; set; }
        public double Balance { get; set; }
        public double BankCharges { get; set; }
        public double InterestRate { get; set; }
        public Customer Cutomer { get; set; }
        public String State { get; set; }

        public Account(int accountNo, double balance, double bankCharges, double interestRate, Customer cutomer, string state)
        {
            AccountNo = accountNo;
            Balance = balance;
            BankCharges = bankCharges;
            InterestRate = interestRate;
            Cutomer = cutomer;
            State = state;
        }
    }
}
