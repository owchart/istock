// news.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "DataCenter.h"
#include "NewsService.h"

extern "C" __declspec(dllexport) int AddSecurity(const char* code, const char *name, int innerCode, int type)
{
	Security security;
	Str::stringTowstring(security.m_code, code);
	Str::stringTowstring(security.m_name, name);
	security.m_innerCode = innerCode;
	security.m_type = type;
	DataCenter::StartService();
	DataCenter::GetSecurityService()->AddSecurity(&security);
	return 1;
}

extern "C" __declspec(dllexport) int GetNews(const char* code, char* str)
{
	DataCenter::StartService();
	string content = DataCenter::GetNewsService()->GetNews(code);
	int length = (int)content.size();
	strcpy(str, content.c_str());
	return length;
}

int _tmain(int argc, _TCHAR* argv[])
{
	char str[102400] = {0};
	GetNews("601857.SH", str);
	return 0;
}


