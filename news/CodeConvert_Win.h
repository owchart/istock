#include "stdafx.h"
// ---------------------------------
#ifndef CODECONVERT_WIN_H
#define CODECONVERT_WIN_H
// 字符编码转换类（使用Windows的编码转换API）
// 运用RAII原则，管理对象的构造函数中分配资源，由其析构函数负责释放资源
class CodeConvert_Win
{
public:
    CodeConvert_Win(const char* input, unsigned int fromCodePage, unsigned int toCodePage);
    // 析构函数，负责释放分配的资源
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