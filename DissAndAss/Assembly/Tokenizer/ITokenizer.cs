using System.Collections.Generic;

namespace DissAndAss.Assembly.Tokenizer
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string line);
    }
}