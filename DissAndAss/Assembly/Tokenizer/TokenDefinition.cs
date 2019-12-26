using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public class TokenDefinition
    {
        private readonly TokenType _returnsTokenType;
        private Func<string,bool> _matchAlgorithm;

        public TokenDefinition(TokenType type, Func<string,bool> matchingAlgorithm)
        {
            _returnsTokenType = type;
            matchingAlgorithm = _matchAlgorithm;
        }

        public Token MatchToken(string value,Func<string,bool> algorithm)
        {
            if (algorithm.Invoke(value))
            {
                return new Token()
                {
                    Type = _returnsTokenType,
                    Value = value,
                };
            }
            else
            {
                return new Token()
                {
                    Type = TokenType.NotDefined,
                    Value = value,
                };
            }
        }

    }
}
