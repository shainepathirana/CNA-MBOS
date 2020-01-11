using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Customer
    {
  

        public string FName { get; set; }
        public string LName { get; set; }
        public string Dob { get; set; }
        public string Nic { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
        public String password { get; set; }

        public Customer(string fName, string lName, string dob, string nic, string address, string gender, string email, int phoneNo, string password)
        {
            FName = fName;
            LName = lName;
            Dob = dob;
            Nic = nic;
            Address = address;
            Gender = gender;
            Email = email;
            PhoneNo = phoneNo;
            this.password = password;
        }
    }

}
