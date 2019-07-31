
using DissAndAss.Disassemble;
using System;

namespace DissAndAss
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dissasembler.exe source.c8 destination.txt--Param
            //Disassembler d1 = new Disassembler(args[0], args[2]);
            //d1.ProccessFile(args[1]);
            Disassembler d1 = new Disassembler("Chip8 Picture.ch8", "--N");
            d1.ProccessFile("HrDissasemble.txt");
            //Console.ReadKey();
        }
    }
}
