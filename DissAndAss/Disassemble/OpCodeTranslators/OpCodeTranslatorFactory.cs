using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Disassemble.OpCodeTranslators
{
    internal static class OpCodeTranslatorFactory
    {
        public static IOpCodeTranslator GetTranslator(string paramter)
        {
            switch (paramter)
            {
                case "--N":
                    return new OpCodeTranslator();
                case "--HR":
                    return new HumanReadableOpCodeTranslator();
                default:
                    return new OpCodeTranslator();
            }
        }
    }
}
