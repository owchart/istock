#include "stdafx.h"
#include "DataCenter.h"
#include "Str.h"

NewsService *m_newsService = 0;
SecurityService *m_securityService = 0;

NewsService* DataCenter::GetNewsService()
{
	return m_newsService;
}

SecurityService* DataCenter::GetSecurityService()
{
	return m_securityService;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void DataCenter::StartService()
{
	if(!m_newsService)
	{
		m_newsService = new NewsService;
	}
	if(!m_securityService)
	{
		m_securityService = new SecurityService;
	}
}