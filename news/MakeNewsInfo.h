/*****************************************************************************\
*                                                                             *
* MakeNewsInfo.h - Make news info functions, types, and definitions.          *
*                                                                             *
*               Version 1.00 ��                                               *
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
	int NewsVer;         //���Ű汾
	int BulletinVer;     //����汾
	int ReportVer;       //�б��汾
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
	*   						//---����-----   //-----����---   //------�б�---   //------����---
	*   int secuVarietyCode;    //֤ȯ����   |	 //֤ȯ����	  |	  //֤ȯ����	|   //֤ȯ����    |
	*   std::string infoCode;	//��Ѷ����	 |	 //��Ѷ����   |	  //��Ѷ����	|   //��Ѷ����    |
	*   std::string title;		//����		 |	 //����	      |	  //����		|   //����		  |
	*   int date;				//����	     |	 //����	      |	  //����		|   //����		  |
	*   int time;				//ʱ��	     |	 //ʱ��-------|	  //ʱ��		|   //ʱ��		  |
	*   std::string mediaName;	//��Դ-------|					  //------------|   //��Դ------- |
	*   std::string insSName;									  //��������	|
	*   int insStar;											  //�Ǽ�		|
	*   int emRatingValue;										  //����--------|
	*   
	*   std::string stockName;  //��Ʊ����
	*   ULONG64 dateTime;       //����ʱ����ɵ�64λ������������
	*   int infoType;           //��Ѷ���� 1������  2������ 3���б�
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

//������Ѷ�������󼰽���
class CMakeNewInfo
{
public:
	//��ȡ������url
	static string GetInfoHttpUrl();

	//http--��������--(ʹ�ú�ǵ� delete [], �����ڴ�й¶)
	static char* MakeNewReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--��������--(ʹ�ú�ǵ� delete [], �����ڴ�й¶)
	static char* MakeBulletinReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--�б�����--(ʹ�ú�ǵ� delete [], �����ڴ�й¶)
	static char* MakeReportReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0);

	//http--��������--(ʹ�ú�ǵ� delete [], �����ڴ�й¶)
	static char* MakeRemindReq(short count, vector<int> &goods, map<int, StockVersionInfo> &mm, int &len, int startDate=0, int endDate=0);

	//http�������Ž���
	static void MakeNewsInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http���ع������
	static void MakeBulletinInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http�����б�����
	static void MakeReportInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);

	//http�������ѽ���
	static void MakeRemindInfo(CBuffer &buff, MarketInfoItemPVector &out, map<int, StockVersionInfo> &mm);
};

#endif