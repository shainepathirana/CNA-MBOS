using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
namespace Client
{
    class Program
    {
        const int PORT_NO = 6000;
        const string SERVER_IP = "127.0.0.1";
        static void Main(string[] args)
        {
            Console.Title = "Client";

            Home();

        }

        public static void Home()
        {
            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();

            int opt=0;
            while (opt != 3)
            {
                Console.WriteLine("\n\n\n");
                Console.WriteLine("                             *****************************************************************");
                Console.WriteLine("                                                     MBOS Application                        ");
                Console.WriteLine("                             *****************************************************************");
                Console.WriteLine("                                           (1)View Account                           ");
                Console.WriteLine("                                           (2)Register                        ");
                Console.WriteLine("                                           (3)Exit                        ");
                Console.Write("\n                       Please enter the choice  - ");

                opt = Convert.ToInt32(Console.ReadLine());
                switch (opt)
                {
                    case 1:
                        String searchId = ViewAccount();
                        //---Send customer data---
                        byte[] bytesToSend1 = ASCIIEncoding.ASCII.GetBytes(searchId);
                        nwStream.Write(bytesToSend1, 0, bytesToSend1.Length);

                        //---read back the text---
                        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                        //---convert the data received into a string---
                        string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);


                        string[] accountDetails = dataReceived.Split('$');

                        if (accountDetails[0] == "Invalid")
                        {
                            String msg = accountDetails[1];
                            Console.WriteLine("\n\n                                 " + msg);

                        }
                        else
                        {
                            AccountMenu(accountDetails); 
                        }

                        break;

                    case 2:
                        string customerData = Register();

                        //---Send customer data---
                        byte[] bytesToSend2 = ASCIIEncoding.ASCII.GetBytes(customerData);
                        nwStream.Write(bytesToSend2, 0, bytesToSend2.Length);

                        //---read back the text---
                        byte[] bytesToRead1 = new byte[client.ReceiveBufferSize];
                        int bytesRead1 = nwStream.Read(bytesToRead1, 0, client.ReceiveBufferSize);
                        Console.WriteLine("\n\n                  " + Encoding.ASCII.GetString(bytesToRead1, 0, bytesRead1));



                        break;

                    case 3:
                        Console.WriteLine("The Program will Exit now");

                        break;

                    default:
                        Console.WriteLine("Invalid option!!! \n Enter valid option ");
                        opt = Convert.ToInt32(Console.ReadLine());

                        break;


                }
            }

        }

        public static string Register()
        {
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                         REGISTER                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                      Please enter following details : \n");

            Console.Write("                                            First Name - ");
            String fName = Console.ReadLine();
            bool b1 = String.IsNullOrWhiteSpace(fName);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            First Name - ");
                fName = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(fName);
                

            }


