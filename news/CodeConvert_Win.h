#include "stdafx.h"
// ---------------------------------
#ifndef CODECONVERT_WIN_H
#define CODECONVERT_WIN_H
// �ַ�����ת���ࣨʹ��Windows�ı���ת��API��
// ����RAIIԭ�򣬹������Ĺ��캯���з�����Դ�������������������ͷ���Դ
class CodeConvert_Win
{
public:
    CodeConvert_Win(const char* input, unsigned int fromCodePage, unsigned int toCodePage);
    // ���������������ͷŷ������Դ
    ~CodeConvert_Win() {
        delete [] wcharBuf;        
        delete [] charBuf;    
        };    
        const char * ToString() {    
            return charBuf;    
        };
private:    
      wchar_t * wcharBuf;    
      char * charBuf;
};
#endif // CODECONVERT_WIN_H