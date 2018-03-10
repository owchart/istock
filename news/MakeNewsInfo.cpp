#include "stdafx.h"
#include "MakeNewsInfo.h"

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

string CMakeNewInfo::GetInfoHttpUrl()
{
	return "";
}

char* CMakeNewInfo::MakeNewReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate/*=0*/)
{
	std::string userID = DataCenter::GetNewsService()->GetRandomUserID();
	short sLen = userID.length();

	CString strDate = Str::GetCurData(_T("%Y%m%d"));
	int startTime(0), endTime(0/*235959*/), endDate = 0;//_ttoi(strDate);

	short codeSize = goods.size();
	int pos(0), totalLen = sLen + codeSize*8 + 24;//1 + 2+idLen + 2+codeSize*8 + 2 + 4*4 + 1
	len = totalLen;

	/*--------------新闻请求字符-----------------------------------------------------------
		*Request	请求19	byte   (公告 请求21  研报请求 23)
		*cid	    用户id	string
		*codeCount	证券内码+版本号数组的长度	short
		*secuVarietyCodes+version	证券内码+版本号数组	int+int数组
		*Count	    记录数	short
		*startDate	时间戳，开始日期 	int
		*startTime	时间戳，开始时间 	int
		*endDate	时间戳，结束日期 	int
		*endTime	时间戳，结束时间 	int
		*flag	    客户端标识	byte
	*/
	//-------------------------------------------------------------------


	char *pReq = new char[totalLen+1];
	memset(pReq, 0, totalLen+1);
	memset(pReq, 19, 1);
	memcpy(pReq+1, &sLen, 2);
	memcpy(pReq+3, userID.c_str(), sLen);
	pos = 3 + sLen;

	memcpy(pReq+pos, &codeSize, 2);
	pos += 2;
	vector<int>::iterator it = goods.begin();
	for(;it!= goods.end();++it)
	{
		int code = (*it);
		int version = 0;
		////查找版本号
		//auto iter = mm.find(code);
		//if(iter != mm.end())
		//	version = iter->second.NewsVer;
		memcpy(pReq+pos, &code, 4);
		memcpy(pReq+pos+4, &version, 4);
		pos += 8;
	}

	memcpy(pReq+pos, &count, 2);
	pos += 2;
	memcpy(pReq+pos, &startDate, 4);
	pos += 4;
	memcpy(pReq+pos, &startTime, 4);
	pos += 4;
	memcpy(pReq+pos, &endDate, 4);
	pos += 4;
	memcpy(pReq+pos, &endTime, 4);
	pos += 4;
	memset(pReq+pos, 123, 1);
	pos++;
	ASSERT(pos == totalLen);

	return pReq;
}

char* CMakeNewInfo::MakeBulletinReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate/*=0*/)
{
	std::string userID = DataCenter::GetNewsService()->GetRandomUserID();
	short sLen = userID.length();

	CString strDate = Str::GetCurData(_T("%Y%m%d"));
	int startTime(0), endTime(0)/*(235959)*/, endDate = 0;//_ttoi(strDate);

	short codeSize = goods.size();
	int pos(0), totalLen = sLen + codeSize*8 + 24;//1 + 2+idLen + 2+codeSize*8 + 2 + 4*4 + 1
	len = totalLen;

	char *pReq = new char[totalLen+1];
	memset(pReq, 0, totalLen+1);
	memset(pReq, 21, 1);
	memcpy(pReq+1, &sLen, 2);
	memcpy(pReq+3, userID.c_str(), sLen);
	pos = 3 + sLen;

	memcpy(pReq+pos, &codeSize, 2);
	pos += 2;
	vector<int>::iterator it = goods.begin();
	for(;it!= goods.end();++it)
	{
		int code = (*it);
		int version = 0;
		////查找版本号
		//auto iter = mm.find(code);
		//if(iter != mm.end())
		//	version = iter->second.BulletinVer;
		memcpy(pReq+pos, &code, 4);
		memcpy(pReq+pos+4, &version, 4);
		pos += 8;
	}

	memcpy(pReq+pos, &count, 2);
	pos += 2;
	memcpy(pReq+pos, &startDate, 4);
	pos += 4;
	memcpy(pReq+pos, &startTime, 4);
	pos += 4;
	memcpy(pReq+pos, &endDate, 4);
	pos += 4;
	memcpy(pReq+pos, &endTime, 4);
	pos += 4;
	memset(pReq+pos, 123, 1);
	pos++;
	ASSERT(pos == totalLen);

	return pReq;
}

