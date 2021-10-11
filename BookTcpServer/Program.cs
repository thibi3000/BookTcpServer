using BookLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BookTcpServer
{
    class Program
    {
        private static List<Book> Books = new List<Book>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the book server");
            TcpListener listener = new TcpListener(IPAddress.Any, 4646);

            listener.Start();
            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Client incoming");
                Task.Run(() =>
               {
                   HandleClient(socket);
               });
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamWriter writer = new StreamWriter(ns);
            StreamReader reader = new StreamReader(ns);

            while (true)
            {
                Console.WriteLine("Type the number of what you want to do");
                Console.WriteLine("1. Get all books \n 2. Get a Certain Book \n 3. Add a book to the list \n 4.Quit");
                string answer = reader.ReadLine();

                if (answer.Equals("1"))
                {
                    GetAll();
                }
                else if (answer.Equals("2"))
                {
                    Console.WriteLine("Please give the isbn of the book you want:");
                    string isbn = reader.ReadLine();
                    Get(isbn);
                }
                else if (answer.Equals("3"))
                {
                    Console.WriteLine("Please specify the isbn of the book:");
                    string isbn = reader.ReadLine();
                    Console.WriteLine("\nPlease specify the title of the book:");
                    string title = reader.ReadLine();
                    Console.WriteLine("\nPlease specify the author of the book:");
                    string author = reader.ReadLine();
                    Console.WriteLine("\nPlease specify the amount of pages of the book:");
                    int pages = Int32.Parse(reader.ReadLine());

                    Add(isbn, title, author, pages);
                }
                else if (answer.Equals("4"))
                {
                    writer.WriteLine("Bye");
                    writer.Flush();
                    socket.Close();
                    break;
                }
                else
                {
                    Console.WriteLine("please specify a number from 1 to 3");
                }
            }
                
        }

        private static string GetAll()
        {
            Console.WriteLine("See below all the books from the list:");
            foreach(Book book in Books)
            {
                return $"Title: {book.Title}, ISBN: {book.ISBN13}, Author: {book.Author}, Number of Pages: {book.PageNumber}\n";
            }
            return "Failed";
        }

        private static Book Get(string isbn)
        {
            return Books.Find(book => book.ISBN13 == isbn);
        }

        private static void Add(string isbn, string title, string author, int pages)
        {
            Book book = new Book(isbn, title, author, pages);
            Books.Add(book);
        }
    }
}
