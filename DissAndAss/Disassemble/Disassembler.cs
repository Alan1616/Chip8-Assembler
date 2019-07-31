
using DissAndAss.Disassemble.OpCodeTranslators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DissAndAss.Disassemble
{
    public class Disassembler
    {
        private const int startingPC = 512;
        private List<byte> binaryCode = new List<byte>();
        private List<string> assemblyData = new List<string>();
        IOpCodeTranslator OpCodeTranslator;

        public Disassembler(string filePath, string translatorParameter)
        {
            OpCodeTranslator = OpCodeTranslatorFactory.GetTranslator(translatorParameter);

            try
            {
                LoadBinaryFile(filePath);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load source file");
            }

        }

        public void ProccessFile(string destinationFile)
        {

            for (int i = 0; i < binaryCode.Count; i+=2)
            {
                ushort opcodeNumeric = (ushort)(binaryCode[i] << 8 | binaryCode[i + 1]);
                Opcode opcode = new Opcode(opcodeNumeric);

                string mnemonic = OpCodeTranslator.TranslateOpcode(opcode);
                assemblyData.Add($"{startingPC + i:X3}  {opcode.FullCode:X4}    {mnemonic}");
            }

            try
            {
                File.WriteAllLines(destinationFile, assemblyData);
            }
            catch (Exception)
            {

                Console.WriteLine("Unable to create destination file");
            }


        }

        private void LoadBinaryFile(string filePath)
        {
            using (BinaryReader s1 = new BinaryReader(File.Open(filePath, FileMode.Open), Encoding.BigEndianUnicode))
            {
                while (s1.BaseStream.Position < s1.BaseStream.Length)
                {
                    binaryCode.Add(s1.ReadByte());
                }
            }
        }
    }
}
