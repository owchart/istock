#include "stdafx.h"
#include "CurlHttp.h"
#define READ_BUFF_SIZE  2048

namespace OwLib
{
	bool CurlHttp::CopyFileBite(FILE *src, long start, long size, FILE *dest)
	{
		if (fseek(src, start, SEEK_SET))
		{
			return false;
		}
		char cBuff[READ_BUFF_SIZE] = {0};
		long readedSize = 0;
		size_t stRead = 0, stWrite = 0;
		int toreadlen = 0;
		while (1)
		{
			toreadlen = size - readedSize;
			stRead = fread(cBuff, sizeof(char), (toreadlen > READ_BUFF_SIZE) ? READ_BUFF_SIZE : toreadlen, src);
			readedSize = (long)(readedSize + stRead);
			stWrite = 0;
			while (1)
			{
				stWrite += fwrite(cBuff + stWrite, sizeof(char), stRead - stWrite, dest );
				if (stWrite >= stRead)
				{
					break;
				}
			}
			if (readedSize >= size)
			{
				break;
			}
		}
		return true;
	}

	size_t CurlHttp::FRead(void *buffer, size_t size, size_t nmemb, void *user_p) 
	{ 	
		FILE *fpp = (FILE*)user_p;
		if (!fpp || feof(fpp))  
		{
			return 0;
		}
		int read = (int)fread(buffer, size, nmemb, fpp);  
		return read; 
	} 

	int CurlHttp::FWrite(void *buffer, size_t size, size_t nmemb, void *stream) 
	{ 
		struct FtpFile *out = (struct FtpFile *)stream; 
		if (out && !out->m_stream) 
		{ 
			fopen_s(&out->m_stream,out->m_filename, "wb"); 
			if (!out->m_stream) 
			{ 
				return 0; 
			} 
			fseek(out->m_stream, 0, SEEK_END);
		} 
		int ret = (int)fwrite(buffer, size, nmemb, out->m_stream); 
		return ret;
	} 

	void CurlHttp::GetIeProxySet(long *type, char *server) const
	{
		HKEY hKey;
		char subKey[MAX_PATH] = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
		char buff[MAX_PATH] = {0};
		LONG nRet = RegOpenKeyA(HKEY_CURRENT_USER, subKey, &hKey);
		if(nRet != ERROR_SUCCESS)
		{
			return;
		}
		unsigned long regType = REG_DWORD;
		unsigned long len = MAX_PATH;
		DWORD enable = 0;
		nRet = RegQueryValueExA(hKey, "ProxyEnable", 0, &regType, (LPBYTE)&enable, &len);
		if(nRet != ERROR_SUCCESS)
		{
			RegCloseKey(hKey);
			return;
		}	
		if (enable == 0)
		{
			return;
		}
		regType = REG_SZ;
		len = MAX_PATH;
		nRet = RegQueryValueExA(hKey, "ProxyServer", 0, &regType, (unsigned char*)&buff[0], &len);
		if(nRet != ERROR_SUCCESS)
		{
			RegCloseKey(hKey);
			return ;
		}	
		RegCloseKey(hKey);
		char * sFind = 0;
		sFind = strstr(buff, ";");
		if (!sFind)
		{
			strcpy_s(server, sizeof(buff), buff);
			*type = 1;
		}
		else
		{
			sFind = strstr(buff, "http=");
			if (!sFind)
			{
				return;
			}
			int len1 = 0, len2 = 0;
			len1 = (int)(sFind - buff);
			char * sFind2 = 0;
			sFind2 = strstr(buff + len1, ";");
			len2 = (int)(sFind2 - buff);
			memcpy(server, buff + len1 + 5, len2 - len1 - 5); 
			server[len2 - len1 - 5] = 0;
			*type = 1;
		}
		return;
	}

	string CurlHttp::ReadFromFile(const string& fileName, int& outLen )
	{
		FILE* file = 0;
		string ret = "";
		fopen_s(&file, fileName.c_str(), "rb");
		if (!file)
		{
			return ret;
		}
		fseek(file, 0, SEEK_END);  
		int size = ftell(file);  
		fseek(file, 0, SEEK_SET);  
		char* data = new char[size + 1];
		ZeroMemory(data, size + 1);
		outLen = (int)fread(data, 1, size, file);
		fclose(file);
		ret = data;
		delete[]data;
		data = 0;
		return ret;
	}

