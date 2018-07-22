#include <iostream>
#include "XEDParse.h"
#define API extern "C" __declspec(dllexport)
#pragma comment(lib,"./lib/XEDParse_x86.lib")

bool UN(const char*, ULONGLONG *)
{
	return true;
}

API void GetAsmCode(const char *code, unsigned char **mcode, unsigned long long IP, size_t *len)
{
	unsigned char *v = (unsigned char *)malloc(256);
	XEDPARSE p;
	p.x64 = false;
	p.cip = IP;
	p.cbUnknown = UN;
	strcpy(p.instr, code);

	XEDParseAssemble(&p);

	*len = p.dest_size;
	for (size_t i = 0; i < p.dest_size; i++)
	{
		v[i] = p.dest[i];
	}
	*mcode = v;
}