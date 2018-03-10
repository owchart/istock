#include "stdafx.h"
#include "SecurityService.h"
#include "Str.h"
#include "CFile.h"
#include "sqlite3x.hpp"
using namespace sqlite3x;

SecurityService::SecurityService()
{
	m_createTableSQL = "CREATE TABLE SECURITY(ID INTEGER PRIMARY KEY, CODE, NAME, PINGYIN, TYPE INTEGER, STATUS INTEGER, CREATETIME DATE, MODIFYTIME DATE)";
	string dataBasePath = Str::GetProgramDir() + "\\securities.db";
	Str::stringTowstring(m_dataBasePath, dataBasePath);
	if(!CFileA::IsFileExist(dataBasePath.c_str()))
	{
		CreateTable();
	}
	if(m_securitiesMap.size() == 0)
	{
		vector<Security> securities;
		GetSecurities(&securities, L"");
		vector<Security>::iterator sIter = securities.begin();
		for(; sIter != securities.end(); ++sIter)
		{
			Security security = *sIter;
			m_securitiesMap[security.m_status] = security;
			m_securitiesMap2[security.m_code] = security;
		}
		securities.clear();
	}
}

SecurityService::~SecurityService()
{
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void SecurityService::CreateTable()
{
	const wchar_t *strPath = m_dataBasePath.c_str();
	sqlite3_connection conn(strPath);
	conn.executenonquery(m_createTableSQL.c_str());
	conn.close();
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

int SecurityService::GetSecurities(vector<Security> *securities, String filter)
{
	int strLen = 100;
	wchar_t *sql = new wchar_t[strLen];
	memset(sql, 0, strLen * sizeof(wchar_t));
	_stprintf_s(sql, strLen - 1, L"%s", L"SELECT * FROM SECURITY");
	String wSql = sql;
	if(filter.length() > 0)
	{
		wSql += L" WHERE " + filter;
	}
	sqlite3_connection conn(m_dataBasePath.c_str());
	sqlite3_command cmd(conn, wSql.c_str());
	sqlite3_reader reader = cmd.executereader();
	while(reader.read())
	{
		Security security;
		security.m_code = reader.getstring16(1);
		security.m_name = reader.getstring16(2);
		security.m_pingyin = reader.getstring16(3);
		security.m_type = reader.getint(4);
		security.m_status = reader.getint(5);
		securities->push_back(security);
	}
	reader.close();
	delete[] sql;
	sql = 0;
	return 1;
}