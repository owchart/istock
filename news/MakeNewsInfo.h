/*****************************************************************************\
*                                                                             *
* MakeNewsInfo.h - Make news info functions, types, and definitions.          *
*                                                                             *
*               Version 1.00 ★                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's news. All rights reserved.    *
*                                                                             *
******************************************************************************/

#ifndef __MAKENEWSINFO_H__
#define __MAKENEWSINFO_H__
#pragma once
#include "stdafx.h"
#include "Buffer.h"
#include "CodeConvert_Win.h"
#include "DataCenter.h"
#include "SecurityService.h"
#include "Security.h"

struct StockVersionInfo
{
	int NewsVer;         //新闻版本
	int BulletinVer;     //公告版本
	int ReportVer;       //研报版本
	int RemindVer;

	StockVersionInfo()
	{
		NewsVer = 0;
		BulletinVer = 0;
		ReportVer = 0;
		RemindVer = 0;
	}
};

struct MarketInfoItem
{
	/*	
	*   						//---新闻-----   //-----公告---   //------研报---   //------提醒---
	*   int secuVarietyCode;    //证券内码   |	 //证券内码	  |	  //证券内码	|   //证券内码    |
	*   std::string infoCode;	//资讯编码	 |	 //资讯编码   |	  //资讯编码	|   //资讯编码    |
	*   std::string title;		//标题		 |	 //标题	      |	  //标题		|   //标题		  |
	*   int date;				//日期	     |	 //日期	      |	  //日期		|   //日期		  |
	*   int time;				//时间	     |	 //时间-------|	  //时间		|   //时间		  |
	*   std::string mediaName;	//来源-------|					  //------------|   //来源------- |
	*   std::string insSName;									  //机构名称	|
	*   int insStar;											  //星级		|
	*   int emRatingValue;										  //评级--------|
	*   
	*   std::string stockName;  //股票名称
	*   ULONG64 dateTime;       //日期时间组成的64位数，用来排序
	*   int infoType;           //资讯类型 1：新闻  2：公告 3：研报
	*/
	int infoType;         
	int secuVarietyCode;  
	int date;				
	int time;				
	ULONG64 dateTime;     
	int insStar;			
	int emRatingValue;	
	std::string stockName;
	std::string infoCode;
	std::string title;
	std::string sDateTime;
	std::string mediaName;
	std::string insSName;

	MarketInfoItem();

	void makeLong64();

	void makeStockName();

	void makeStrDateTime();
};

typedef std::vector<MarketInfoItem*> MarketInfoItemPVector;

//新闻资讯公告请求及解析
class CMakeNewInfo
{
public:
	//获取服务器url
	static string GetInfoHttpUrl();

	//http--新闻请求--(使用后记得 delete [], 否则内存泄露)
	static char* MakeNewReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--公告请求--(使用后记得 delete [], 否则内存泄露)
	static char* MakeBulletinReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--研报请求--(使用后记得 delete [], 否则内存泄露)
	static char* MakeReportReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--提醒请求--(使用后记得 delete [], 否则内存泄露)
	static char* MakeRemindReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0, int endDate=0);

	//http返回新闻解析
	static void MakeNewsInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http返回公告解析
	static void MakeBulletinInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http返回研报解析
	static void MakeReportInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http返回提醒解析
	static void MakeRemindInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);
};

#endif