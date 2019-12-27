using System.Collections.Generic;

namespace DissAndAss.Assembly.Compiler
{
    public interface ICompiler
    {
        ushort[] Compile(List<List<Token>> lines);
    }
}