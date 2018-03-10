
// ---------------------------------
#include "StdAfx.h"
#include <Windows.h>
#include "CodeConvert_Win.h"
// ���ֱ����ת��ʱ����Ҫ��ת����wchar��Ȼ���wcharת����Ŀ����루wchar�����ַ���unicode-16���룩
CodeConvert_Win::CodeConvert_Win( const char* input, unsigned int fromCodePage, unsigned int toCodePage )
{
	// �Ȼ�ȡת����ĳ���
	int len = MultiByteToWideChar(fromCodePage, 0, input, -1, NULL, 0);
	// ������ַ��ռ�
	wcharBuf = new wchar_t[len+1];
	memset(wcharBuf,0,sizeof(wchar_t)*(len+1));
	// ת��Ϊ���ַ�
	MultiByteToWideChar(fromCodePage, 0, input, -1, wcharBuf, len);
	// ��ȡĿ������ʽ���ַ�������
	len = WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, NULL, 0, NULL, NULL);
	// ����Ŀ������ַ����ռ�
	charBuf = new char[len+1];
	memset(charBuf,0,sizeof(char)*(len+1));
	// ת����Ŀ�����
	WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, charBuf, len, NULL, NULL);
}