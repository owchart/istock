/*****************************************************************************\
*                                                                             *
* Str.h -      String functions                                               *
*                                                                             *
*               Version 4.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

#ifndef __STR_H__
#define __STR_H__
#pragma once
#include "stdafx.h"
#include "objbase.h"
#include "CodeConvert_Win.h"
#pragma comment(lib,"ole32.lib") 

class Str
{
private:
	//��������
	static double round(double x); 
public:
	//�����ַ���
	static void Contact(wchar_t *str, LPCTSTR str1, LPCTSTR str2 = L"", LPCTSTR str3 = L"");
	//ת��Ϊǧ��λ
	static String ConvertThousands(double value, int digit);
	//���ݿ����תΪ�ļ���
	static string ConvertDBCodeToFileName(const string& code);
	//���ݿ����תΪ���˴���
	static string ConvertDBCodeToSinaCode(const string& code);
	//���ݿ����תΪ��Ѷ����
	static string ConvertDBCodeToTencentCode(const string& code);
	//���˴���תΪ���ݿ����
	static String ConvertSinaCodeToDBCode(const String& code);
	//��Ѷ����תΪ���ݿ����
	static String ConvertTencentCodeToDBCode(const String& code);
	//��ȡ���ݿ��ַ���
	static String GetDBString(const String& strSrc);
	//��ȡΨһ��ʶ
	static string GetGuid();
	//��ȡ��ʽ������
	static void GetFormatDate(double date, wchar_t *str);
	//��ȡ����·��
	static string GetProgramDir();
	//��ȡ�ַ����Ŀռ�
	static int GetStringCapacity(const string& str);
	//��ȡ���ַ����Ŀռ�
	static int GetWStringCapacity(const String& str);
	//��ȡ����ָ��С��λ�����ַ���
	static void GetValueByDigit(double value, int digit, wchar_t *str);
	//��ȡ������ֵ
	static double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);
	//ͨ��������ֵ��ȡ������ʱ����
	static void M130(double num, int *tm_year, int *tm_mon, int *tm_mday, int *tm_hour, int *tm_min, int *tm_sec, int *tm_msec);
	//�滻�ַ���
	static string Replace(const string& str, const string& src, const string& dest);
	//�滻���ַ���
	static String Replace(const String& str, const String& src, const String& dest);
	//�ָ��ַ���
	static vector<string> Split(const string& str, const string& pattern);
	//�ָ���ַ���
	static vector<String> Split(const String& str, const String& pattern);
	//ת��Ϊ���ַ���
	static void stringTowstring(String &strDest, const string& strSrc);
	//ת��ΪСд
	static String ToLower(const String& str);
	//ת��Ϊ��д
	static String ToUpper(const String& str);
	//ת��Ϊխ�ַ���
	static void wstringTostring(string &strDest, const String& strSrc);
public:
		// ��ȡ����ʱ��
	static CString GetCurTime(CString format = _T("%Y-%m-%d %H:%M:%S") );
	static CString GetCurData(CString format = _T("%Y-%m-%d") );
		// Unicode to Ansc  (char -> char)
	static void UnicodeToANSC(std::string& out, const char* inSrc);

	// Ansc to Unicode  (char -> char)
	static void ANSCToUnicode(std::string& out, const char* inSrc);
};
#endif