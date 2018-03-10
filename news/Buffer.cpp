// Buffer.cpp: implementation of the CBuffer class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Buffer.h"
#include <assert.h>

void UnicodeToANSI(string &strDest, const String& strSrc )
{
	char*     pElementText;
	int    iTextLen;
	// wide char to multi char
	iTextLen = WideCharToMultiByte( CP_ACP,
		0,
		strSrc.c_str(),
		-1,
		NULL,
		0,
		NULL,
		NULL );
	pElementText = new char[iTextLen + 1];
	memset( ( void* )pElementText, 0, sizeof( char ) * ( iTextLen + 1 ) );
	::WideCharToMultiByte( CP_ACP,
		0,
		strSrc.c_str(),
		-1,
		pElementText,
		iTextLen,
		NULL,
		NULL );
	//string strText;
	strDest = pElementText;
	delete[] pElementText;
	//return strText;
}

//ANSI to Unicode
void ANSIToUnicode(String &strDest, const string& strSrc)
{
	int  len = 0;
	len = strSrc.length();
	int  unicodeLen = ::MultiByteToWideChar( CP_ACP,
		0,
		strSrc.c_str(),
		-1,
		NULL,
		0 );  
	wchar_t *  pUnicode;  
	pUnicode = new  wchar_t[unicodeLen+1];  
	memset(pUnicode,0,(unicodeLen+1)*sizeof(wchar_t));  
	::MultiByteToWideChar( CP_ACP,
		0,
		strSrc.c_str(),
		-1,
		(LPWSTR)pUnicode,
		unicodeLen );  
	//String  rt;  
	strDest = ( wchar_t* )pUnicode;
	delete  []pUnicode; 

	//return  rt;  
}

DWORD CBuffer::m_dwPageSize = 0;

CBuffer::CBuffer()
{
	m_nSize = 0;

	m_bSustainSize = false;
	m_bSingleRead = false;
	m_nReadPos = 0;

	m_pBase =  NULL;
	m_nDataSize = 0;

	m_nInitSize = 0;

	if(m_dwPageSize == 0)
	{
		SYSTEM_INFO si;
		GetSystemInfo(&si);
		m_dwPageSize = si.dwPageSize;
		while(m_dwPageSize < 8192)
			m_dwPageSize += si.dwPageSize;
	}
}

void CBuffer::Initialize(UINT nInitsize, bool bSustain)
{
	m_bSustainSize = bSustain;
	ReAllocateBuffer(nInitsize);
	m_nInitSize = m_nSize;
}


CBuffer::~CBuffer()
{
	if(m_pBase)
		delete []m_pBase;
		//VirtualFree(m_pBase, 0, MEM_RELEASE);
}
	

UINT CBuffer::GetMemSize() 
{
	return m_nSize;
}

UINT CBuffer::GetBufferLen() 
{
	if(m_pBase == NULL)
		return 0;

	if (m_bSingleRead==true)
		return m_nDataSize-m_nReadPos;
	else
		return m_nDataSize;
}

PBYTE CBuffer::GetBuffer(UINT nPos)
{
	if(m_pBase == NULL)
		return NULL;

	return m_pBase + nPos;
}
UINT CBuffer::GetBufferLen() const
{
	if(m_pBase == NULL)
		return 0;

	if (m_bSingleRead==true)
		return m_nDataSize-m_nReadPos;
	else
		return m_nDataSize;
}

PBYTE CBuffer::GetBuffer(UINT nPos)const
{
	if(m_pBase == NULL)
		return NULL;

	return m_pBase + nPos;
}
UINT CBuffer::ReAllocateBuffer(UINT nRequestedSize)
{
	if(nRequestedSize <= m_nSize)
		return 0;

	UINT nNewSize = m_nSize;
	if(nNewSize < m_dwPageSize)
		nNewSize = m_dwPageSize;

	while(nRequestedSize > nNewSize)
		nNewSize *= 2;

	assert(m_nDataSize < nNewSize);

	// New Copy Data Over
	//PBYTE pNewBuffer = (PBYTE) VirtualAlloc(NULL, nNewSize, MEM_COMMIT, PAGE_READWRITE);
	PBYTE pNewBuffer = new BYTE[nNewSize];
	if (m_pBase)
	{
		ZeroMemory(pNewBuffer,nNewSize);
		if(m_nDataSize)
			CopyMemory(pNewBuffer, m_pBase, m_nDataSize);
		//VirtualFree(m_pBase, 0, MEM_RELEASE);
		delete []m_pBase;
	}
	
	assert(pNewBuffer);
	// Hand over the pointer
	m_pBase = pNewBuffer;

	m_nSize = nNewSize;

//	TRACE("ReAllocateBuffer %d\n",m_nSize);

	return m_nSize;
}


