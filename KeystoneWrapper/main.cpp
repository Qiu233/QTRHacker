#include <keystone/keystone.h>
#include <iostream>
#define API extern "C" __declspec(dllexport)

#pragma comment(lib,"keystone.lib")

API void FreeCode(unsigned char *mcode)
{
	ks_free(mcode);
}

API int GetAsmCode(const char *code, unsigned char **mcode, size_t *len)
{
	ks_engine *ks;
	size_t count;
	ks_open(KS_ARCH_X86, KS_MODE_32, &ks);
	ks_asm(ks, code, 0, mcode, len, &count);
	ks_close(ks);

	return 0;
}