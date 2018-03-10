/*****************************************************************************\
*                                                                             *
* NewService.h - News functions, types, and definitions.                      *
*                                                                             *
*               Version 1.00 бя                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's news. All rights reserved.    *
*                                                                             *
******************************************************************************/

#ifndef __NEWSSERVICE_H__
#define __NEWSSERVICE_H__
#pragma once
#include "stdafx.h"
#include "DataCenter.h"
#include "SecurityService.h"

class NewsService
{
public:
	NewsService();
	virtual ~NewsService();
	vector<string> m_userids;
public:
	string GetRandomUserID();
	string GetNews(string code);
};

#endif