UINT CBuffer::DeAllocateBuffer(UINT nRequestedSize)
{
	assert(m_nSize > 0);

	if(m_bSustainSize)
		return 0;

	if(m_nSize <= m_nInitSize)
		return 0;

	if(nRequestedSize < m_nDataSize)
		return 0;

	if(nRequestedSize < m_nInitSize)
		nRequestedSize = m_nInitSize;

	UINT nNewSize = m_nSize;
	while(nNewSize >= nRequestedSize * 2)
		nNewSize /= 2;

	if(nNewSize == m_nSize)
		return 0;

	assert(m_nDataSize <= nNewSize);

	//PBYTE pNewBuffer = (PBYTE) VirtualAlloc(NULL, nNewSize, MEM_COMMIT, PAGE_READWRITE);
	PBYTE pNewBuffer = new BYTE[nNewSize];
	if(m_pBase)
	{
		ZeroMemory(pNewBuffer,nNewSize);
		if(m_nDataSize)
			CopyMemory(pNewBuffer, m_pBase, m_nDataSize);

		delete [] m_pBase;
		//VirtualFree(m_pBase, 0, MEM_RELEASE);
	}

	// Hand over the pointer
	m_pBase = pNewBuffer;

	m_nSize = nNewSize;

//	TRACE("DeAllocateBuffer %d\n",m_nSize);

	return m_nSize;
}


UINT CBuffer::Write(const void* pData, UINT nSize)
{
	if(nSize)
	{
		ReAllocateBuffer(nSize + m_nDataSize);

		CopyMemory(m_pBase+m_nDataSize, pData, nSize);

		// Advance Pointer
		m_nDataSize += nSize;
	}

	return nSize;
}

UINT CBuffer::Write(String& strData)
{
	int nSize = strData.size();
	return Write((const void*) strData.c_str(), nSize);
}


UINT CBuffer::Insert(const void* pData, UINT nSize)
{
	ReAllocateBuffer(nSize + m_nDataSize);

	MoveMemory(m_pBase+nSize, m_pBase, m_nDataSize);
	CopyMemory(m_pBase, pData, nSize);

	// Advance Pointer
	m_nDataSize += nSize;

	return nSize;
}


UINT CBuffer::Insert(String& strData)
{
	int nSize = strData.size();
	return Insert((const void*) strData.c_str(), nSize);
}


UINT CBuffer::Read(void* pData, UINT nSize)
{
	// all that we have 
		
	if (nSize)
	{
		if (m_bSingleRead)
		{
			if (nSize+m_nReadPos > m_nDataSize)
			{
				throw DATA_LACK;
				return 0;
			}

			CopyMemory(pData, m_pBase+m_nReadPos, nSize);
			m_nReadPos += nSize;
		}
		else
		{
			if (nSize > m_nDataSize)
			{
				throw DATA_LACK;
				return 0;
			}

			m_nDataSize -= nSize;

			CopyMemory(pData, m_pBase, nSize);		
			if (m_nDataSize > 0)
				MoveMemory(m_pBase, m_pBase+nSize, m_nDataSize);
		}
	}
		
	DeAllocateBuffer(m_nDataSize);

	return nSize;
}

UINT CBuffer::SkipData(int nSize)
{
	if(m_bSingleRead)
	{
		if (nSize+m_nReadPos > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}

		m_nReadPos += nSize;

		return nSize;
	}
	else
	{
		m_nDataSize -= nSize;

		if (m_nDataSize > 0)
			MoveMemory(m_pBase, m_pBase+nSize, m_nDataSize);
		
		DeAllocateBuffer(m_nDataSize);
	}

	return 0;
}
 

