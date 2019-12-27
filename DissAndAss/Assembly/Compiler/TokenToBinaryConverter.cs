using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DissAndAss.Assembly.Compiler
{
    public class Compiler : ICompiler
    {

        public ushort[] Compile(List<List<Token>> lines)
        {
            List<ushort> bytes = new List<ushort>();
            var linesCount = 0;

            foreach (List<Token> line in lines)
            {

                linesCount++;

                var lineWithoutComments = GetLineWithoutComments(line);

                // Temporary solution for ignore only comment lines
                if (lineWithoutComments.Count <= 0)
                {
                    continue;
                }


                Opcode currentOpcode = ConvertLineToOpcode(lineWithoutComments, linesCount);
                bytes.Add(currentOpcode.FullCode);

            }

            throw new NotImplementedException();
        }

        private Opcode ConvertLineToOpcode(List<Token> line, int linesCount)
        {
            bool IsTokenMnemonic(Token token) => token.Type == TokenType.Mnemonic;
            bool IsTokenInvalid(Token token) => token.Type == TokenType.Invalid;

            // TODO = this will cause problem
            line = GetLineWithoutComments(line);

            // first token should always be mnemonic 
            if (!IsTokenMnemonic(line[0]))
            {
                throw new Exception($"Syntax Error at line {linesCount}");
            }

            for (int i = 1; i < line.Count; i++)
            {
                // no other token can be mnemonic
                if (IsTokenMnemonic(line[i]))
                {
                    throw new Exception($"Syntax Error at line {linesCount}, there might be more than one mnemonic in line");
                }
                // no token can be marked as invalid duH!
                if (IsTokenInvalid(line[i]))
                {
                    throw new Exception($"Syntax Error at line {linesCount}, invalid token {line[i].Value}");
                }

            }

            bool IsSpecialOperand(Token token) => token.Type == TokenType.DT ||
                        token.Type == TokenType.IRange ||
                        token.Type == TokenType.IRgeister ||
                        token.Type == TokenType.ST ||
                        token.Type == TokenType.K;

            //bool IsOperand(Token token) => token.Type == TokenType.DT ||
            //    token.Type == TokenType.GenericRegister ||
            //    token.Type == TokenType.HeximalData ||
            //    token.Type == TokenType.IRange ||
            //    token.Type == TokenType.IRgeister ||
            //    token.Type == TokenType.ST ||
            //    token.Type == TokenType.K;

            bool IsOperand(Token token) => token.Type == TokenType.GenericRegister ||
                token.Type == TokenType.HeximalData;
     


            // check for comma unless after first and there is only one or after last operation

            var mnemonicMatchingOperations = OperationsSet.OperationsMap.Values.Where(x => x.Mnemonic == line[0].Value).ToList();
            int operandCount = line.Where(IsOperand).Count();
            mnemonicMatchingOperations = mnemonicMatchingOperations.Where(x => x.OperandsCount == operandCount).ToList();

            if (mnemonicMatchingOperations.Count() <= 0)
            {
                throw new Exception($"Syntax Error at line {linesCount}, cant match mnemonic operand count");
            }

            return new Opcode(0x00EA);


        }



        private List<Token> GetLineWithoutComments(List<Token> line)
        {
            // coment always ends the line therefore there is only Sequence end token
            // afterwards so we can safely assume this.
            return line.Where(x => x.Type != TokenType.Comment).ToList();
        }


        //private Opcode Get_CLS(List<Token> line)
        //{
        //    var mnemonicMatchingOperations = OperationsSet.OperationsMap.Values.Where(x => x.Mnemonic == line[0].Value);

        //}


    }
}
