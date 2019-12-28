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


                ushort currentOpcode = ConvertLineToBinaryData(lineWithoutComments, linesCount);
                bytes.Add(currentOpcode);
            }

            return bytes.ToArray();
        }

        private ushort ConvertLineToBinaryData(List<Token> line, int linesCount)
        {

            // TODO = this will cause problem
            //line = GetLineWithoutComments(line);

            bool IsSpecialOperand(Token token) => token.Type == TokenType.DT ||
                        token.Type == TokenType.IRange ||
                        token.Type == TokenType.IRgeister ||
                        token.Type == TokenType.ST ||
                        token.Type == TokenType.K;

            bool IsOperand(Token token) => token.Type == TokenType.GenericRegister ||
                token.Type == TokenType.HeximalData;


            List<Token> operands = line.Where(x => IsOperand(x)).ToList();
            List<Token> registerOperands = operands.Where(x => x.Type == TokenType.GenericRegister).ToList();
            List<Token> valueOperands = operands.Where(x => x.Type == TokenType.HeximalData).ToList();
            List<Token> specialOperands = operands.Where(x => IsSpecialOperand(x)).ToList();


            var mnemonicMatchingOperations = OperationsSet.OperationsMap.Values.Where(x => x.Mnemonic == line[0].Value).ToList();


            int operandCount = line.Where(IsOperand).Count();
            mnemonicMatchingOperations = mnemonicMatchingOperations.Where(x => LineMatchesTokenSetInGivenDefinition(x, line)).ToList();

            if (mnemonicMatchingOperations.Count() <= 0)
            {
                throw new Exception($"Syntax Error at line {linesCount}, cant match mnemonic with any definition");
            }


            if (mnemonicMatchingOperations.Count > 1 )
            {
                throw new Exception("There are more than one definition matching operations (should never happen outside of testing)");
            }


            OperationDefinition matchedOperation = mnemonicMatchingOperations.First();

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

                value = ushort.Parse(registerOperands[1].Value.Replace("V", String.Empty), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                code = (ushort)(code | (value << 4));
            }

            if (matchedOperation.HasFreeData)
            {

                ushort value = ushort.Parse(valueOperands[0].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                value &=  matchedOperation.FreeDataMaxLength;

                code |= value ;
            }

            return code;

        }


        private bool LineMatchesTokenSetInGivenDefinition(OperationDefinition definition, List<Token> line)
        {
            if (definition.AssocietedTokenSet.Count != line.Count)
            {
                return false;
            }

            for (int i = 0; i < line.Count; i++)
            {
                if (definition.AssocietedTokenSet[i] != line[i].Type)
                {
                    return false;
                }
            }

            return true;
        }

        //private bool IsActualOperand(Token token) => token.Type == TokenType.DT ||
        //    token.Type == TokenType.GenericRegister ||
        //    token.Type == TokenType.HeximalData ||
        //    token.Type == TokenType.IRange ||
        //    token.Type == TokenType.IRgeister ||
        //    token.Type == TokenType.ST ||
        //    token.Type == TokenType.K;

        //private bool IsLineStructureGood(List<Token> line)
        //{
        //    var enumerator = line.GetEnumerator();

        //    Token previous = new Token();
           

        //    while (enumerator.MoveNext())
        //    {
        //        var current = enumerator.Current;

        //        if ( previous.Type == TokenType.Mnemonic && (!IsActualOperand(current) && current.Type != TokenType.SequenceEnd) )
        //        {
        //            return false;
        //        }

        //        if ( IsActualOperand(previous) && ( current.Type != TokenType.Comma && current.Type != TokenType.SequenceEnd) )
        //        {
        //            return false;
        //        }

        //        if ( previous.Type == TokenType.Comma && ( !IsActualOperand(current) && current.Type != TokenType.SequenceEnd) )
        //        {
        //            return false;
        //        }


        //        previous = enumerator.Current;

        //    }

        //    return true;

        //}



        private List<Token> GetLineWithoutComments(List<Token> line)
        {
            // coment always ends the line therefore there is only Sequence end token
            // afterwards so we can safely assume this.
            return line.Where(x => x.Type != TokenType.Comment).ToList();
        }


    }
}
