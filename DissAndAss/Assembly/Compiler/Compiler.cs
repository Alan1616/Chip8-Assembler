using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DissAndAss.Assembly.Compiler
{
    public class Compiler
    {

        public ushort[] Compile(List<List<Token>> lines)
        {
            List<ushort> bytes = new List<ushort>();
            var linesCount = 0;

            foreach (List<Token> line in lines)
            {

               linesCount++;
               Opcode currentOpcode =  ConvertLineToOpcode(line, linesCount);
               bytes.Add(currentOpcode.FullCode);

            }
        
        }

        private Opcode ConvertLineToOpcode(List<Token> line, int linesCount)
        {
            bool IsTokenMnemonic(Token token) => token.Type == TokenType.Mnemonic;

            line = GetLineWithoutComments(line);

            // first token should always be mnemonic 
            if (!IsTokenMnemonic(line[0]))
            {
                throw new Exception($"Syntax Error at line {linesCount}");
            }

            // no other token can be mnemonic
            for (int i = 1; i < line.Count; i++)
            {
                if (IsTokenMnemonic(line[i]))
                {
                    throw new Exception($"Syntax Error at line {linesCount}");
                }
            }
        


            var mnemonicMatchingOperations = OperationsSet.OperationsMap.Values.Where(x => x.Mnemonic == line[0].Value);



        }

        private List<Token> GetLineWithoutComments(List<Token> line)
        {
            // coment always ends the line therefore there is only Sequence end token
            // afterwards so we can safely assume this.
            return line.Where(x=> x.Type != TokenType.Comment).ToList();
        }


    }
}
