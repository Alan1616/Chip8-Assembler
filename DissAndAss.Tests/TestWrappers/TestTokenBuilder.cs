using DissAndAss.Assembly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace DissAndAss.Tests.TestWrappers
{
    public class TestTokenBuilder : IXunitSerializable
    {
        private Token _token;

        public TestTokenBuilder()
        {

        }
        public TestTokenBuilder(Token token)
        {
            _token = token;
        }

        public Token Build()
        {
            return _token;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _token = new Token();
            _token.Type = info.GetValue<TokenType>("type");
            _token.Value = info.GetValue<string>("value");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("type", _token.Type, typeof(TokenType));
            info.AddValue("value", _token.Value, typeof(string));
        }
    }
}
