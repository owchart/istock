using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace OwLib {
    /// <summary>
    /// 
    /// </summary>
	public static class FormulaProxy {
		/// <summary>
		/// 编译器初始化
		/// </summary>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_Init", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FormulaInit ( );
		/// <summary>
		/// 公式系统释放所有未释放资源,调用程序退出时调用
		/// </summary>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_Uninit", CallingConvention = CallingConvention.Cdecl)]
		public static extern void FormulaUninit ( );
		/// <summary>
		/// 获取公式数据库版本
		/// </summary>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_DB_GetDBVersion", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetDBVersion ( );
		/// <summary>
		/// 获取数据库公式数量
		/// </summary>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_DB_GetFormulaCount", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GetDbFormulaCount ( );
		/// <summary>
		/// 根据公式名从数据库获取公式
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="formula"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_DB_GetFormulaByName", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool GetDbFormulaByName ( int type, string name, ref Formula formula );
		/// <summary>
		/// 根据index从数据库获取公式
		/// </summary>
		/// <param name="index"></param>
		/// <param name="formula"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_FORMULA_DB_GetFormula", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool GetDbFormula ( int index, ref Formula formula );
		/// <summary>
		/// 验证公式源码是否正确
		/// </summary>
		/// <param name="src"></param>
		/// <param name="srcLen"></param>
		/// <param name="paras"></param>
		/// <param name="paracount"></param>
		/// <param name="result"></param>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_TestFormula", CallingConvention = CallingConvention.Cdecl)]
		public static extern void TestFormula ( string src, int srcLen, string[] paras, int paracount, ref TestFormulaResult result );
		/// <summary>
		/// 执行公式计算
		/// </summary>
		/// <param name="openday"></param>
		/// <param name="zq"></param>
		/// <param name="sc"></param>
		/// <param name="code"></param>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <param name="errmsg"></param>
		/// <param name="finaloutput"></param>
		/// <param name="nLen"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_ExecuteFormula", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ExecuteFormula ( int openday, int zq, int sc, string code, FORMULA_TIME begin, FORMULA_TIME end, int type, string name, string errmsg, ref FmFormulaOutput finaloutput, int nLen );
		/// <summary>
		/// 把K线数据传入执行公式计算
		/// </summary>
		/// <param name="openday"></param>
		/// <param name="zq"></param>
		/// <param name="sc"></param>
		/// <param name="code"></param>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <param name="type"></param>
		/// <param name="kline"></param>
		/// <param name="name"></param>
		/// <param name="errmsg"></param>
		/// <param name="finaloutput"></param>
		/// <param name="nLen"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_ExecuteFormulaWithKLine", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ExecuteFormula ( int openday, int zq, int sc, string code, FORMULA_TIME begin, FORMULA_TIME end, int type, Kline[] kline, string name, string errmsg, ref FmFormulaOutput finaloutput, int nLen );

        /// <summary>
        /// 把K线数据和公式传入执行公式计算
        /// </summary>
        /// <param name="openday"></param>
        /// <param name="zq"></param>
        /// <param name="sc"></param>
        /// <param name="code"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="kline"></param>
        /// <param name="f"></param>
        /// <param name="errmsg"></param>
        /// <param name="finaloutput"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        [DllImport("EmfcProxy.dll",EntryPoint = "FM_ExecuteFormulaWithKLineAndFormula",CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ExecuteFormulaWithKLineAndFormula(int openday, int zq, int sc, string code,
                                                                       FORMULA_TIME begin, FORMULA_TIME end,
                                                                       Kline[] kline, Formula f, string errmsg,
                                                                       ref FmFormulaOutput finaloutput, int nLen);
		/// <summary>
		/// 从数据库中删除公式
		/// </summary>
		/// <param name="fid"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_DeleteFormulaFromDB", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool DeleteFormulaFromDB ( int fid );
		/// <summary>
		/// 保存公式到数据库
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		[DllImport("EmfcProxy.dll", EntryPoint = "FM_SaveFormulaToDB", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SaveFormulaToDB ( ref Formula f );
        /// <summary>
        /// 保存公式到公式编辑器内存
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [DllImport("EmfcProxy.dll",EntryPoint = "FM_SaveFormula",CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SaveFormula(ref Formula f);

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="openday"></param>
        /// <param name="zq"></param>
        /// <param name="sc"></param>
        /// <param name="code"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="kline"></param>
        /// <param name="f"></param>
        /// <param name="data"></param>
        /// <param name="datalen"></param>
        /// <param name="finaloutput"></param>
        /// <param name="nlen"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        [DllImport("EmfcProxy.dll", EntryPoint = "FM_ExecuteFormulaWithExtraConst", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ExecuteFormulaWithExtraConst(int openday, int zq, int sc, string code, FORMULA_TIME begin, FORMULA_TIME end, Kline[] kline, Formula f, FM_FORMULA_EXTRA_CONST[] data, int datalen, ref FmFormulaOutput finaloutput, int nlen, string errmsg);

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="f"></param>
        /// <param name="data"></param>
        /// <param name="datalen"></param>
        /// <param name="finaloutput"></param>
        /// <param name="nlen"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        [DllImport("EmfcProxy.dll", EntryPoint = "FM_ExecuteFormulaWithExtraConstWithoutKline", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ExecuteFormulaWithExtraConst(Formula f, FM_FORMULA_EXTRA_CONST[] data, int datalen, ref FmFormulaOutput finaloutput, int nlen, string errmsg);

		/// <summary>
		/// 导出公式
		/// </summary>
		/// <param name="f"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool ExportFormula (Formula f, string filename ) {
			try {
				SerializeFormula serializeFormula = new SerializeFormula();
				serializeFormula.des = f.des;
				serializeFormula.drawtype = f.drawtype;
				serializeFormula.fid = f.fid;
				serializeFormula.flag = Marshal.PtrToStringAnsi(f.flag);
				serializeFormula.help = Marshal.PtrToStringAnsi(f.help);
				serializeFormula.name = f.name;
				serializeFormula.paracount = f.paracount;
				for (int i = 0; i < f.paracount; i++) {
					serializeFormula.para[i].name = f.para[i].name;
					serializeFormula.para[i].maxvalue = f.para[i].maxvalue;
					serializeFormula.para[i].minvalue = f.para[i].minvalue;
					serializeFormula.para[i].defvalue = f.para[i].defvalue;
					serializeFormula.para[i].step = f.para[i].step;
					serializeFormula.para[i].uservalue = f.para[i].uservalue;
				}
				serializeFormula.paramtip = Marshal.PtrToStringAnsi(f.paramtip);
				serializeFormula.password = Marshal.PtrToStringAnsi(f.password);
				serializeFormula.src = Marshal.PtrToStringAnsi(f.src);
				serializeFormula.subtype = f.subtype;
				serializeFormula.type = f.type;
				serializeFormula.y = f.y;
				serializeFormula.y2 = f.y2;
				serializeFormula.y2num = f.y2num;
				serializeFormula.ynum = f.ynum;
				BinaryFormatter serializer = new BinaryFormatter();
				FileStream file = new FileStream(filename, FileMode.Create);
				serializer.Serialize(file, serializeFormula);
				return true;
			} catch (Exception e) {
				LogUtilities.LogMessage(e.Message);
				return false;
			}
		}
		/// <summary>
		/// 导入公式
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static SerializeFormula ImportFormula ( string filename ) {
			try {
				BinaryFormatter deserializer = new BinaryFormatter();
				FileStream file = new FileStream(filename, FileMode.Open);
				return (SerializeFormula)deserializer.Deserialize(file);
			} catch (Exception e) {
				LogUtilities.LogMessage(e.Message);
				return null;
			}
		}
		/// <summary>
		/// 获取全部公式
		/// </summary>
		/// <returns></returns>
		public static IList<Formula> GetAllFormulas ( ) {
			int count = GetDbFormulaCount();
			IList<Formula> result = new List<Formula>();
			for (int i = 0; i < count; i++) {
				Formula formula = new Formula();
				GetDbFormula(i, ref formula);
				result.Add(formula);
			}
			return result;
		}
		/// <summary>
		/// 获取常用公式
		/// </summary>
		/// <returns></returns>
		public static IList<Formula> GetOftenUseFormulas ( ) {
			int count = GetDbFormulaCount();
			IList<Formula> result = new List<Formula>();
			for (int i = 0; i < count; i++) {
				Formula formula = new Formula();
				GetDbFormula(i, ref formula);
				string flag = Marshal.PtrToStringAnsi(formula.flag);
				if (flag.Contains("[C]")) {
					result.Add(formula);
				}
			}
			return result;
		}

		/// <summary>
		/// 获取用户公式
		/// </summary>
		/// <returns></returns>
		public static IList<Formula> GetUserFormulas ( ) {
			int count = GetDbFormulaCount();
			IList<Formula> result = new List<Formula>();
			for (int i = 0; i < count; i++) {
				Formula formula = new Formula();
				GetDbFormula(i, ref formula);
				string flag = Marshal.PtrToStringAnsi(formula.flag);
				if (flag.Contains("[U]")) {
					result.Add(formula);
				}
			}
			return result;
		}
		/// <summary>
		/// 获取系统公式
		/// </summary>
		/// <returns></returns>
		public static IList<Formula> GetSystemFormulas ( ) {
			int count = GetDbFormulaCount();
			IList<Formula> result = new List<Formula>();
			for (int i = 0; i < count; i++) {
				Formula formula = new Formula();
				GetDbFormula(i, ref formula);
				string flag = Marshal.PtrToStringAnsi(formula.flag);
				if (flag.Contains("[S]")) {
					result.Add(formula);
				}
			}
			return result;
		}
	}
}
