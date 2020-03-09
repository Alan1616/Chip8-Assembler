using DissAndAss.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace DissAndAss.Tests.TestWrappers
{
    public class TestLineBuilder : IXunitSerializable
    {
        private TestTokenBuilder[] _tokens;

        // required for deserializer
        public TestLineBuilder()
        {

        }

        public TestLineBuilder(IEnumerable<TestTokenBuilder> tokens)
        {
            _tokens = tokens.ToArray();
        }

        public IEnumerable<Token> Build()
        {
            foreach (var t in _tokens)
            {
                yield return t.Build();
            }
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _tokens = info.GetValue<TestTokenBuilder[]>("tokens");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("tokens", _tokens, typeof(TestTokenBuilder[]));
        }
    }
}
