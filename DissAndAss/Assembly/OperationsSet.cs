using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public static class OperationsSet
    {
        private static List<Operation> _operationSet = new List<Operation>();
        public static Dictionary<string, Operation> OperationsMap { get; set; } = new Dictionary<string, Operation>();


        static OperationsSet()
        {
            _operationSet.Add(new Operation { Representation = "0nnn", Mnemonic = "SYS", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataLength = 3 });
            _operationSet.Add(new Operation { Representation = "00EE", Mnemonic = "CLS", HasSource = false, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "1nnn", Mnemonic = "RET", HasSource = false, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "2nnn", Mnemonic = "JP", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataLength = 3});
            _operationSet.Add(new Operation { Representation = "3xkk", Mnemonic = "CALL", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataLength = 3});
            _operationSet.Add(new Operation { Representation = "4xkk", Mnemonic = "SE", HasSource = true, HasTarget = false, HasFreeData = true,  FreeDataLength = 2});
            _operationSet.Add(new Operation { Representation = "5xy0", Mnemonic = "SE", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "6xkk", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataLength = 2});
            _operationSet.Add(new Operation { Representation = "7xkk", Mnemonic = "ADD", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataLength = 2 });
            _operationSet.Add(new Operation { Representation = "8xy0", Mnemonic = "LD", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy1", Mnemonic = "OR", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy2", Mnemonic = "AND", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy3", Mnemonic = "XOR", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy4", Mnemonic = "ADD", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy5", Mnemonic = "SUB", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy6", Mnemonic = "SHR", HasSource = true, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xy7", Mnemonic = "SUBN", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "8xyE", Mnemonic = "SHL", HasSource = true, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "9xy0", Mnemonic = "SHL", HasSource = true, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Annn", Mnemonic = "LD", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataLength = 3 });
            _operationSet.Add(new Operation { Representation = "Bnnn", Mnemonic = "JP", HasSource = false, HasTarget = false, HasFreeData = false,  FreeDataLength = 3});
            _operationSet.Add(new Operation { Representation = "Cxkk", Mnemonic = "RND", HasSource = true, HasTarget = false, HasFreeData = true,  FreeDataLength = 2});
            _operationSet.Add(new Operation { Representation = "Dxyn", Mnemonic = "DRW", HasSource = true, HasTarget = true, HasFreeData = true, FreeDataLength = 1 });
            _operationSet.Add(new Operation { Representation = "Ex9E", Mnemonic = "SKP", HasSource = true, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "ExA1", Mnemonic = "SKNP", HasSource = true, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx07", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx15", Mnemonic = "LD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx18", Mnemonic = "LD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx1E", Mnemonic = "ADD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx29", Mnemonic = "LD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx33", Mnemonic = "LD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx55", Mnemonic = "LD", HasSource = false, HasTarget = true, HasFreeData = false, });
            _operationSet.Add(new Operation { Representation = "Fx65", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, });

            foreach (var operation in _operationSet)
            {
                OperationsMap.Add(operation.Representation, operation);
            }
        }


    }
}
