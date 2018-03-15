#include "stdafx.h"
#include "SecurityService.h"
#include "Str.h"
#include "CFile.h"

SecurityService::SecurityService()
{
}

SecurityService::~SecurityService()
{
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityService::AddSecurity(Security *security)
{
	m_securitiesMap[security->m_innerCode] = *security;
	m_securitiesMap2[security->m_code] = *security;
}

int SecurityService::GetSecurity(int innerCode, Security *security)
{
	int state = 0;
	map<int, Security>::iterator sIter2 = m_securitiesMap.find(innerCode);
	if(sIter2 != m_securitiesMap.end())
	{
		*security = sIter2->second;
		state = 1;
	}
	return state;
}

int SecurityService::GetSecurity(String code, Security *security)
{
	int state = 0;
	map<String, Security>::iterator sIter2 = m_securitiesMap2.find(code);
	if(sIter2 != m_securitiesMap2.end())
	{
		*security = sIter2->second;
		state = 1;
	}
	return state;
}