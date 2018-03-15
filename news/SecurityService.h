/***************************************************************************\
*                                                                           *
* SecurityService.h -  Security service functions, types, and definitions   *
*                                                                           *
*               Version 1.00 ★                                             *
*                                                                           *
*               Copyright (c) 2016-2016, Server. All rights reserved.       *
*               Created by Todd.                                            *
*                                                                           *
****************************************************************************/

#ifndef __SECURITYSERVICE_H__
#define __SECURITYSERVICE_H__
#pragma once
#include "stdafx.h"
#include "Security.h"

//证券服务
class SecurityService
{
public:
	map<int, Security> m_securitiesMap;
	map<String, Security> m_securitiesMap2;
public:
	SecurityService();
	virtual ~SecurityService();
public:
	void AddSecurity(Security *security); 
	//根据内码获取证券代码
	int GetSecurity(int innerCode, Security *security); 
	//根据代码获取证券代码
	int GetSecurity(String code, Security *security); 
};

#endif