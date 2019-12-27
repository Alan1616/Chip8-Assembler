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
        //private List<string> _lines;


        public Assembler(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }


        public void Compile()
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

            byte[] bytes =  _compiler.Compile(linesByTokens);

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

        private void WriteByteDataToFile(string fileName, byte[] data)
        {
            using (BinaryWriter bwriter = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate),Encoding.BigEndianUnicode))
            {
                bwriter.Write(data);
            }
        }

        private List<string> LoadLines(string fileName)
        {

            return File.ReadAllLines(fileName).ToList();        
        }



    }
}
