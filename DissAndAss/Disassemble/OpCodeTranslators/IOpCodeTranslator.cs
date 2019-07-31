using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Disassemble.OpCodeTranslators
{
    internal interface IOpCodeTranslator
    {
        string TranslateOpcode(Opcode opcode);
    }
}
