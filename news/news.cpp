// news.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#include "DataCenter.h"
#include "NewsService.h"

extern "C" __declspec(dllexport) int GetNews(const char* code, char* str)
{
	DataCenter::StartService();
	string content = DataCenter::GetNewsService()->GetNews(code);
	int length = (int)content.size();
	strcpy(str, content.c_str());
	return length;
}

//int _tmain(int argc, _TCHAR* argv[])
//{
//	char str[102400] = {0};
//	GetNews("601857.SH", str);
//	return 0;
//}