            Console.Write("                                            Last Name -  ");
            String lName = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(lName);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Last Name -  ");
                lName = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(lName);
            }



            Console.Write("                                            Date of Birth (yyyy/mm/dd) -  ");
            String dob =Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(dob);

            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Date of Birth (yyyy/mm/dd) -  ");
                dob =Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(dob);

            }



            Console.Write("                                            NIC -  ");
            String nic = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(nic);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            NIC -  ");
                nic = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(nic);

            }




            Console.Write("                                            Address -  ");
            String address = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(address);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Address -  ");
                address = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(address);
            }



            Console.Write("                                            Gender(Male/Female) -  ");
            string gender = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(gender);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Gender(Female/Male) -  ");
                gender = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(gender); ;
            }



            Console.Write("                                            Email -  ");
            String email = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(email);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Email -  ");
                email = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(email);
            }




            Console.Write("                                            Phone Number -  ");
            int phoneNo;
            try
            {
                phoneNo = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("\n                               Can not leave this field blank or enter letters, Enter again.\n");
                Console.Write("                                            Phone Number -  ");
                phoneNo = Convert.ToInt32(Console.ReadLine());
            }


            Console.Write("                                            Password -  ");
            String password = Console.ReadLine();
            b1 = String.IsNullOrWhiteSpace(password);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Password -  ");
                password = Console.ReadLine();
                b1 = String.IsNullOrWhiteSpace(password);
            }


            string customer = "Register" + "$" + fName + "$" + lName + "$" + dob + "$" + nic + "$" + address + "$" + gender + "$" + email + "$" + phoneNo+"$"+password;

            return customer;


        }

        public static string ViewAccount()
        {
            int searchId;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                       VIEW ACCOUNT                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.Write("                                            Enter Account Number - ");
            try
            {
                searchId = Convert.ToInt32(Console.ReadLine());
            }

            catch (System.FormatException)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Enter Account Number -  ");
                searchId = Convert.ToInt32(Console.ReadLine());
            }

            Console.Write("                                            Enter Password - ");
            String password = Console.ReadLine();
            bool b1 = String.IsNullOrWhiteSpace(password);
            while (b1 == true)
            {
                Console.WriteLine("\n                               Can not leave this field blank, Enter again.\n");
                Console.Write("                                            Enter Password -  ");
                password = Console.ReadLine();
                break;
            }



            string accId = "View Account" + "$" + searchId+"$"+password;

            return accId;


        }

        public static void AccountMenu(string[] accountDetails)
        {
            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();
            int accountNo = Convert.ToInt32(accountDetails[0]);
           
            int opt = 0;
            while (opt != 10)
            {
                Console.WriteLine("\n\n\n");
                Console.WriteLine("                             *****************************************************************");
                Console.WriteLine("                                                     MBOS Application                        ");
                Console.WriteLine("                             *****************************************************************");
                Console.WriteLine("                                           (1)Deposit Money                           ");
                Console.WriteLine("                                           (2)Withdraw Money                        ");
                Console.WriteLine("                                           (3)Check Balance                        ");
                Console.WriteLine("                                           (4)Transfer Funds                        ");
                Console.WriteLine("                                           (5)View Account Details                        ");
                Console.WriteLine("                                           (6)Update Account Details                        ");
                Console.WriteLine("                                           (7)Check Interest for Current Balance                        ");
                Console.WriteLine("                                           (8)View Bank Charges                        ");
                Console.WriteLine("                                           (9)UnRegister                        ");
                Console.WriteLine("                                           (10)Log Out                        ");

                Console.Write("\n                       Please enter the choice  - ");
             
                opt = Convert.ToInt32(Console.ReadLine());
                switch (opt)
                {
                    case 1:
                        {
                            String depositDetails = DepositMoney(accountNo);

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(depositDetails);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                            Console.WriteLine("\n\n                  " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));

                        }
                        break;

                    case 2:
                        {
                            String withdrawDetails =WithdrawMoney(accountNo);

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(withdrawDetails);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);


                            string[] withdraw = dataReceived.Split('$');

                            if (withdraw[0] == "Fail")
                            {
                                String msg =withdraw[1];
                                Console.WriteLine("\n\n                  " + msg);

                            }
                            else
                            {
                                Console.WriteLine("\n\n                  " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                            }

                        }
                        break;

                    case 3:
                        {
                            Console.WriteLine("\n\n\n");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("                                                       VIEW BALANCE                        ");
                            Console.WriteLine("                             *****************************************************************");


                            String balanceDetails ="Balance"+"$"+accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(balanceDetails);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                            Console.WriteLine("\n\n                               " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                            
                        }
                        break;

                    case 4:
                        {
                            String transferDetails = TransferMoney(accountNo);

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(transferDetails);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);


                            string[] transfer = dataReceived.Split('$');

                            if (transfer[0] == "Fail")
                            {
                                String msg =transfer[1];
                                Console.WriteLine("\n\n                  " + msg);

                            }
                            else
                            {
                                Console.WriteLine("\n\n                  " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                            }

                        }
                        break;

                    case 5:
                        {
                            string viewDetails = "View Account Details" + "$" + accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(viewDetails);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);


                            string[] userDetails = dataReceived.Split('$');

                            Console.WriteLine("\n\n\n");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("                                                       VIEW ACCOUNT DETAILS                        ");
                            Console.WriteLine("                             *****************************************************************");
                            Console.Write("\n                                            First Name - " + userDetails[0]);
                            Console.Write("\n                                            Last Name - " + userDetails[1]);
                            Console.Write("\n                                            Date of Birth - " + userDetails[2].ToString());
                            Console.Write("\n                                            NIC Number - " + userDetails[3]);
                            Console.Write("\n                                            Address - " + userDetails[4]);
                            Console.Write("\n                                            Gender - " + userDetails[5]);
                            Console.Write("\n                                            Email - " + userDetails[6]);
                            Console.Write("\n                                            Phone Number - " + userDetails[7]);
                            Console.WriteLine("\n                             *****************************************************************");
                            Console.WriteLine("                             *****************************************************************");


                        }
                        break;


                    case 6:
                        {
                            string updateData = AccountUpdate(accountNo);

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(updateData);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                            Console.WriteLine("\n\n                  " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));


                        }
                        break;

                    case 7:
                        {

                            string viewInterest = "View Interest" + "$" + accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(viewInterest);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

                             Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW INTEREST                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             "+dataReceived);
                                Console.WriteLine("\n\n                             *****************************************************************");
                                Console.WriteLine("                             *****************************************************************");

                        }
                        break;

                    case 8:
                        {
                            string viewCharges = "View Charges" + "$" + accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(viewCharges);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

                            Console.WriteLine("\n\n\n");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("                                                       VIEW BANK CHARGES                        ");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("\n\n                             " + dataReceived);
                            Console.WriteLine("\n\n                             *****************************************************************");
                            Console.WriteLine("                             *****************************************************************");
                        }
                        break;

                    case 9:
                        {
                            string unRegister = "UnRegister" + "$" + accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(unRegister);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            //---read back the text---
                            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

                            //---convert the data received into a string---
                            string dataReceived = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                            string[] msg = dataReceived.Split('$');

                            Console.WriteLine("\n\n\n");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("                                                       UNREGISTER                        ");
                            Console.WriteLine("                             *****************************************************************");
                            Console.WriteLine("\n\n                             " +msg[0]);
                            Console.WriteLine("\n\n                             *****************************************************************");
                            Console.WriteLine("                             *****************************************************************");

                            opt = Convert.ToInt32(msg[1]);
                        }
                        break;

                    case 10:
                        {
                            string logOut = "Logout" + "$" + accountNo;

                            //---Send customer data---
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(logOut);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                          
                        }
                        break;

                }



            }
          


        }

        public static string DepositMoney(int accountNo)
        {
            double amount;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                       DEPOSIT MONEY                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.Write("                                            Enter Deposit Amount - ");
            amount = Convert.ToDouble(Console.ReadLine());

            string deposit = "Deposit" + "$" +accountNo+"$"+amount ;

            return deposit;
        }
         
        public static string WithdrawMoney(int accountNo)
        {
            double amount;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                       WITHDRAW MONEY                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.Write("                                            Enter Withdrawal Amount - ");
            amount = Convert.ToDouble(Console.ReadLine());

            string withdraw = "Withdraw" + "$" + accountNo + "$" + amount;

            return withdraw;
        }

        public static string TransferMoney(int senderAccountNo)
        {
            double amount;
            int recieverAccountNo;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                       TRANSFER MONEY                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.Write("                                            Enter Transfer Amount - ");
            amount = Convert.ToDouble(Console.ReadLine());
            Console.Write("                                            Enter desired Reciever's Account Number - ");
            recieverAccountNo = Convert.ToInt32(Console.ReadLine());
            string transfer = "Transfer" + "$" + senderAccountNo + "$" + amount + "$" + recieverAccountNo;

            return transfer;
        }

        public static string AccountUpdate(int accountNo)
        {
            Console.WriteLine("\n\n\n");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                                         UPDATE ACCOUNT                        ");
            Console.WriteLine("                             *****************************************************************");
            Console.WriteLine("                                      Please enter following details : \n");


            Console.Write("                                            New Address -  ");
            String address = Console.ReadLine();
           

            Console.Write("                                            New Email -  ");
            String email = Console.ReadLine();
           


            Console.Write("                                            New Phone Number -  ");
            int phoneNo;
            try
            {
                phoneNo = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("\n                               Can not leave this field blank or enter letters, Enter again.\n");
                Console.Write("                                            Phone Number -  ");
                phoneNo = Convert.ToInt32(Console.ReadLine());
            }



            string customer = "Update" + "$" +accountNo + "$"+ address + "$" + email + "$" + phoneNo;

            return customer;


        }



    }
}

//Random comment added