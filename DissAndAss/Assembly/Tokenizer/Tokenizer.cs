using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DissAndAss.Assembly.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private List<string> Mnemoics = OperationsSet.OperationsMap.Values.Select(x => x.Mnemonic).Distinct().ToList();

        private List<TokenDefinition> TokenDefinitions = new List<TokenDefinition>();
        public  Tokenizer()
        {
            TokenDefinitions.Add(new TokenDefinition(TokenType.Comma, x => new Tuple<bool, string>(x.StartsWith(","), ",")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Space, x => new Tuple<bool, string>(x.StartsWith(" "), " ")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.K, x => new Tuple<bool, string>(x.StartsWith("K"), "K")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.IRgeister, x => new Tuple<bool, string>(x.StartsWith("I"), "I")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.IRange, x => new Tuple<bool, string>(x.StartsWith("[I]"), "[I]")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.DT, x => new Tuple<bool, string>(x.StartsWith("DT"), "DT")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.ST, x => new Tuple<bool, string>(x.StartsWith("ST"), "ST")));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Mnemonic, x =>
            {
                foreach (string mnemonic in Mnemoics)
                {
                    if (x.StartsWith(mnemonic))
                    {
                        return new Tuple<bool, string>(true, mnemonic);
                    }
                }
                return new Tuple<bool, string>(false, "");

            }));
            TokenDefinitions.Add(new TokenDefinition(TokenType.GenericRegister, x =>
            {
                var numeric = 0;

                Regex regex = new Regex("^V[0-9a-fA-F]{1,2}");

                Match match = regex.Match(x);

                if (match.Success)
                {
                    var number = match.Value.Substring(1);

                    var canConvertToHex = int.TryParse(number, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numeric);

                    if (canConvertToHex && numeric <= 15)
                    {
                        return new Tuple<bool, string>(true, match.Value);
                    }

                }
                return new Tuple<bool, string>(false, match.Value);

            }));
            TokenDefinitions.Add(new TokenDefinition(TokenType.Comment, x =>
            {
                Regex regex = new Regex(@"^\//(.*)");

                Match match = regex.Match(x);

                if (match.Success)
                {
                    return new Tuple<bool, string>(true, match.Value);
                }
                return new Tuple<bool, string>(false, match.Value);

            }));
            TokenDefinitions.Add(new TokenDefinition(TokenType.HeximalData, x =>
            {
                var numeric = 0;

                Regex regex = new Regex("^[0-9a-fA-F]{1,3}");

                Match match = regex.Match(x);
                if (match.Success)
                {
                    var canConvertToHex = int.TryParse(match.Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numeric);

                    if (canConvertToHex && numeric <= 4095)
                    {
                        return new Tuple<bool, string>(true, match.Value);
                    }


                }
                return new Tuple<bool, string>(false, numeric.ToString());

            }));
        }

        public List<Token> Tokenize(string line)
        {
            List<Token> output = new List<Token>();

            string remainingText = line;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);

                if (match.IsMatch && match.TokenType != TokenType.Space)
                {
                    output.Add(new Token() { Type = match.TokenType, Value = match.Value });
                    remainingText = match.RemainingText;
                }

                else if (match.TokenType == TokenType.Space)
                {
                    remainingText = remainingText.Substring(1);
                }

                else
                {
                    //Create invalid token
                    output.Add(CreateInvalidTokenMatch(remainingText));
                    remainingText = "";
                    //throw new Exception("Invalid token");
                }

            }

            output.Add(new Token() { Type = TokenType.SequenceEnd, Value = String.Empty });

            return output;
        }

        private TokenDefinition.TokenMatch FindMatch(string text)
        {
            foreach (TokenDefinition tokenDefinition in TokenDefinitions)
            {
                var match = tokenDefinition.MatchToken(text);
                if (match.IsMatch)
                    return match;
            }

            return new TokenDefinition.TokenMatch() { IsMatch = false };
        }

        private Token CreateInvalidTokenMatch(string lqlText)
        {
            return new Token()
            {
                Type = TokenType.Invalid,
                Value = lqlText,
            };

        }

    }
}