void CBuffer::ClearBuffer()
{
	// Force the buffer to be empty
	m_nDataSize = 0;
	m_nReadPos = 0;

	DeAllocateBuffer(0);
}

void CBuffer::Copy(CBuffer& buffer)
{
	UINT nReSize = buffer.GetMemSize();

	if(nReSize != m_nSize)
	{
		if (m_pBase)
			delete []m_pBase;
			//VirtualFree(m_pBase, 0, MEM_RELEASE);
		//m_pBase = (PBYTE) VirtualAlloc(NULL, nReSize, MEM_COMMIT, PAGE_READWRITE);
		m_pBase  = new BYTE[nReSize];
		ZeroMemory(m_pBase,nReSize);
		m_nSize = nReSize;
	}
	m_nDataSize = buffer.GetBufferLen();

	if(m_nDataSize > 0)
		CopyMemory(m_pBase, buffer.GetBuffer(), m_nDataSize);
}
 

void CBuffer::FileWrite(const String& strFileName)
{
	if(m_pBase == NULL || m_nDataSize == 0)
		return;

 	CFile file;
 
 	if(file.Open(strFileName.c_str(), CFile::modeCreate | CFile::modeWrite | CFile::shareExclusive))
 	{
 		TRY
 		{
 			file.Write(m_pBase, m_nDataSize);
 		}
 		CATCH_ALL(e)
 		{
 		}
 		END_CATCH_ALL
 
 		file.Close();
 	}
}

void CBuffer::FileWrite( const string& strFileName )
{
	String szW;
	ANSIToUnicode(szW,strFileName);

	FileWrite(szW);
}

void CBuffer::FileRead(const String& strFileName)
{
	CFile file;

	if(file.Open(strFileName.c_str(), CFile::modeRead | CFile::shareDenyWrite))
	{
		char* pcFileData = NULL;

		TRY
		{
			DWORD dwLength = (DWORD)file.GetLength();
			pcFileData = new char[dwLength];
			file.Read(pcFileData, dwLength);

			Write(pcFileData, dwLength);

			delete[] pcFileData;
		}
		CATCH_ALL(e)
		{
			if(pcFileData)
				delete[] pcFileData;
		}
		END_CATCH_ALL

			file.Close();
	}
}

void CBuffer::FileRead( const string& strFileName )
{
	String szW;
	ANSIToUnicode(szW,strFileName);

	FileRead(szW);
}


UINT CBuffer::Delete(UINT nSize)
{
	if(nSize == 0)
		return nSize;

	if (nSize > m_nDataSize)
		nSize = m_nDataSize;

	m_nDataSize -= nSize;

	if(m_nDataSize > 0)
		MoveMemory(m_pBase, m_pBase+nSize, m_nDataSize);
		
	DeAllocateBuffer(m_nDataSize);

	return nSize;
}

const CBuffer& CBuffer::operator+(CBuffer& buff)
{
	this->Write(buff.GetBuffer(), buff.GetBufferLen());

	return* this;
}

UINT CBuffer::DeleteEnd(UINT nSize)
{
	if(nSize > m_nDataSize)
		nSize = m_nDataSize;
		
	if(nSize)
	{
		m_nDataSize -= nSize;
		DeAllocateBuffer(m_nDataSize);
	}
		
	return nSize;
}

void CBuffer::WriteChar(char cValue)
{
	Write(&cValue, sizeof(char));
}

char CBuffer::ReadChar()
{
	char cValue;
	Read(&cValue, sizeof(char));
	return cValue;
}

void CBuffer::WriteShort(short sValue)
{
	Write(&sValue, sizeof(short));
}

short CBuffer::ReadShort()
{
	short sValue;
	Read(&sValue, sizeof(short));
	return sValue;
}

void CBuffer::WriteInt(int nValue)
{
	Write(&nValue, sizeof(int));
}

int CBuffer::ReadInt()
{
	int nValue;
	Read(&nValue, sizeof(int));
	return nValue;
}
void CBuffer::WriteFloat( float fValue)
{
	Write(&fValue, sizeof(float));
}

float CBuffer::ReadFloat()
{
	float fValue;
	Read(&fValue, sizeof(float));
	return fValue;
}

void CBuffer::WriteDouble( double dbValue)
{
	Write(&dbValue, sizeof(double));
}

