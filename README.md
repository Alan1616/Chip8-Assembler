# Chip8-DisassembleAndAssemble
Complement to Chip8 interpreter.  
Disassembler for chip8  
usage Disasembler.exe source.c8 destination.txt --Param  
where --Param is empty or --N for typical assembly language or --HR for more readable one. 

Assembler for chip8
 
Basic assembler for chip8 language. It stays true to http://devernay.free.fr/hacks/chip8/C8TECH10.HTM documentation.
There are few changes like:  
- **LD F, Vx**  **LD B, Vx** are defined by **LD (F), Vx** and **LD (B), Vx**
cause F and B are assumed to be hex numbers  
- SHL and SHR operations use only Vx register, Vy is never used - y value always stays 0 
(which is how most interpeters interpret those codes anyway). Example **SHL V5**, **SHR VA**  

**for now every number whether it is register number or raw data is hexadecimal i.e VE not V15 
caue 15 = 21(10)**  
**One instruction per line, no nesting.**  
You can put specific data in place by using **DATA xxxx**.  

Todo:  
- [ ] - support for hex ( like 0xFFFF) and decimal values  
- [ ] - basic editor  
- [ ] - value labels  



