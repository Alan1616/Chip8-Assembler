
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
            //Dissasembler.exe source.c8 destination.txt--Param

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

            Assembler a = new Assembler(new Tokenizer(), new Compiler());
            a.GenerateSourceFile();


            //Disassembler d1 = new Disassembler("Chip8 Picture.ch8", "--N");
            //d1.ProccessFile("HrDissasemble.txt");
            //Console.ReadKey();
        }
    }
}
