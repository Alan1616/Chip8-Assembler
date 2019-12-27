using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DissAndAss.Assembly.Compiler
{
    public class TokenToBinaryConverter : ITokenToBinaryConverter
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

            if (!IsLineStructureGood(line))
            {
                throw new Exception($"Syntax Error at line {linesCount}, check your line structure - [MNM, (OP1), (OP2), (OP3)]");
            }

            bool IsSpecialOperand(Token token) => token.Type == TokenType.DT ||
                        token.Type == TokenType.IRange ||
                        token.Type == TokenType.IRgeister ||
                        token.Type == TokenType.ST ||
                        token.Type == TokenType.K;

            bool IsOperand(Token token) => token.Type == TokenType.GenericRegister ||
                token.Type == TokenType.HeximalData;



            // check for comma unless after first and there is only one or after last operation


            List<Token> operands = line.Where(x => IsOperand(x)).ToList();
            List<Token> registerOperands = operands.Where(x => x.Type == TokenType.GenericRegister).ToList();
            List<Token> valueOperands = operands.Where(x => x.Type == TokenType.HeximalData).ToList();
            List<Token> specialOperands = operands.Where(x => IsSpecialOperand(x)).ToList();


            var mnemonicMatchingOperations = OperationsSet.OperationsMap.Values.Where(x => x.Mnemonic == line[0].Value).ToList();
            int operandCount = line.Where(IsOperand).Count();
            mnemonicMatchingOperations = mnemonicMatchingOperations.Where(x =>
            {
                var operandsMatch = x.OperandsCount == operands.Count;
                var dataOperandsMatch = false;

                if (x.HasFreeData && valueOperands.Count == 1 ) //add value is less or equal than max value
                {
                    dataOperandsMatch = true;
                }

                else if (!x.HasFreeData && valueOperands.Count == 0)
                {
                    dataOperandsMatch = true;
                }


                return operandsMatch && dataOperandsMatch;


            }).ToList();

            if (mnemonicMatchingOperations.Count() <= 0)
            {
                throw new Exception($"Syntax Error at line {linesCount}, cant match mnemonic with operand count");
            }


            if (mnemonicMatchingOperations.Count > 1 )
            {
                throw new Exception("Dupa");
            }

            var matchedOperation = mnemonicMatchingOperations.First();


            //if mnemonic LD do diffrent
            if (matchedOperation.Mnemonic != "LD")
            {
                ushort code = 0x0000;

                code = (ushort)(code | (matchedOperation.BaseValue ));

                if (matchedOperation.HasSource)
                {           
                    ushort value = ushort.Parse(registerOperands[0].Value.Replace("V", String.Empty), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                    if (matchedOperation.Mnemonic == "JP" && value !=0)
                    {
                        throw new Exception("Cant jump from Register other Than V0");
                    }

                    code = (ushort)(code | (value << 8));
                }

                if (matchedOperation.HasTarget)
                {
                    ushort value = 0;

                    if (matchedOperation.HasSource)
                    {
                         value = ushort.Parse(registerOperands[1].Value.Replace("V", String.Empty), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                         code = (ushort)(code | (value << 4));
                    }
                    else
                    {
                        value = ushort.Parse(registerOperands[0].Value.Replace("V", String.Empty), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                        code = (ushort)(code | (value << 8));
                    }



                }

                if (matchedOperation.HasFreeData)
                {

                    ushort value = ushort.Parse(valueOperands[0].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                    value &=  matchedOperation.FreeDataMaxLength;

                    code |= value ;
                }

                return new Opcode(code);

            }

            else
            {

            }
            return new Opcode(0);


        }

        private bool IsActualOperand(Token token) => token.Type == TokenType.DT ||
            token.Type == TokenType.GenericRegister ||
            token.Type == TokenType.HeximalData ||
            token.Type == TokenType.IRange ||
            token.Type == TokenType.IRgeister ||
            token.Type == TokenType.ST ||
            token.Type == TokenType.K;

        private bool IsLineStructureGood(List<Token> line)
        {
            var enumerator = line.GetEnumerator();

            Token previous = new Token();
           

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if ( previous.Type == TokenType.Mnemonic && (!IsActualOperand(current) && current.Type != TokenType.SequenceEnd) )
                {
                    return false;
                }

                if ( IsActualOperand(previous) && ( current.Type != TokenType.Comma && current.Type != TokenType.SequenceEnd) )
                {
                    return false;
                }

                if ( previous.Type == TokenType.Comma && ( !IsActualOperand(current) && current.Type != TokenType.SequenceEnd) )
                {
                    return false;
                }


                previous = enumerator.Current;

            }

            return true;

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
