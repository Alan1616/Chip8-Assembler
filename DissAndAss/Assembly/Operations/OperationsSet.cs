using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public static class OperationsSet
    {
        private static List<OperationDefinition> _operationDefinitionSet = new List<OperationDefinition>();
        public static List<OperationDefinition> OperationDefinitionsSet { get => _operationDefinitionSet; }

        static OperationsSet()
        {
            // To be more specific with c8 documentation fields HasSource and HasTarget should be other way around.

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "0nnn", Mnemonic = "SYS", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFF,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "00E0", Mnemonic = "CLS", HasSource = false, HasTarget = false, HasFreeData = false,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "00EE", Mnemonic = "RET", HasSource = false, HasTarget = false, HasFreeData = false,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "1nnn", Mnemonic = "JP", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFF,  
                 AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "2nnn", Mnemonic = "CALL", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFF,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "3xkk", Mnemonic = "SE", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFF,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "4xkk", Mnemonic = "SNE", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFF,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "5xy0", Mnemonic = "SE", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "6xkk", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "7xkk", Mnemonic = "ADD", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy0", Mnemonic = "LD", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy1", Mnemonic = "OR", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy2", Mnemonic = "AND", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy3", Mnemonic = "XOR", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy4", Mnemonic = "ADD", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy5", Mnemonic = "SUB", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy6", Mnemonic = "SHR", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xy7", Mnemonic = "SUBN", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "8xyE", Mnemonic = "SHL", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "9xy0", Mnemonic = "SNE", HasSource = true, HasTarget = true, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Annn", Mnemonic = "LD", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.IRgeister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Bnnn", Mnemonic = "JP", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Cxkk", Mnemonic = "RND", HasSource = true, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Dxyn", Mnemonic = "DRW", HasSource = true, HasTarget = true, HasFreeData = true, FreeDataMaxLength = 0xF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Ex9E", Mnemonic = "SKP", HasSource = true, HasTarget = false, HasFreeData = false,
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "ExA1", Mnemonic = "SKNP", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx07", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.DT, TokenType.SequenceEnd } });

             _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx0A", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.K, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx15", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.DT, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx18", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.ST, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx1E", Mnemonic = "ADD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.IRgeister, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx29", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false,  
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.F, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx33", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.B, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx55", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.IRange, TokenType.Comma, TokenType.GenericRegister, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "Fx65", Mnemonic = "LD", HasSource = true, HasTarget = false, HasFreeData = false, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.IRange, TokenType.SequenceEnd } });

            _operationDefinitionSet.Add(new OperationDefinition { Representation = "0000", Mnemonic = "DATA", HasSource = false, HasTarget = false, HasFreeData = true, FreeDataMaxLength = 0xFFFF, 
                AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.HeximalData, TokenType.SequenceEnd } });
        }


    }
}
