using System;
using System.Collections.Generic;
using System.Text;

namespace EmQComm.Formula {
    /// <summary>
    /// 
    /// </summary>
	public class FormulaDict {
        /// <summary>
        /// 类型
        /// </summary>
		public class DrawType {
            /// <summary>
            /// ID
            /// </summary>
			public int Id ;
            /// <summary>
            /// 名称
            /// </summary>
			public string Name ;
		}
        /// <summary>
        /// 
        /// </summary>
		public class FormulaSubType {
            /// <summary>
            /// ID
            /// </summary>
			public int Id ;
            /// <summary>
            /// 名称
            /// </summary>
			public string Name ;
		}
        /// <summary>
        /// 
        /// </summary>
		public class FormulaType {
            /// <summary>
            /// 
            /// </summary>
			public FormulaType ( ) {
				SubTypes = new List<FormulaSubType>();
			}
            /// <summary>
            /// ID
            /// </summary>
			public int Id ;
            /// <summary>
            /// NAME
            /// </summary>
            public string Name ;
            /// <summary>
            /// 
            /// </summary>
            public List<FormulaSubType> SubTypes ;
		}
		/// <summary>
		/// 绘图类型
		/// </summary>
		public List<DrawType> DrawTypes ;
		/// <summary>
		/// 公式类型
		/// </summary>
		public List<FormulaType> FormulaTypes ;
        /// <summary>
        /// 
        /// </summary>
		public FormulaDict ( ) {
			DrawTypes = new List<DrawType>();
			FormulaTypes = new List<FormulaType>();
		}
	}
    /// <summary>
    /// 函数功能
    /// </summary>
	public class FormulaFunctions {
        /// <summary>
        /// 
        /// </summary>
		public class Function {
            /// <summary>
            /// 名称
            /// </summary>
			public string Name ;
            /// <summary>
            /// 描述
            /// </summary>
			public string Description ;
            /// <summary>
            /// 
            /// </summary>
			public string Usage ;
		}
        /// <summary>
        /// 类别
        /// </summary>
		public class Category {
            /// <summary>
            /// 
            /// </summary>
			public Category ( ) {
				Functions = new List<Function>();
			}
            /// <summary>
            /// 名称
            /// </summary>
			public string Name ;
            /// <summary>
            /// 功能
            /// </summary>
			public List<Function> Functions ;
		}
        /// <summary>
        /// 
        /// </summary>
		public List<Category> FunctionCategories ;
        /// <summary>
        /// 
        /// </summary>
		public FormulaFunctions(){
			FunctionCategories = new List<Category>();
		}
	}
}