double CBuffer::ReadDouble()
{
	double fValue;
	Read(&fValue, sizeof(double));
	return fValue;
}

void CBuffer::WriteLong(INT64 hValue)
{
	LARGE_INTEGER li;
	li.QuadPart = hValue;

	WriteInt(li.HighPart);
	WriteInt(li.LowPart);
}

INT64 CBuffer::ReadLong()
{
	LARGE_INTEGER li;
	li.HighPart = ReadInt();
	li.LowPart = ReadInt();
	
	return li.QuadPart;
}

UINT CBuffer::ReadString(String& strData)
{
	string szTmp;

	UINT nRet = ReadString(szTmp);

	ANSIToUnicode(strData,szTmp);

	return nRet;
//
//	strData.clear();
//
//	DWORD dwSize;
//	Read(&dwSize, sizeof(DWORD));
//
//	if(dwSize == 0)
//		return 2;
//
//	//LPTSTR pstr = strData.GetBufferSetLength(dwSize);
//	if (m_bSingleRead)
//	{
//		if (dwSize+m_nReadPos > m_nDataSize)
//		{
//			throw DATA_LACK;
//			return 0;
//		}
//
//		//CopyMemory(pstr, m_pBase+m_nReadPos, dwSize);
//		string str((const char*)m_pBase+m_nReadPos,dwSize);
// 
//		ANSIToUnicode(strData,str);
// 
//		m_nReadPos += dwSize;
//	}
//	else
//	{
//		if (dwSize > m_nDataSize)
//		{
//			throw DATA_LACK;
//			return 0;
//		}
//		
//		//CopyMemory(pstr, m_pBase, dwSize);
//
//		string str((const char*)m_pBase,dwSize);
// 
//		ANSIToUnicode(strData,str);
// 
//		Delete(dwSize);
//	}
//
//	//strData.ReleaseBuffer();	//**
//
//	return (dwSize+sizeof(DWORD));
////#endif
}

UINT CBuffer::WriteString(String& strData)
{
 
	string str;
	UnicodeToANSI(str,strData);

	DWORD dwSize = str.size();
	Write(&dwSize, sizeof(DWORD));
	 
	if(dwSize)
		Write((const void*) str.c_str(), dwSize);
	 
	return (UINT)(dwSize + sizeof(DWORD));
}


UINT CBuffer::ReadString(string& strData)
{
	strData.clear();

	DWORD dwSize;
	Read(&dwSize, sizeof(DWORD));

	if(dwSize == 0)
		return 2;

	//LPTSTR pstr = strData.GetBufferSetLength(dwSize);
	if (m_bSingleRead)
	{
		if (dwSize+m_nReadPos > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}

		//CopyMemory(pstr, m_pBase+m_nReadPos, dwSize);
		string str((const char*)m_pBase+m_nReadPos,dwSize);
 
		strData = str;
 
		m_nReadPos += dwSize;
	}
	else
	{
		if (dwSize > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}
		
		//CopyMemory(pstr, m_pBase, dwSize);

		string str((const char*)m_pBase,dwSize);
 
		strData = str;
 
		Delete(dwSize);
	}

	//strData.ReleaseBuffer();	//**

	return (dwSize+sizeof(DWORD));
//#endif
}

UINT CBuffer::WriteString(string& strData)
{
 
	DWORD wSize = strData.size();
	Write(&wSize, sizeof(DWORD));

	if(wSize)
		Write((const void*) strData.c_str(), wSize);
	
	return (UINT)(wSize+sizeof(DWORD));
 
}
 //////////////////////////////////////////////////////////////////////////

UINT CBuffer::ReadShortString(String& strData)
{
	string szTmp;
	 
	UINT nRet = ReadShortString(szTmp);

	ANSIToUnicode(strData,szTmp);

	return nRet;
}

UINT CBuffer::WriteShortString(String& strData)
{
 
	string str;
	UnicodeToANSI(str,strData);

	WORD dwSize = (WORD)str.size();
	Write(&dwSize, sizeof(WORD));
	 
	if(dwSize)
		Write((const void*) str.c_str(), dwSize);
	 
	return (UINT)(dwSize + sizeof(WORD));
}


