using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public partial class TokenDefinition
    {
        private readonly TokenType _returnsTokenType;
        private Func<string, Tuple<bool, string>> _matchAlgorithm;

        public TokenDefinition(TokenType type, Func<string, Tuple<bool, string>> matchingAlgorithm)
        {
            _returnsTokenType = type;
            _matchAlgorithm = matchingAlgorithm;
        }

        public TokenMatch MatchToken(string value)
        {
            Tuple<bool, string> algorithmResult = _matchAlgorithm.Invoke(value);

            if (algorithmResult.Item1)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    TokenType = _returnsTokenType,
                    Value = algorithmResult.Item2,
                    RemainingText = value.Substring(algorithmResult.Item2.Length)
                };
            }
            else
            {
                return new TokenMatch()
                {
                    IsMatch = false,
                };
            }
        }


    }
}
