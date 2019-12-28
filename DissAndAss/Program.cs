
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


            if (args.Length <=0)
            {
                throw new Exception("No source specified");
            }

            if (args.Length <= 1)
            {
                throw new Exception("No Destination specified");
            }
            //ass.txt assembledTest --A

            string parameter = "--N";

            if (args.Length == 3)
            {
                parameter = args[2];
            }

            if (args.Length > 3)
            {
                throw new Exception("Use of undefined parameters");
            }


            if (parameter == "--A")
            {

                Assembler a = new Assembler(new Tokenizer(), new TokenToBinaryConverter());
                a.GenerateBinaryFile(args[0], args[1]);
            }    
            else
            {
                Disassembler d1 = new Disassembler(args[0], parameter);
                d1.ProccessFile(args[1]);
            }



            //Assembler a = new Assembler(new Tokenizer(), new TokenToBinaryConverter());
            ////a.GenerateSourceFile(args[0],args[1]);
            //a.GenerateBinaryFile("ass.txt", "testingDissasembly.c8");


            //Disassembler d1 = new Disassembler("testingDissasembly.c8", "-N");
            //d1.ProccessFile("ReassmbledWithComments.txt");
            //Console.ReadKey();
        }
    }
}
