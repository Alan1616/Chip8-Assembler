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
                if (lineWithoutComments.Count <= 1)
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

            if (line.Any(x => x.Type == TokenType.Invalid))
            {
                throw new Exception($"Syntax Error at line {linesCount}, invalid token found");
            }

            List<Token> registerOperands = line.Where(x => x.Type == TokenType.GenericRegister).ToList();
            List<Token> valueOperands = line.Where(x => x.Type == TokenType.HeximalData).ToList();


            var mnemonicMatchingOperations = OperationsSet.OperationDefinitionsSet.Where(x => x.Mnemonic == line[0].Value.ToUpper()).ToList();


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
                    throw new Exception($"Cant jump from Register other Than V0 at line {linesCount}");
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

                uint value = uint.Parse(valueOperands[0].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                if (value > matchedOperation.FreeDataMaxLength )
                {
                    throw new Exception($"Hexadecimal value out of range at line {linesCount}");
                }

                value &=  matchedOperation.FreeDataMaxLength;

                code |= (ushort)value ;
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

        // coment always ends the line therefore there is only Sequence end token
        // afterwards so we can safely assume this.
        private List<Token> GetLineWithoutComments(List<Token> line)
        {     
            return line.Where(x => x.Type != TokenType.Comment).ToList();
        }


    }
}
