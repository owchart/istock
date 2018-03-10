
// ---------------------------------
#include "StdAfx.h"
#include <Windows.h>
#include "CodeConvert_Win.h"
// 两种编码间转换时，需要先转换成wchar，然后从wchar转换到目标编码（wchar，宽字符，unicode-16编码）
CodeConvert_Win::CodeConvert_Win( const char* input, unsigned int fromCodePage, unsigned int toCodePage )
{
	// 先获取转换后的长度
	int len = MultiByteToWideChar(fromCodePage, 0, input, -1, NULL, 0);
	// 分配宽字符空间
	wcharBuf = new wchar_t[len+1];
	memset(wcharBuf,0,sizeof(wchar_t)*(len+1));
	// 转换为宽字符
	MultiByteToWideChar(fromCodePage, 0, input, -1, wcharBuf, len);
	// 获取目标编码格式的字符串长度
	len = WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, NULL, 0, NULL, NULL);
	// 分配目标编码字符串空间
	charBuf = new char[len+1];
	memset(charBuf,0,sizeof(char)*(len+1));
	// 转换到目标编码
	WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, charBuf, len, NULL, NULL);
}