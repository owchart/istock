#include "stdafx.h"
#include "NewsService.h"
#include "MakeNewsInfo.h"
#include "CurlHttp.h"
using namespace OwLib;

NewsService::NewsService()
{
	string userid = "";
	string path = Str::GetProgramDir() + "\\config\\userid.txt";
	CFileA::Read(path.c_str(), &userid);
	m_userids = Str::Split(userid, "\n");
}

NewsService::~NewsService()
{
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

string NewsService::GetRandomUserID()
{
	int useridSize = (int)m_userids.size();
	return m_userids[rand() % useridSize];
}


bool WriteFile(const std::string& fileName, char* data, long length)
{
	FILE* f = fopen(fileName.c_str(), "a");
	if(f)
	{
		fwrite(data, length, 1, f);
		fclose(f);
	}
	return true;
}


string NewsService::GetNews(string code)
{
	String wCode = L"";
	Str::stringTowstring(wCode, code);
	CurlHttp *pHttpcurl = CurlHttp::GetInstance();
	Security security;
	DataCenter::GetSecurityService()->GetSecurity(wCode, &security);

	vector<int> g_vecSecuVarietyCode;
	map<int, StockVersionInfo> g_mapVersion; 
	g_vecSecuVarietyCode.push_back(security.m_innerCode);
	StockVersionInfo svInfo;
	g_mapVersion[security.m_innerCode] = svInfo;

	string url = "http://183.136.163.249:1818/market/web.action";
	int totalLen = 0;
	//请求新闻
	char *ps = CMakeNewInfo::MakeNewReq(50, g_vecSecuVarietyCode, g_mapVersion, totalLen);

	string resultNews = pHttpcurl->Post(url, ps, totalLen);

	//请求公告
	ps = CMakeNewInfo::MakeBulletinReq(50, g_vecSecuVarietyCode, g_mapVersion, totalLen);
	string resultBulletin = pHttpcurl->Post(url, ps, totalLen);

	//请求研报
	ps = CMakeNewInfo::MakeReportReq(50, g_vecSecuVarietyCode, g_mapVersion, totalLen);
	string resultReport = pHttpcurl->Post(url, ps, totalLen);

	MarketInfoItemPVector *pVec = new MarketInfoItemPVector;  //vector数组指针
	//--------------http解析--新闻返回-----------------------------------------------
	const char *rstr = resultNews.c_str();
	int result = resultNews.length();
	if(result > 4)
	{
		CBuffer buff;
		buff.Initialize(PAGE_SIZE,false);
		buff.Write(rstr, result);
		CMakeNewInfo::MakeNewsInfo(buff, *pVec, g_mapVersion);
	}
	//--------------http解析--公告返回---------------------------------------------------------------
	const char *rstr1 = resultBulletin.c_str();
	result = resultBulletin.length();
	if(result > 4)
	{
		CBuffer buff;
		buff.Initialize(PAGE_SIZE,false);
		buff.Write(rstr1, result);
		CMakeNewInfo::MakeBulletinInfo(buff, *pVec, g_mapVersion);
	}
	//--------------http解析--研报返回---------------------------------------------------------------
	const char *rstr2 = resultReport.c_str();
	result = resultReport.length();
	if(result > 4)
	{
		CBuffer buff;
		buff.Initialize(PAGE_SIZE,false);
		buff.Write(rstr2, result);
		CMakeNewInfo::MakeReportInfo(buff, *pVec, g_mapVersion);
	}
	string content = "";
	//int size = (int)pVec->size();
	//for(int i = 0; i < size; i++)
	//{
	//	char str[102400] = {0};
	//	MarketInfoItem *item = pVec[i];
	//	sprintf_s(str, 102399, "%d,%d,%d,%s,%s", item->date, item->time, item->infoType, item->infoCode.c_str(), item->title.c_str());
	//	content += str;
	//}
	std::vector<MarketInfoItem*>::iterator it;
	char str[102400] = {0};
	for(it = pVec->begin(); it != pVec->end(); it++)
	{
		memset(str, 0, 102400);
		MarketInfoItem *item = *it;
		sprintf_s(str, 102399, "%d,%d,%d,%s,%s\n", item->date, item->time, item->infoType, item->infoCode.c_str(), item->title.c_str());
		std::string tmp = str;
		WriteFile("D:\\1.txt", (char*)tmp.c_str(), tmp.length());
		content += str;
	}
	return content;
}