using System;
using Read;
using Write;

namespace RFID
{
    class Menu
    {
        public static void Main()
        {
            Console.WriteLine("Make a selection from the following options and press enter:\n1. Press R to Read \n2. Press W to write \n3. Press Q to exit");
            var userSelection = Console.ReadLine();

            if(userSelection == "r" | userSelection == "R")
            {
                Console.WriteLine("Place Card to Read");
                Reader.init();
                //Reader.Program.ReadCode();
            }

            if(userSelection == "w" | userSelection == "W")
            {
                Console.WriteLine("Place Card to Write");
                while(true)
                {
                    Write.Program.WriteCode();
                }                
            }

            if(userSelection == "q" | userSelection == "Q")
            {
                Console.WriteLine("Exiting");
                return;
            }

            else
            {
                Console.WriteLine("Invalid selection.");
                Main();
            }

        }
    }
}