	void CurlHttp::SetCurlProxy(CURL *curl)  
	{
		if(!m_proxyInfo)
		{
			m_proxyInfo = new CurlProxyInfo();
		}
		string strProxy = m_proxyInfo->m_address;
		strProxy.append(":");
		strProxy.append(m_proxyInfo->m_port);
		string strUserPwd = m_proxyInfo->m_account;
		strUserPwd.append(":");
		strUserPwd.append(m_proxyInfo->m_pwd);
		if (!(m_proxyInfo->m_address.empty()))
		{
			curl_easy_setopt(curl, CURLOPT_PROXYTYPE, m_proxyInfo->m_proxyType); 
			curl_easy_setopt(curl, CURLOPT_PROXY, strProxy.c_str());  
			if (m_proxyInfo->m_account.length()>0)
			{
				curl_easy_setopt(curl, CURLOPT_PROXYUSERPWD, strUserPwd.c_str()); 
				curl_easy_setopt(curl, CURLOPT_PROXYAUTH, CURLAUTH_ANY); 
			}
		}
	}

	bool CurlHttp::TestProxy(CurlProxyInfo *proxyInfo, const string& url)
	{
		string buffer = "";
		string retHeader = "";
		char errorBuffer[CURL_ERROR_SIZE]; 
		CURL *curl = curl_easy_init();
		if(!curl)
		{
			return false;
		}
		curl_easy_setopt(curl, CURLOPT_ERRORBUFFER, errorBuffer);
		curl_easy_setopt(curl, CURLOPT_USERAGENT, "Mozilla/5.0 (Windows; U; zh-CN) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/3.1");
		curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
		curl_easy_setopt(curl, CURLOPT_TIMEOUT, 3);
		curl_easy_setopt(curl, CURLOPT_BUFFERSIZE, BUFFERLEN);
		curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, CurlHttp::Write);
		curl_easy_setopt(curl, CURLOPT_WRITEDATA, &buffer); 
		curl_easy_setopt(curl, CURLOPT_WRITEHEADER, &retHeader); 
		string strProxy = proxyInfo->m_address;
		strProxy.append(":");
		strProxy.append(proxyInfo->m_port);
		string strUserPwd = proxyInfo->m_account;
		strUserPwd.append(":");
		strUserPwd.append(proxyInfo->m_pwd);
		if (!(proxyInfo->m_address.empty()))
		{
			curl_easy_setopt(curl, CURLOPT_PROXYTYPE, proxyInfo->m_proxyType); 
			curl_easy_setopt(curl, CURLOPT_PROXY, strProxy.c_str());  
			if (proxyInfo->m_account.length()>0)
			{
				curl_easy_setopt(curl, CURLOPT_PROXYUSERPWD, strUserPwd.c_str()); 
				curl_easy_setopt(curl, CURLOPT_PROXYAUTH, CURLAUTH_ANY); 
			}
		}
		CURLcode code = curl_easy_perform(curl);  
		if(code != CURLE_OK)
		{
			buffer = "";
		}
		curl_easy_cleanup(curl);
		bool bHas200 = false ;
		bool bHas407 = false;
		string::size_type sFist = 0;
		while (1)
		{
			string::size_type sizePos = retHeader.find("HTTP/", sFist);
			if (string::npos != sizePos)
			{
				sizePos += 9;
				string sdf = retHeader.substr(sizePos);
				int Ret = atoi(sdf.c_str());
				if (407 == Ret)
				{
					bHas407 = true;
				}
				else if (200 == Ret)
				{
					bHas200 = true;
				}
				sFist = sizePos;
			}
			else 
			{
				break;
			}
		}
		if (bHas200)
		{
			return true;
		}
		else if (bHas407)
		{
			return false;
		}	
		return true;
	}

	int CurlHttp::UploadFile(const string& fileName, const string& url) 
	{
		CURLcode code;
		FILE * fp = 0;
		fopen_s(&fp, fileName.c_str(), "rb");
		if (!fp)
		{
			return -1; 
		}
		fseek(fp, 0, SEEK_END);
		int file_size = ftell(fp); 
		rewind(fp); 
		CURL *easy_handle = 0; 
		easy_handle = curl_easy_init(); 
		if (!easy_handle) 
		{ 
			return -1;
		}
		curl_easy_setopt(easy_handle, CURLOPT_URL, url.c_str());
		curl_easy_setopt(easy_handle, CURLOPT_UPLOAD, 1L);
		curl_easy_setopt(easy_handle, CURLOPT_READFUNCTION, &FRead);
		curl_easy_setopt(easy_handle, CURLOPT_READDATA, fp); 
		curl_easy_setopt(easy_handle, CURLOPT_TIMEOUT, 30);
		curl_easy_setopt(easy_handle, CURLOPT_FTP_CREATE_MISSING_DIRS, 1);
		curl_easy_setopt(easy_handle, CURLOPT_INFILESIZE_LARGE, file_size); 
		code = curl_easy_perform(easy_handle); 
		if (code == CURLE_OK) 
		{
			int il=100;
			il++;
		}
		fclose(fp);
		curl_easy_cleanup(easy_handle);
		return 0;
	}

	int CurlHttp::Write(char *data, size_t size, size_t nmemb, string *writeData)
	{
		if (!writeData)
		{
			return 0;
		}
		int len = (int)(size * nmemb);
		writeData->append(data, len); 
		return len;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	CurlHttp::CurlHttp(HWND callhwnd)
		:m_proxyInfo(0),m_hWnd(callhwnd)
	{
		CURLcode code = curl_global_init(CURL_GLOBAL_ALL);
	}

	CurlHttp::~CurlHttp()
	{
		if (m_proxyInfo)
		{
			delete m_proxyInfo;
			m_proxyInfo = 0;
		}
		curl_global_cleanup();
	}

	CurlHttp* CurlHttp::s_pHttp = 0;

	CurlHttp* CurlHttp::GetInstance(HWND hwnd/* = 0 */)
	{
		if (!s_pHttp)
		{
			s_pHttp = new CurlHttp(hwnd);
		}
		return CurlHttp::s_pHttp;
	}

	void CurlHttp::ReleaseInstance()
	{
		if (s_pHttp)
		{
			delete s_pHttp;
			s_pHttp = 0;
		}
	}

	void CurlHttp::SetCallHwnd( HWND hWnd )
	{
		CurlHttp* phttp = GetInstance();
		if (phttp)
		{
			phttp->m_hWnd = hWnd;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	string CurlHttp::DownloadFile(string fileUrl, string filePath, ProgressInfo *progress /*= 0*/, int timeout /*= 600 */)
	{
		m_Lock.Lock();
		struct FtpFile ftpfile = 
		{
			filePath.c_str(), 0 
		}; 
		CURL *curl = curl_easy_init();
		CURLcode res = CURLE_OK; 
		if (curl) 
		{
			curl_easy_setopt(curl, CURLOPT_URL, fileUrl.c_str());
			curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, FWrite); 
			curl_easy_setopt(curl, CURLOPT_WRITEDATA, &ftpfile); 
			curl_easy_setopt(curl, CURLOPT_NOPROGRESS, FALSE); 
			curl_easy_setopt(curl, CURLOPT_PROGRESSFUNCTION, progress ? progress->m_fun : 0); 
			curl_easy_setopt(curl, CURLOPT_PROGRESSDATA, progress ? progress->m_ptr : 0); 
			curl_easy_setopt(curl, CURLOPT_TIMEOUT, timeout);
			curl_easy_setopt(curl, CURLOPT_BUFFERSIZE, BUFFERLEN);
			SetCurlProxy(curl);
			try
			{
				res = curl_easy_perform(curl); 
			}
			catch (int len)
			{
				if (CURLE_OBSOLETE16 == len)
				{
					res = CURLE_OBSOLETE16;
				}
			}
			catch (...)
			{
			}
			curl_easy_cleanup(curl);
		}
		if (ftpfile.m_stream) 
		{
			fclose(ftpfile.m_stream); 
		}
		m_lastErrorCode = res;
		if (CURLE_OK == res) 
		{
			m_Lock.UnLock();
			return filePath;
		}
		m_Lock.UnLock();
		return "";
	}

	string CurlHttp::Get(const string& url, bool isRan, int timeout)
	{
		string buffer = "";
		char errorBuffer[CURL_ERROR_SIZE]; 
		CURL *curl = curl_easy_init();
		if(!curl)
		{
			return "";
		}
		curl_easy_setopt(curl, CURLOPT_ERRORBUFFER, errorBuffer);
		curl_easy_setopt(curl, CURLOPT_USERAGENT, "Mozilla/5.0 (Windows; U; zh-CN) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/3.1");
		curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
		curl_easy_setopt(curl, CURLOPT_TIMEOUT, timeout);
		curl_easy_setopt(curl, CURLOPT_BUFFERSIZE, BUFFERLEN);
		curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, Write);
		curl_easy_setopt(curl, CURLOPT_WRITEDATA, &buffer); 		
		SetCurlProxy(curl);
		CURLcode code = curl_easy_perform(curl);  
		if(code != CURLE_OK)
		{
			buffer = "";
		}
		curl_easy_cleanup(curl);
		m_lastErrorCode = code;
		return buffer;
	}

	void CurlHttp::GetIeProxySet(string *ip, int *port)
	{
		long ifff = 1;
		char sServerTmp[256] = {0};
		GetIeProxySet(&ifff, sServerTmp);
		string address = sServerTmp;
		int pos = (int)address.find(":");
		if(pos != -1)
		{
			*ip = address.substr(0, pos);
			*port = atoi(address.substr(pos + 1).c_str());
		}
		else
		{
			*ip = address;
			*port = 80;
		}
	}

	CURLcode CurlHttp::GetLastErrorCode() const
	{
		return m_lastErrorCode;
	}

	string CurlHttp::GetIp()
	{
		char host[256] = {0};
		gethostname(host, 255);
		hostent *pHost = gethostbyname(host);
		in_addr addr;
		if (!pHost)
		{
			return "";
		}
		char *p = pHost->h_addr_list[0];
		memcpy(&addr.S_un.S_addr, p, pHost->h_length);
		char *ip = inet_ntoa(addr);
		if(ip)
		{
			return string(ip);
		}
		return "";
	}

	string CurlHttp::ParseURL2Ip(const string& url)
	{
		if (url.empty())
		{
			return "";
		}
		hostent *pHost = gethostbyname(url.c_str());
		if (pHost)
		{
			string ret = inet_ntoa(*(struct in_addr *)*pHost->h_addr_list);
			return ret.c_str();
		}
		return "";
	}

	string CurlHttp::Post(const string& url, void *content, long contentsize, int timeout /*= 10*/)
	{
		string buffer = "";
		char errorBuffer[CURL_ERROR_SIZE]; 
		CURL *curl = curl_easy_init();
		if(!curl)
		{
			return "";
		}   
		curl_easy_setopt(curl, CURLOPT_ERRORBUFFER, errorBuffer);
		curl_easy_setopt(curl, CURLOPT_USERAGENT, "Mozilla/5.0 (Windows; U; zh-CN) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/3.1");
		curl_easy_setopt(curl, CURLOPT_URL, url.c_str());
		curl_easy_setopt(curl, CURLOPT_TIMEOUT, timeout);
		curl_easy_setopt(curl, CURLOPT_BUFFERSIZE, BUFFERLEN);
		curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, Write);
		curl_easy_setopt(curl, CURLOPT_WRITEDATA, &buffer); 
		curl_easy_setopt(curl, CURLOPT_POST, 1);
		curl_easy_setopt(curl, CURLOPT_POSTFIELDS, content);    
		curl_easy_setopt(curl, CURLOPT_POSTFIELDSIZE, contentsize);
		struct curl_slist *headerlist = 0;
		headerlist = curl_slist_append(headerlist, "Content-Type: application/octet-stream");
		headerlist = curl_slist_append(headerlist, "Expect:");
		curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headerlist);
		SetCurlProxy(curl);
		CURLcode code = curl_easy_perform(curl);
		curl_slist_free_all (headerlist);
		curl_easy_cleanup(curl);
		m_lastErrorCode = code;
		return code == CURLE_OK ? buffer : "";
	} 

	string CurlHttp::UploadBitFile(string uploadUrl, string uploadFileName, void *ptr)
	{
		CURL *easy_handle  = curl_easy_init();  		
		curl_easy_setopt(easy_handle, CURLOPT_URL, uploadUrl.c_str());
		int pos = (int)uploadFileName.rfind("\\");
		string fileName = uploadFileName;
		if(pos >= 0)
		{
			fileName = uploadFileName.substr(pos+1, uploadFileName.length() - pos - 1);
		}
		int filesize = 0;
		string buffer = "";
		string retInfo = ReadFromFile(uploadFileName,filesize);
		char *data = 0;
		if (!retInfo.empty())
		{
			data = (char*)retInfo.c_str();
		}
		else
		{
			return "";
		}
		curl_slist *http_headers = 0;
		http_headers = curl_slist_append(http_headers, "Content-Type: text/xml");
		http_headers = curl_slist_append(http_headers, "Expect:");
		curl_easy_setopt(easy_handle, CURLOPT_HTTPHEADER, http_headers);
		curl_easy_setopt(easy_handle, CURLOPT_POSTFIELDS, data);
		curl_easy_setopt(easy_handle, CURLOPT_POSTFIELDSIZE, (long)filesize);
		curl_easy_setopt(easy_handle, CURLOPT_USERAGENT, "Mozilla/5.0 (Windows; U; zh-CN) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/3.1");		
		curl_easy_setopt(easy_handle, CURLOPT_WRITEFUNCTION, Write);
		curl_easy_setopt(easy_handle, CURLOPT_WRITEDATA, &buffer); 
		curl_easy_setopt(easy_handle, CURLOPT_NOPROGRESS, FALSE); 
		curl_easy_setopt(easy_handle, CURLOPT_PROGRESSFUNCTION, 0); 
		curl_easy_setopt(easy_handle, CURLOPT_PROGRESSDATA, ptr); 
		SetCurlProxy(easy_handle);
		CURLcode res;
		try
		{
			res = curl_easy_perform(easy_handle); 
		}
		catch (...)
		{
		}
		curl_slist_free_all(http_headers);
		curl_easy_cleanup(easy_handle);
		if(res == CURLE_OK)
		{
			return buffer;
		}
		return "";
	}
}