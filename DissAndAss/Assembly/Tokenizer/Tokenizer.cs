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
        private List<string> _mnemoics = OperationsSet.OperationDefinitionsSet.Select(x => x.Mnemonic).Distinct().ToList();

        private List<TokenDefinition> _tokenDefinitions = new List<TokenDefinition>();
        public  Tokenizer()
        {
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Comma, x => new Tuple<bool, string>(x.StartsWith(","), ",")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Space, x => new Tuple<bool, string>(x.StartsWith(" "), " ")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.K, x => new Tuple<bool, string>(x.StartsWith("K"), "K")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IRgeister, x => new Tuple<bool, string>(x.StartsWith("I"), "I")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IRange, x => new Tuple<bool, string>(x.StartsWith("[I]"), "[I]")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DT, x => new Tuple<bool, string>(x.StartsWith("DT"), "DT")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ST, x => new Tuple<bool, string>(x.StartsWith("ST"), "ST")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.F, x => new Tuple<bool, string>(x.StartsWith("(F)"), "(F)")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.B, x => new Tuple<bool, string>(x.StartsWith("(B)"), "(B)")));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Mnemonic, x =>
            {
                foreach (string mnemonic in _mnemoics)
                {
                    if (x.StartsWith(mnemonic))
                    {
                        return new Tuple<bool, string>(true, mnemonic);
                    }
                }
                return new Tuple<bool, string>(false, "");

            }));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.GenericRegister, x =>
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
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Comment, x =>
            {
                Regex regex = new Regex(@"^\//(.*)");

                Match match = regex.Match(x);

                if (match.Success)
                {
                    return new Tuple<bool, string>(true, match.Value);
                }
                return new Tuple<bool, string>(false, match.Value);

            }));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.HeximalData, x =>
            {
                var numeric = 0;

                Regex regex = new Regex("^[0-9a-fA-F]{1,4}");

                Match match = regex.Match(x);
                if (match.Success)
                {
                    var canConvertToHex = int.TryParse(match.Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out numeric);

                    if (canConvertToHex && numeric <= 0xFFFF)
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

                else if (match.IsMatch && match.TokenType == TokenType.Space)
                {
                    remainingText = remainingText.Substring(1);
                }

                else
                {
                    output.Add(CreateInvalidToken(remainingText));
                    remainingText = "";
                }

            }

            output.Add(new Token() { Type = TokenType.SequenceEnd, Value = String.Empty });

            return output;
        }

        private TokenMatch FindMatch(string text)
        {
            foreach (TokenDefinition tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.MatchToken(text);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }

        private Token CreateInvalidToken(string text)
        {
            return new Token()
            {
                Type = TokenType.Invalid,
                Value = text,
            };

        }

    }
}
