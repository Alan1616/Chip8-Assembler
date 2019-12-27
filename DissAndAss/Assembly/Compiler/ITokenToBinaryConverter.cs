using System.Collections.Generic;

namespace DissAndAss.Assembly.Compiler
{
    public interface ITokenToBinaryConverter
    {
        ushort[] Compile(List<List<Token>> lines);
    }
}