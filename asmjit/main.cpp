#include <asmtk/asmtk.h>
#include <algorithm>

using namespace asmjit;
using namespace asmtk;


extern "C" __declspec(dllexport) void ParseAssemble(const char*instr, bool x64, UINT32 cip, unsigned char*dest, UINT32* dest_size)
{
	CodeInfo codeinfo(x64 ? ArchInfo::kTypeX64 : ArchInfo::kTypeX86, 0, cip);
	CodeHolder code;
	code.init(codeinfo);
	X86Assembler a(&code);
	AsmParser p(&a);
	p.parse(instr);
	code.sync();

	auto & buffer = code.getSectionEntry(0)->getBuffer();
	*dest_size = (unsigned int)buffer.getLength();
	memcpy(dest, buffer.getData(), *dest_size);

}