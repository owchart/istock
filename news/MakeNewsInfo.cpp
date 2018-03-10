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

	/*--------------���������ַ�-----------------------------------------------------------
		*Request	����19	byte   (���� ����21  �б����� 23)
		*cid	    �û�id	string
		*codeCount	֤ȯ����+�汾������ĳ���	short
		*secuVarietyCodes+version	֤ȯ����+�汾������	int+int����
		*Count	    ��¼��	short
		*startDate	ʱ�������ʼ���� 	int
		*startTime	ʱ�������ʼʱ�� 	int
		*endDate	ʱ������������� 	int
		*endTime	ʱ���������ʱ�� 	int
		*flag	    �ͻ��˱�ʶ	byte
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
		////���Ұ汾��
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
		////���Ұ汾��
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
		////���Ұ汾��
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

	//--------------���������ַ�-----------------------------------------------------------
	//Request	����25	byte
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
		////���Ұ汾��
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
		int rSVCode(0), oldSVCode(0);  //����
		int rVersion(0);    //���°汾��

		char id = buff.ReadChar();
		if(id != 19)
			return;
		short rCount = buff.ReadShort();//��¼��
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 1; //��Ѷ����
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//����
			pItem->makeStockName();//��Ʊ����

			rVersion  = buff.ReadInt();//���°汾��
// 			if(oldSVCode != rSVCode) //��д�汾��
// 			{
// 				oldSVCode = rSVCode;
// 				std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
// 				if(iter != mm.end())
// 					iter->second.NewsVer = rVersion;
// 			}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//��Ѷ����
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//����

			pItem->date = buff.ReadInt();//����
			pItem->time = buff.ReadInt();//ʱ��
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sMedia);
			Str::UnicodeToANSC(pItem->mediaName, sMedia.c_str());//��Դ

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
		int rSVCode(0), oldSVCode(0);  //����
		int rVersion(0);    //���°汾��

		char id = buff.ReadChar();
		if(id != 21)
			return;
		short rCount = buff.ReadShort();//��¼��
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 2; //��Ѷ����
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//����

			pItem->makeStockName();//��Ʊ����

			rVersion  = buff.ReadInt();//���°汾��
			//if(oldSVCode != rSVCode) //��д�汾��
			//{
			//	oldSVCode = rSVCode;
			//	std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			//	if(iter != mm.end())
			//		iter->second.BulletinVer = rVersion;
			//}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//��Ѷ����
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//����

			pItem->date = buff.ReadInt();//����
			pItem->time = buff.ReadInt();//ʱ��
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
		int rSVCode(0), oldSVCode(0);  //����
		int rVersion(0);    //���°汾��

		char id = buff.ReadChar();
		if(id != 23)
			return;
		short rCount = buff.ReadShort();//��¼��
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sInsName;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 3; //��Ѷ����
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//����

			pItem->makeStockName();//��Ʊ����

			rVersion  = buff.ReadInt();//���°汾��
			//if(oldSVCode != rSVCode) //��д�汾��
			//{
			//	oldSVCode = rSVCode;
			//	std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			//	if(iter != mm.end())
			//		iter->second.ReportVer = rVersion;
			//}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//��Ѷ����
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//����

			pItem->date = buff.ReadInt();//����
			pItem->time = buff.ReadInt();//ʱ��
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sInsName);
			Str::UnicodeToANSC(pItem->insSName, sInsName.c_str());//��������

			pItem->insStar = buff.ReadChar();			//�Ǽ�
			pItem->emRatingValue = buff.ReadChar();		//����

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
		int rSVCode(0), oldSVCode(0);  //����
		int rVersion(0);    //���°汾��

		char id = buff.ReadChar();
		if(id != 25)
			return;
		short rCount = buff.ReadShort();//��¼��
		char flag = buff.ReadChar();
		if(flag != 123)
			return;

		string sInfoCode, sTitle, sMedia;
		for (short i = 0; i < rCount; i++)
		{
			MarketInfoItem* pItem = new MarketInfoItem;
			pItem->infoType = 4; //��Ѷ����
			rSVCode = buff.ReadInt();
			pItem->secuVarietyCode = rSVCode;//����
			pItem->makeStockName();//��Ʊ����

			rVersion  = buff.ReadInt();//���°汾��
			// 			if(oldSVCode != rSVCode) //��д�汾��
			// 			{
			// 				oldSVCode = rSVCode;
			// 				std::map<int, StockVersionInfo>::iterator iter = mm.find(rSVCode);
			// 				if(iter != mm.end())
			// 					iter->second.RemindVer = rVersion;
			// 			}

			buff.ReadShortString(sInfoCode);
			Str::UnicodeToANSC(pItem->infoCode, sInfoCode.c_str());	//��Ѷ����
			buff.ReadShortString(sTitle);
			Str::UnicodeToANSC(pItem->title, sTitle.c_str());		//����

			pItem->date = buff.ReadInt();//����
			pItem->time = buff.ReadInt();//ʱ��
			pItem->makeLong64();
			pItem->makeStrDateTime();

			buff.ReadShortString(sMedia);
			Str::UnicodeToANSC(pItem->mediaName, sMedia.c_str());//��Դ

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