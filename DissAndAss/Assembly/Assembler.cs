using DissAndAss.Assembly.Compiler;
using DissAndAss.Assembly.Tokenizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DissAndAss.Assembly
{
    public class Assembler
    {
        private ITokenizer _tokenizer;
        private ITokenToBinaryConverter _compiler;
        //private List<string> _lines;


        public Assembler(ITokenizer tokenizer, ITokenToBinaryConverter compiler)
        {
            _tokenizer = tokenizer;
            _compiler = compiler;
        }


        public void GenerateSourceFile()
        {
            List<string> lines = new List<string>();
            List<List<Token>> linesByTokens = new List<List<Token>>();

            try
            {
                 lines = LoadLines("ass.txt");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


            foreach (string line in lines)
            {
                List<Token> tokens  =  _tokenizer.Tokenize(line);
                linesByTokens.Add(tokens);
            }

            ushort[] bytes = _compiler.Compile(linesByTokens);

            var fileName = "binaryOutput.c8";
            try
            {
                WriteByteDataToFile(fileName, bytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void WriteByteDataToFile(string fileName, ushort[] data)
        {
            using (BinaryWriter bwriter = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate),Encoding.BigEndianUnicode))
            {
                foreach (ushort item in data)
                {
                    byte b1 = (byte)  ((0xFF00 & item) >> 8);
                    byte b2 = (byte)  ((0x00FF & item) );

                    bwriter.Write(b1);
                    bwriter.Write(b2);
                }
                bwriter.Close();
            }
        }

        private List<string> LoadLines(string fileName)
        {

            return File.ReadAllLines(fileName).ToList();        
        }



    }
}
