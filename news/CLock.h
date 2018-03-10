/*****************************************************************************\
*                                                                             *
* CLock.h -     Lock functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00   ¡ï¡ï¡ï¡ï¡ï                                     *
*                                                                             *
*               Copyright (c) 2015-2016, Update. All rights reserved.         *
*               Created by Wang Shaoxu 2015/12/01.                            *
*                                                                             *
******************************************************************************/
#ifndef __CLOCK__H__
#define __CLOCK__H__
#pragma once

class CLock
{
public:
	CLock();
	~CLock();

	void Lock();
	void UnLock();
private:
	CRITICAL_SECTION m_cs;
};

#endif
