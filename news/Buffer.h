// Buffer.h: interface for the CBuffer class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BUFFER_H__829F6693_AC4D_11D2_8C37_00600877E420__INCLUDED_)
#define AFX_BUFFER_H__829F6693_AC4D_11D2_8C37_00600877E420__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#include <string>
#include "Str.h"
using namespace std;

#define		DATA_LACK		-1
#define  PAGE_SIZE  1024 * 8
 
#ifdef EXPORT_DLL_TYPE
#define EXPORT_DLL __declspec(dllexport)
#else
#define EXPORT_DLL __declspec(dllimport)
#endif

typedef unsigned char       BYTE;
typedef BYTE            *PBYTE;
//#ifdef EXPORT_DATARECVDLL
//#define DLL_EXPORTDATA __declspec(dllexport)
//#else if
//#define DLL_EXPORTDATA __declspec(dllimport)
//#endif
extern void* g_Alloc(long lNum);

extern void g_Free(void* p);

class CBuffer  
{
// Attributes
protected:
	PBYTE		m_pBase;
	UINT		m_nDataSize;
	UINT		m_nSize;

	UINT		m_nInitSize;

	bool		m_bSustainSize;

	static DWORD		m_dwPageSize;

public:
	bool		m_bSingleRead;
	UINT		m_nReadPos;

// Methods
protected:
	UINT DeAllocateBuffer(UINT nRequestedSize);
	UINT GetMemSize();
public:
	CBuffer();
	~CBuffer();

	void ClearBuffer();
	void Initialize(UINT nInitsize, bool bSustain);

	UINT Delete(UINT nSize);
	UINT Read(void* pData, UINT nSize);
	UINT Write(const void* pData, UINT nSize);
	UINT Write(String& strData);
	UINT Insert(const void* pData, UINT nSize);
	UINT Insert(String& strData);
	UINT DeleteEnd(UINT nSize);

	UINT SkipData(int nSize);

	void Copy(CBuffer& buffer);	
	PBYTE GetBuffer(UINT nPos=0);
	UINT GetBufferLen();
	PBYTE GetBuffer(UINT nPos=0)const;
	UINT GetBufferLen()const;


	void FileWrite(const String& strFileName);
	void FileRead(const String& strFileName);

	void FileWrite(const string& strFileName);
	void FileRead(const string& strFileName);

	const CBuffer& operator+(CBuffer& buff);
	UINT ReAllocateBuffer(UINT nRequestedSize);

	void WriteChar(char);
	char ReadChar();
	void WriteShort(short);
	short ReadShort();
	void WriteInt(int);
	int ReadInt();
	void WriteFloat(float);
	float ReadFloat();
	void WriteDouble(double);
	double ReadDouble();
	void WriteLong(INT64);
	INT64 ReadLong();

	UINT ReadString(String&);
	UINT ReadString(string& strData);
	UINT WriteString(String&);
	UINT WriteString(string& strData);

	UINT ReadShortString(String& strData);
	UINT ReadShortString(string& strData);
	UINT WriteShortString(String& strData);
	UINT WriteShortString(string& strData); 

	UINT ReadCharString(String& strData);
	UINT ReadCharString(string& strData);
	UINT ReadUndefineString(string& strData);
	int Read7BitEncodedInt();
};

#endif // !defined(AFX_BUFFER_H__829F6693_AC4D_11D2_8C37_00600877E420__INCLUDED_)