char* CMakeNewInfo::MakeReportReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate/*=0*/)
{
	std::string userID = DataCenter::GetNewsService()->GetRandomUserID();
	short sLen = userID.length();

	CString strDate = Str::GetCurData(_T("%Y%m%d"));
	int startTime(0), endTime(0)/*(235959)*/, endDate = 0;//_ttoi(strDate);

	short codeSize = goods.size();
	int pos(0), totalLen = sLen + codeSize*8 + 24;//1 + 2+idLen + 2+codeSize*8 + 2 + 4*4 + 1
	len = totalLen;

	char *pReq = new char[totalLen+1];
	memset(pReq, 0, totalLen+1);
	memset(pReq, 23, 1);
	memcpy(pReq+1, &sLen, 2);
	memcpy(pReq+3, userID.c_str(), sLen);
	pos = 3 + sLen;

	memcpy(pReq+pos, &codeSize, 2);
	pos += 2;
	
	vector<int>::iterator it = goods.begin();
	for(;it!= goods.end();++it)
	{
		int code = (*it);
		int version = 0;
		////查找版本号
		//auto iter = mm.find(code);
		//if(iter != mm.end())
		//	version = iter->second.ReportVer;
		memcpy(pReq+pos, &code, 4);
		memcpy(pReq+pos+4, &version, 4);
		pos += 8;
	}

	memcpy(pReq+pos, &count, 2);
	pos += 2;
	memcpy(pReq+pos, &startDate, 4);
	pos += 4;
	memcpy(pReq+pos, &startTime, 4);
	pos += 4;
	memcpy(pReq+pos, &endDate, 4);
	pos += 4;
	memcpy(pReq+pos, &endTime, 4);
	pos += 4;
	memset(pReq+pos, 123, 1);
	pos++;
	ASSERT(pos == totalLen);

	return pReq;
}

char* CMakeNewInfo::MakeRemindReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate/*=0*/,int endDate/*=0*/)
{
	ASSERT(startDate > 10000000 || startDate == 0);
	std::string userID = DataCenter::GetNewsService()->GetRandomUserID();
	short sLen = userID.length();

	int startTime(0), endTime(0);
	if(endDate != 0)
		endTime = 235959;

	short codeSize = goods.size();
	int pos(0), totalLen = sLen + codeSize*8 + 24;//1 + 2+idLen + 2+codeSize*8 + 2 + 4*4 + 1
	len = totalLen;

	//--------------提醒请求字符-----------------------------------------------------------
	//Request	请求25	byte
	char *pReq = new char[totalLen+1];
	memset(pReq, 0, totalLen+1);
	memset(pReq, 25, 1);
	memcpy(pReq+1, &sLen, 2);
	memcpy(pReq+3, userID.c_str(), sLen);
	pos = 3 + sLen;

	memcpy(pReq+pos, &codeSize, 2);
	pos += 2;

	vector<int>::iterator it = goods.begin();
	for(;it!= goods.end();++it)
	{
		int code = (*it);
		int version = 0;
		////查找版本号
		//auto iter = mm.find(code);
		//if(iter != mm.end())
		//	version = iter->second.RemindVer;
		memcpy(pReq+pos, &code, 4);
		memcpy(pReq+pos+4, &version, 4);
		pos += 8;
	}

	memcpy(pReq+pos, &count, 2);
	pos += 2;
	memcpy(pReq+pos, &startDate, 4);
	pos += 4;
	memcpy(pReq+pos, &startTime, 4);
	pos += 4;
	memcpy(pReq+pos, &endDate, 4);
	pos += 4;
	memcpy(pReq+pos, &endTime, 4);
	pos += 4;
	memset(pReq+pos, 123, 1);
	pos++;
	ASSERT(pos == totalLen);

	return pReq;
}

void CMakeNewInfo::MakeNewsInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm)
{
	try
	{
		int rSVCode(0), oldSVCode(0);  //内码
		int rVersion(0);    //最新版本号

		char id = buff.ReadChar();
		if(id != 19)
			return;
		short rCount = buff.ReadShort();//记录数
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 1; //资讯类型
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//内码
			pItem->makeStockName();//股票名称

			rVersion  = buff.ReadInt();//最新版本号
// 			if(oldSVCode != rSVCode) //回写版本号
// 			{
// 				oldSVCode = rSVCode;
// 				std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
// 				if(iter != mm.end())
// 					iter->second.NewsVer = rVersion;
// 			}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//资讯编码
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//标题

			pItem->date = buff.ReadInt();//日期
			pItem->time = buff.ReadInt();//时间
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sMedia);
			Str::UnicodeToANSC(pItem->mediaName, sMedia.c_str());//来源

			out.push_back(pItem);
		}
	}
	catch (...)
	{
		ASSERT(0);
	}
}

