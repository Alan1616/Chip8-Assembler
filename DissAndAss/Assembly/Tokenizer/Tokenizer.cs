using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DissAndAss.Assembly.Tokenizer
{
    public static class Tokenizer
    {
        private static List<string> Mnemoics = OperationsSet.OperationsMap.Values.Select(x => x.Mnemonic).Distinct().ToList();

        public static List<TokenDefinition> TokenDefinitions = new List<TokenDefinition>();
        static Tokenizer()
        {
            TokenDefinitions.Add(new TokenDefinition(TokenType.Comma, x=> x=="," ));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Space, x => x == " "));
            TokenDefinitions.Add(new TokenDefinition(TokenType.K, x => x == "K"));
            TokenDefinitions.Add(new TokenDefinition(TokenType.IRgeister, x => x == "I"));
            TokenDefinitions.Add(new TokenDefinition(TokenType.IRange, x => x == "[I]"));
            TokenDefinitions.Add(new TokenDefinition(TokenType.DT, x => x == "DT"));
            TokenDefinitions.Add(new TokenDefinition(TokenType.ST, x => x == "ST"));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Mnemonic, x => Mnemoics.Contains(x)));
            TokenDefinitions.Add(new TokenDefinition(TokenType.GenericRegister, x => 
            {
                if (x[0] == 'V' && x.Length <= 3 && x.Length >= 2)
                {
                    var number = x.Substring(1, x.Length - 1);
                    var numberic = 0;
                    var canConvertToHex = int.TryParse(number, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numberic);

                    if (canConvertToHex && numberic <= 15)
                    {
                        return true;
                    }
                    

                }
                return false;
            
            }));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Comment, x => x.StartsWith("//")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.HeximalData, x => 
            {
                if (x.Length > 0 && x.Length < 4)
                {
                    var numberic = 0;
                    var canConvertToHex = int.TryParse(x, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numberic);

                    if (canConvertToHex && numberic <= 4095)
                    {
                        return true;
                    }


                }
                return false;


            }));

        }


        public static List<Token> Tokenize(string line)
        {
            var output = new List<Token>();
            while (!string.IsNullOrWhiteSpace(line))
            {



            }

            output.Add(new Token() { Type = TokenType.SequenceEnd, Value = String.Empty });

            return output;
        }


    }
}
