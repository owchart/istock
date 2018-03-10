/*****************************************************************************\
*                                                                             *
* LoginService.h -  Login service functions, types, and definitions           *
*                                                                             *
*               Version 1.00 ★                                               *
*                                                                             *
*               Copyright (c) 2016-2016, Server. All rights reserved.         *
*               Created by Todd.                                              *
*                                                                             *
*******************************************************************************/

#ifndef __DATACENTER_H__
#define __DATACENTER_H__
#pragma once
#include "CFile.h"
#include "NewsService.h"
#include "SecurityService.h"
class NewsService;
class SecurityService;

//数据中心
class DataCenter
{
public:
	//获取新闻服务
	static NewsService* GetNewsService();
	//获取证券服务
	static SecurityService* GetSecurityService();
public:
	//启动服务
	static void StartService();
};

#endif