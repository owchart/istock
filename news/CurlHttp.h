/*****************************************************************************\
*                                                                             *
* CurlHttp.h -    Curl Http class, types, and definitions                     *
*                                                                             *
*               Version 1.00 бя                                               *
*                                                                             *
*               Copyright (c) 2010-2014, Todd's OwChart. All rights reserved. *
*               Check 2016/9/15 by QiChunyou.   Modified by Wang Shaoxu       *
*                                                                             *
******************************************************************************/

#ifndef __CURLHTTP__H__
#define __CURLHTTP__H__
#pragma once
#include "stdafx.h"
#include "windef.h"
#include "curl/curl.h"
#include "curl/easy.h"
#include <Winsock2.h>
#include <Winldap.h>
#include <iostream>
#include "Winhttp.h"
#include <string>
using namespace std;

#pragma comment(lib,"Ws2_32.lib")
#pragma comment(lib,"Wldap32.lib")
#pragma comment(lib,"Winhttp.lib")
#pragma comment(lib, "Advapi32.lib")

#define BUFFERLEN 1024

namespace OwLib
{
	typedef int (*progressFun)(void* ptr, double rDlTotal, double rDlNow, double rUlTotal, double rUlNow) ;

	struct FtpFile 
	{
		const char *m_filename; 
		FILE *m_stream; 
	}; 

	struct CurlProxyInfo
	{
		CurlProxyInfo()
		{
			m_proxyType = CURLPROXY_HTTP;
		}
		void clear()
		{
			m_proxyType = CURLPROXY_HTTP;
			m_address.clear();
			m_port.clear();
			m_account.clear();
			m_pwd.clear();
			m_domain.clear();
		}
		curl_proxytype m_proxyType;
		string m_address;
		string m_port;
		string m_account;
		string m_pwd;
		string m_domain;
	};

	struct ProgressInfo 
	{
		progressFun m_fun;
		void *m_ptr;
	};

	class CurlHttp
	{
	private:
		HWND m_hWnd;
		CURLcode m_lastErrorCode; 
		CurlProxyInfo *m_proxyInfo;
	private:
		CLock m_Lock;
	private:
		static bool CopyFileBite(FILE *src, long start, long size, FILE *dest);
		static size_t FRead(void *buffer, size_t size, size_t nmemb, void *user_p);
		static int FWrite(void *buffer, size_t size, size_t nmemb, void *stream);
		void GetIeProxySet(long *type, char *server) const;
		static string ReadFromFile(const string& fileName, int& outLen);
		void SetCurlProxy(CURL *curl);
		bool TestProxy(CurlProxyInfo *proxyInfo, const string& url);
		int UploadFile(const string& fileName, const string& url) ;
		static int Write(char *data, size_t size, size_t nmemb, string *writerData);   
	public:
		CurlHttp(HWND callhwnd);
		virtual ~CurlHttp();
		static CurlHttp* s_pHttp;
		static CurlHttp* GetInstance(HWND m_hWnd = 0);
		static void ReleaseInstance();
		static void SetCallHwnd(HWND m_hWnd);
	public:
		string DownloadFile(string strFileUrl, string strFilePath, ProgressInfo *proParam = 0, int timeout = 600);
		string Get(const string& url, bool isRan, int timeout = 10);
		void GetIeProxySet(string *ip, int *port);
		CURLcode GetLastErrorCode() const;
		string GetIp();
		string ParseURL2Ip(const string& url);
		string Post(const string& url,void *content, long contentsize, int timeout = 10);
		string UploadBitFile(string strUploadUrl, string strUploadFileName, void *ptr);
	};
}
#endif