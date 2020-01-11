using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
      
        const int PORT_NO = 6000;
        const string SERVER_IP = "127.0.0.1";
       
        static void Main(string[] args)
        {
            Console.Title = "Server";
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening...");
            listener.Start();

            bool run = true;
            int counter = 0;
            while (run)
            {
                counter++;

                //Connecting to the client
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine(" >> Client  " + counter + " connected \n");

                Console.WriteLine("The local End point is  :" + listener.LocalEndpoint);

                ClientHandler handler = new ClientHandler();

                //Start a thread for each client
                Thread clientThread = new Thread(() => handler.StartClient(client));
                clientThread.Start();


                



            }
        }
        public class ClientHandler
        {
          
            public static List<Account> accountList = new List<Account>();
            public static int count = 001;
            private readonly object locker = new object();

            public void StartClient(TcpClient client)
            {
                bool run = true;
               

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();

                do
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    //---read incoming stream---
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    //---convert the data received into a string---
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);


                    string[] msg = dataReceived.Split('$');

                    switch (msg[0])
                    {

                        case "Register"://--Register new Customer-------/
                            {
                                lock (locker)
                                {


                                    Console.WriteLine("\n\n\n");
                                    Console.WriteLine("                             *****************************************************************");
                                    Console.WriteLine("                                                         REGISTER                        ");
                                    Console.WriteLine("                             *****************************************************************");
                                    Console.WriteLine("\n\n                             Received : " + dataReceived);

                                    int phoneNo = Convert.ToInt32(msg[8]);

                                    Customer s = new Customer(msg[1], msg[2], msg[3], msg[4], msg[5], msg[6], msg[7], phoneNo, msg[9]);


                                    //customerList.Add(s);

                                    Account a = new Account(count, 500, 0, 0.08, s, "Active");

                                    accountList.Add(a);

                                    string sendMsg = "Customer successfully saved in the server. Account Number is " + accountList[count - 1].AccountNo + "\n\n\n\n";


                                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                    //---send the text---
                                    Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                                    count++;
                                }
                            }
                            break;

                        case "View Account":     //--View Account------//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW ACCOUNT                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                string accountDetails;
                                int searchId = Convert.ToInt32(msg[1]);
                                var id = accountList.Where(b => b.AccountNo == searchId).FirstOrDefault();

                                if (id == null)
                                {
                                    accountDetails = "Invalid" + "$ " + "There is no Account registered under ID number " + searchId;
                                }

                                else if (id.State == "Deactivated")
                                {
                                    accountDetails = "Invalid" + "$ " + "Your Account has been Deactivated due to unavoidable reasons. Please contact our nearest branch.";
                                }

                                else {
                                    string password = msg[2];

                                    bool psw;
                                    
                                        if (id.Cutomer.password == password)
                                        {
                                            psw = true;
                                        }
                                        else
                                        {
                                            psw = false;
                                        }
                                    


                                 if (id != null && psw == false)
                                    {
                                        accountDetails = "Invalid" + "$ " + "Invalid Credentials.Check Password ";
                                    }

                                    else
                                    {
                                        accountDetails = Convert.ToString(id.AccountNo);
                                        id.State = "Logged In";
                                        Console.WriteLine(searchId + " is Logged in");

                                    }
                                }
                                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(accountDetails);

                                    //---send the text---
                                    Console.WriteLine("\n\n                             Sending : " + accountDetails);
                                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                                
                            }
                            break;

                        case "Deposit":      //--Deposit Money To Account---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       DEPOSIT MONEY                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);


                                int accountNo = Convert.ToInt32(msg[1]);
                                double amount = Convert.ToDouble(msg[2]);
                                var result = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();
                                result.Balance += amount;


                                string sendMsg = "Successfully deposited "+amount+" To Account Number "+accountNo+" Your current balance is "+result.Balance;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;

                        case "Withdraw":      //--Withdraw Money from Account---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       WITHDRAW MONEY                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                string sendMsg;
                                int accountNo = Convert.ToInt32(msg[1]);
                                double amount = Convert.ToDouble(msg[2]);
                                var result = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();

                                if (result.Balance >amount)   //--Check if there is enough money in the account to make the withdraw--//
                                {

                                    result.Balance -= amount;



                                    sendMsg = "Successfully withdrawn " + amount + " from Account Number " + accountNo + " Your current balance is " + result.Balance;

                                }

                                else
                                {
                                    sendMsg = "Fail" + "$" + "You do not have enough balance to make this withdrawal.";
                                }


                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;

                        case "Balance":    //--Check Balance of the account---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW BALANCE                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var result = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();

                                string sendMsg = "Your Current Balance is " + result.Balance;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;


                        case "Transfer":      //--Transfer Money from Account---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       TRANSFER MONEY                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                string sendMsg;

                                int senderAccountNo = Convert.ToInt32(msg[1]);
                                double transferAmount = Convert.ToDouble(msg[2]);
                                int recieverAccountNo = Convert.ToInt32(msg[3]);
                                var result = accountList.Where(b => b.AccountNo == senderAccountNo).FirstOrDefault();
                                var search= accountList.Where(b => b.AccountNo == recieverAccountNo).FirstOrDefault(); //--Check whether the recieving account exists in the system


                                if (search == null)
                                {
                                    sendMsg = "Fail" + "$" + "Receiver's Account does not exist in the system.Enter valid Account Number";
                                }

                                else if (search.State == "Deactivated")
                                {
                                    sendMsg = "Fail" + "$" + "Receiver's Account is currently disabled cannot complete the transfer.";
                                }

                                else if (result.Balance > transferAmount)   //--Check if there is enough money in the account to make the transfer--//
                                {

                                    result.Balance -= transferAmount;
                                    result.Balance -= 10;             //--Deducting bank charge for the transfer--//
                                    result.BankCharges += 10;

                                    var transfer = accountList.Where(b => b.AccountNo == recieverAccountNo).FirstOrDefault();
                                    transfer.Balance += transferAmount;
                                    sendMsg = "Successfully Transferred " + transferAmount + " from Account Number " + senderAccountNo + " to Account Number " + recieverAccountNo;

                                }

                                else
                                {
                                    sendMsg ="Fail"+"$"+ "You do not have enough balance to make this transfer. Please deposit money to make the transfer.";
                                }

                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;

                        case "View Account Details": //--View Customer Details--//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW ACCOUNT DETAILS                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var account = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();

                                string sendMsg = account.Cutomer.FName + "$" + account.Cutomer.LName + "$" + account.Cutomer.Dob + "$" + account.Cutomer.Nic + "$" + account.Cutomer.Address + "$" + account.Cutomer.Gender + "$" + account.Cutomer.Email + "$" + account.Cutomer.PhoneNo;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            }
                            break;


                        case "Update":    //--Update customer Details---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       UPDATE ACCOUNT                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo =Convert.ToInt32 (msg[1]);
                                string address = msg[2];
                                string email = msg[3];
                                int phoneNo = Convert.ToInt32(msg[4]);

                              
                                 //--Updating customer details in accountList---//

                                foreach(Account acc in accountList)
                                {
                                    if (acc.AccountNo == accountNo)
                                    {
                                      
                                        acc.Cutomer.Address = address;
                                        acc.Cutomer.Email = email;
                                        acc.Cutomer.PhoneNo = phoneNo;


                                    }
                                }


                               

                                string sendMsg = "Successfully Updated details of Account " + accountNo;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            }
                            break;

                        case "View Interest":   //--View interest for current balance--//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW INTEREST                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var account = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();
                                double interest = account.Balance * account.InterestRate;   //--Calculating interest for current balance--//

                                string sendMsg = "Monthly Interest rate is " + account.InterestRate + " According to current balance your interest is " + interest;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;

                        case "View Charges":   //--View interest for current balance--//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       VIEW BANK CHARGES                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var account = accountList.Where(b => b.AccountNo == accountNo).FirstOrDefault();
                                double charges = account.BankCharges;   //--Calculating interest for current balance--//

                                string sendMsg = "Bank charge per transfer is Rs.10. Currently bank has deducted Rs." + charges + " for your transactions.";
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                            }
                            break;

                        case "UnRegister":   //--Delete Customer and Account---//
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       UNREGISTER                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var accountToRemove = accountList.SingleOrDefault(b => b.AccountNo == accountNo);  //--Search account using account no---//
                                accountList.Remove(accountToRemove);


                                string sendMsg = "Successfully Deleted account "+accountNo+" and customer details from the system."+"$"+10;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(sendMsg);

                                //---send the text---
                                Console.WriteLine("\n\n                             Sending : " + sendMsg);
                                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                            }
                            break;

                        case "Logout":
                            {
                                Console.WriteLine("\n\n\n");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("                                                       LOG OUT                        ");
                                Console.WriteLine("                             *****************************************************************");
                                Console.WriteLine("\n\n                             Received : " + dataReceived);

                                int accountNo = Convert.ToInt32(msg[1]);
                                var stateUpdate = accountList.SingleOrDefault(b => b.AccountNo == accountNo);  //--Searching relevant Account--//
                                stateUpdate.State = "Active";  //--Update Account state to Active---//

                                

                                Console.Write("\n\n                             Do you want to Activate or Deactivate an Account (y/n)? ");
                                string opt = Console.ReadLine();

                                if (opt =="y")
                                {
                                    Console.Write("\n\n                                            Enter desired Account number : ");
                                    int accNo;
                                    try
                                    {
                                         accNo = Convert.ToInt32(Console.ReadLine());
                                    }

                                    catch(System.FormatException)
                                    {
                                        Console.WriteLine("\n                                   Can not leave this field blank, Enter again.\n");
                                        Console.Write("                                            Enter desired Account Number -  ");
                                        accNo = Convert.ToInt32(Console.ReadLine());
                                    }

                                    var manage = accountList.Where(b => b.AccountNo == accNo).FirstOrDefault();

                                    if (manage == null)
                                    {
                                        Console.Write("\n\n                             Account Does not Exist.");
                                        break;
                                    }
                                    else
                                    {
                                        Console.Write("\n\n                             Do you want to Activate (a) or Deactivate (d) Account?  ");
                                        string choice = Console.ReadLine();

                                        if (choice == "a")
                                        {
                                            manage.State = "Active";
                                            Console.WriteLine("\n\n                             Successfully Activated Account "+accNo);
                                        }
                                        else
                                        {
                                            manage.State = "Deactivated";
                                            Console.WriteLine("\n\n                                            Successfully Deactivated Account " + accNo);
                                        }
                                    }
                                }
                          


                            }
                            break;

                    }

                }
                while (run==true);
                }
        }
    }
}

