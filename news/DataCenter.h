/*****************************************************************************\
*                                                                             *
* LoginService.h -  Login service functions, types, and definitions           *
*                                                                             *
*               Version 1.00 ��                                               *
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

//��������
class DataCenter
{
public:
	//��ȡ���ŷ���
	static NewsService* GetNewsService();
	//��ȡ֤ȯ����
	static SecurityService* GetSecurityService();
public:
	//��������
	static void StartService();
};

#endif