void CMakeNewInfo::MakeBulletinInfo( CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm)
{
	try
	{
		int rSVCode(0), oldSVCode(0);  //内码
		int rVersion(0);    //最新版本号

		char id = buff.ReadChar();
		if(id != 21)
			return;
		short rCount = buff.ReadShort();//记录数
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 2; //资讯类型
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//内码

			pItem->makeStockName();//股票名称

			rVersion  = buff.ReadInt();//最新版本号
			//if(oldSVCode != rSVCode) //回写版本号
			//{
			//	oldSVCode = rSVCode;
			//	std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			//	if(iter != mm.end())
			//		iter->second.BulletinVer = rVersion;
			//}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//资讯编码
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//标题

			pItem->date = buff.ReadInt();//日期
			pItem->time = buff.ReadInt();//时间
			pItem->makeLong64();
			pItem->makeStrDateTime();

			out.push_back(pItem);
		}
	}
	catch (...)
	{
		ASSERT(0);
	}
}

void CMakeNewInfo::MakeReportInfo( CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm)
{
	try
	{
		int rSVCode(0), oldSVCode(0);  //内码
		int rVersion(0);    //最新版本号

		char id = buff.ReadChar();
		if(id != 23)
			return;
		short rCount = buff.ReadShort();//记录数
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sInsName;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 3; //资讯类型
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//内码

			pItem->makeStockName();//股票名称

			rVersion  = buff.ReadInt();//最新版本号
			//if(oldSVCode != rSVCode) //回写版本号
			//{
			//	oldSVCode = rSVCode;
			//	std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			//	if(iter != mm.end())
			//		iter->second.ReportVer = rVersion;
			//}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//资讯编码
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//标题

			pItem->date = buff.ReadInt();//日期
			pItem->time = buff.ReadInt();//时间
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sInsName);
			Str::UnicodeToANSC(pItem->insSName, sInsName.c_str());//机构名称

			pItem->insStar = buff.ReadChar();			//星级
			pItem->emRatingValue = buff.ReadChar();		//评级

			out.push_back(pItem);
		}
	}
	catch (...)
	{
		ASSERT(0);
	}
}

void CMakeNewInfo::MakeRemindInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm)
{
	try
	{
		int rSVCode(0), oldSVCode(0);  //内码
		int rVersion(0);    //最新版本号

		char id = buff.ReadChar();
		if(id != 25)
			return;
		short rCount = buff.ReadShort();//记录数
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 4; //资讯类型
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//内码
			pItem->makeStockName();//股票名称

			rVersion  = buff.ReadInt();//最新版本号
			// 			if(oldSVCode != rSVCode) //回写版本号
			// 			{
			// 				oldSVCode = rSVCode;
			// 				std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			// 				if(iter != mm.end())
			// 					iter->second.RemindVer = rVersion;
			// 			}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//资讯编码
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//标题

			pItem->date = buff.ReadInt();//日期
			pItem->time = buff.ReadInt();//时间
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sMedia);
			Str::UnicodeToANSC(pItem->mediaName, sMedia.c_str());//来源

			out.push_back(pItem);
		}
	}
	catch (...)
	{
		ASSERT(0);
	}
}

MarketInfoItem::MarketInfoItem()
{
	infoType = 0;
	secuVarietyCode = 0;
	date = 0;
	time = 0;
	insStar = 0;
	emRatingValue = 0;
	dateTime = 0;
}

void MarketInfoItem::makeLong64()
{
	dateTime = ((ULONG64)(((UINT)(((DWORD_PTR)(time))/* & 0xffffffff*/)) | ((ULONG64)((UINT)(((DWORD_PTR)(date))/* & 0xffffffff*/))) << 32));
}

void MarketInfoItem::makeStockName()
{
	Security security;
	if(DataCenter::GetSecurityService()->GetSecurity(secuVarietyCode, &security))
	{
		Str::wstringTostring(stockName, security.m_name);
	}
}

void MarketInfoItem::makeStrDateTime()
{
	CString str,str1;
	str.Format(_T("%08d"), date);
	str.Insert(6,'-');
	str.Insert(4,'-');
	str1 = str + _T(" ");

	str.Format(_T("%06d"), time);
	str.Insert(4,':');
	str.Insert(2,':');
	str1 += str;
	Str::wstringTostring(sDateTime, str1.GetString());
}