UINT CBuffer::ReadShortString(string& strData)
{
	strData.clear();

	WORD dwSize;
	Read(&dwSize, sizeof(WORD));

	if(dwSize == 0)
		return 2;

	//LPTSTR pstr = strData.GetBufferSetLength(dwSize);
	if (m_bSingleRead)
	{
		if (dwSize+m_nReadPos > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}

		//CopyMemory(pstr, m_pBase+m_nReadPos, dwSize);
		string str((const char*)m_pBase+m_nReadPos,dwSize);
 
		strData = str;
 
		m_nReadPos += dwSize;
	}
	else
	{
		if (dwSize > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}
		
		//CopyMemory(pstr, m_pBase, dwSize);

		string str((const char*)m_pBase,dwSize);
 
		strData = str;
 
		Delete(dwSize);
	}

	//strData.ReleaseBuffer();	//**

	return (dwSize+sizeof(WORD));
//#endif
}

UINT CBuffer::WriteShortString(string& strData)
{
 
	WORD wSize = (WORD)strData.size();
	Write(&wSize, sizeof(WORD));

	if(wSize)
		Write((const void*) strData.c_str(), wSize);
	
	return (UINT)(wSize+sizeof(WORD));
 
}
 //////////////////////////////////////////////////////////////////////////

UINT CBuffer::ReadCharString(String& strData)
{
	string szTmp;
	 
	UINT nRet = ReadCharString(szTmp);

	ANSIToUnicode(strData,szTmp);

	return nRet;
} 
UINT CBuffer::ReadCharString(string& strData)
{
	strData.clear();

	char dwSize;
	Read(&dwSize, sizeof(char));

	if(dwSize == 0)
		return 2;

	//LPTSTR pstr = strData.GetBufferSetLength(dwSize);
	if (m_bSingleRead)
	{
		if (dwSize+m_nReadPos > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}

		//CopyMemory(pstr, m_pBase+m_nReadPos, dwSize);
		string str((const char*)m_pBase+m_nReadPos,dwSize);
 
		strData = str;
 
		m_nReadPos += dwSize;
	}
	else
	{
		if (dwSize > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}
		
		//CopyMemory(pstr, m_pBase, dwSize);

		string str((const char*)m_pBase,dwSize);
 
		strData = str;
 
		Delete(dwSize);
	}

	//strData.ReleaseBuffer();	//**

	return (dwSize+sizeof(char));
//#endif
}
int CBuffer::Read7BitEncodedInt()
{
	int index = 0;
	byte num3;
	int num = 0;
	int num2 = 0;
	do
	{
		if (num2 == 0x23)
		{
			ASSERT(0);
			throw 0;
		}
		Read(&num3, sizeof(byte));
	 
		//num3 = stringByte[index++];
		num |= (num3 & 0x7f) << num2;
		num2 += 7;
	}
	while ((num3 & 0x80) != 0);
	return num;
}
UINT CBuffer::ReadUndefineString(string& strData)
{
	strData.clear();

	int dwSize = Read7BitEncodedInt();
	//Read(&dwSize, sizeof(char));

	if(dwSize == 0)
		return 2;

	//LPTSTR pstr = strData.GetBufferSetLength(dwSize);
	if (m_bSingleRead)
	{
		if (dwSize+m_nReadPos > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}

		//CopyMemory(pstr, m_pBase+m_nReadPos, dwSize);
		string str((const char*)m_pBase+m_nReadPos,dwSize);
 
		strData = str;
 
		m_nReadPos += dwSize;
	}
	else
	{
		if (dwSize > m_nDataSize)
		{
			throw DATA_LACK;
			return 0;
		}
		
		//CopyMemory(pstr, m_pBase, dwSize);

		string str((const char*)m_pBase,dwSize);
 
		strData = str;
 
		Delete(dwSize);
	}

	//strData.ReleaseBuffer();	//**

	return (dwSize+sizeof(char));
//#endif
}

void* g_Alloc( long lNum )
{
	//	p = (char*)GlobalAllocPtr(GHND, 8192);
	char* p = new char[lNum];
	memset(p, 0, lNum);
	return p;
}

void g_Free( void* p )
{
	//	GlobalFreePtr(m_pcRecv);
	delete[] p;
}
