
using DissAndAss.Assembly;
using DissAndAss.Assembly.Compiler;
using DissAndAss.Assembly.Tokenizer;
using DissAndAss.Disassemble;
using System;
using System.Linq;

namespace DissAndAss
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dissasembler.exe source.c8 destination.txt --Param

            //if (String.IsNullOrEmpty(args[0]))
            //{
            //    Console.WriteLine("No source specified");
            //}

            //if (String.IsNullOrEmpty(args[1]))
            //{
            //    Console.WriteLine("No Destination specified");
            //}


            //if (String.IsNullOrEmpty(args[2]))
            //{
            //    args[1] = "--N";
            //}




            //if (args[2] == "A")
            //{

            //    if (args[1].Count(x => x == '.') == 1)
            //    {
            //        if (args[1].Split('.').ToArray()[1].Equals("txt"))
            //        {
            //            Assembler a1 = new Assembler();
            //            a1.AssembleToFile();
            //        }
            //    }

            //    else
            //    {
            //        throw new FormatException("your format should be filename.txt");
            //    }

            //}

            //Disassembler d1 = new Disassembler(args[0], args[2]);
            //d1.ProccessFile(args[1]);

            //Assembler a = new Assembler(new Tokenizer(), new TokenToBinaryConverter());
            ////a.GenerateSourceFile(args[0],args[1]);
            //a.GenerateBinaryFile("ass.txt", "testingDissasembly.c8");


            Disassembler d1 = new Disassembler("testingDissasembly.c8", "-N");
            d1.ProccessFile("ReassmbledWithComments.txt");
            //Console.ReadKey();
        }
    }
}
