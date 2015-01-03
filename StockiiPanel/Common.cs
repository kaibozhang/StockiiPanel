﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Collections;//在C#中使用ArrayList必须引用Collections类
using System.IO;
using System.Reflection;

namespace StockiiPanel
{
    /// <summary>
    /// 公共接口类
    /// </summary>
    class Commons
    {
        public static int colNum = 57;
        public static DataTable classfiDt = new DataTable();

        //版块分类
        enum Board
        {
            Section = 1,
            Industry = 2,
            Up = 3,
            Down = 4
        }

        /// <summary>
        /// 查询股票所有分组信息，包括地区和行业两种
        /// </summary>
        /// <param name="sectionToolStripMenuItem">地区菜单</param>
        /// <param name="industryToolStripMenuItem">行业菜单</param>
        public static void GetStockClassification(ToolStripMenuItem sectionToolStripMenuItem, ToolStripMenuItem industryToolStripMenuItem)
        {
            DataSet ds = JSONHandler.GetClassfication();
            DataTable dt = ds.Tables["stockclassification"];
            classfiDt = dt.Copy();
            DataView dvMenuOptions = new DataView(dt.DefaultView.ToTable(true,new string[]{"areaname"}));//distinct

            foreach (DataRowView rvMain in dvMenuOptions)//循环得到主菜单
            {
                if (rvMain["areaname"].ToString().Equals(""))
                    continue;

                ToolStripMenuItem tsItemParent = new ToolStripMenuItem();

                tsItemParent.Text = rvMain["areaname"].ToString();
                tsItemParent.Name = rvMain["areaname"].ToString();
                sectionToolStripMenuItem.DropDownItems.Add(tsItemParent);
            }
            dvMenuOptions = new DataView(dt.DefaultView.ToTable(true, new string[] { "industryname" }));

            foreach (DataRowView rvMain in dvMenuOptions)//循环得到主菜单
            {
                if (rvMain["industryname"].ToString().Equals(""))
                    continue;

                ToolStripMenuItem tsItemParent = new ToolStripMenuItem();

                tsItemParent.Text = rvMain["industryname"].ToString();
                tsItemParent.Name = rvMain["industryname"].ToString();
                industryToolStripMenuItem.DropDownItems.Add(tsItemParent);
            }
        }

        /// <summary>
        /// 查询股票所有基本信息，包括股票id、股票名称以及上市日期
        /// </summary>
        /// <returns></returns>
        public static DataSet GetStockBasicInfo()
        {
            DataSet ds = JSONHandler.GetStockBasicInfo();

            return ds;
        }

        /// <summary>
        /// 查询股票所有详细信息，包括股票每天的各种指标值
        /// </summary>
        /// <param name="stockid">股票市场中股票的id号</param>
        /// <param name="sortname">排序字段</param>
        /// <param name="asc">升序或者降序排序</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="page">分页查询中的第几页</param>
        /// <param name="pagesize">分页查询中，每页查询的数量</param>
        /// <param name="totalpage">总页数</param>
        /// <returns></returns>
        public static bool GetStockDayInfo(ArrayList stockid, String sortname, bool asc, String startDate, String endDate, int page, int pagesize, out int errorNo, out DataSet ds, out int totalpage)
        {
            bool stop = false;

            ds = JSONHandler.GetStockDayInfo(stockid, sortname, asc, startDate, endDate, page, pagesize, out totalpage, out errorNo);

            var query = (from u in ds.Tables["stockdayinfo"].AsEnumerable()
                         join r in classfiDt.AsEnumerable()
                         on u.Field<string>("stockid") equals r.Field<string>("stockid")
                         select new
                         {
                             stock_id = u.Field<string>("stockid"),
                             stock_name = r.Field<string>("stockname"),
                             created = u.Field<string>("created"),
                             bull_profit = u.Field<string>("bull_profit"),
                             bbi_balance = u.Field<string>("bbi_balance"),
                             num2_sell_price = u.Field<string>("num2_sell_price"),
                             last_deal_amount = u.Field<string>("last_deal_amount"),
                             num3_buy = u.Field<string>("num3_buy"),
                             num3_sell = u.Field<string>("num3_sell"),
                             short_covering = u.Field<string>("short_covering"),
                             bear_stop_losses = u.Field<string>("bear_stop_losses"),
                             current_price = u.Field<string>("current_price"),
                             turnover_ratio = u.Field<string>("turnover_ratio"),
                             num2_buy = u.Field<string>("num2_buy"),
                             cir_of_cap_stock = u.Field<string>("cir_of_cap_stock"),
                             sell = u.Field<string>("sell"),
                             update_date = u.Field<string>("update_date"),
                             min_circulation_value = u.Field<string>("min_circulation_value"),
                             relative_strength_index = u.Field<string>("relative_strength_index"),
                             min = u.Field<string>("min"),
                             pe_ratio = u.Field<string>("pe_ratio"),
                             DaPanWeiBi = u.Field<string>("DaPanWeiBi"),
                             sold_price = u.Field<string>("sold_price"),
                             today_begin_price = u.Field<string>("today_begin_price"),
                             bought_price = u.Field<string>("bought_price"),
                             upordown_per_deal = u.Field<string>("upordown_per_deal"),
                             num1_sell = u.Field<string>("num1_sell"),
                             circulation_value = u.Field<string>("circulation_value"),
                             daily_up_down = u.Field<string>("daily_up_down"),
                             growth_speed = u.Field<string>("growth_speed"),
                             max = u.Field<string>("max"),
                             num1_buy = u.Field<string>("num1_buy"),
                             volume_ratio = u.Field<string>("volume_ratio"),
                             DaPanWeiCha = u.Field<string>("DaPanWeiCha"),
                             buy = u.Field<string>("buy"),
                             num2_sell = u.Field<string>("num2_sell"),
                             num_per_deal = u.Field<string>("num_per_deal"),
                             total_value = u.Field<string>("total_value"),
                             max_circulation_value = u.Field<string>("max_circulation_value"),
                             sb_ratio = u.Field<string>("sb_ratio"),
                             growth_ratio = u.Field<string>("growth_ratio"),
                             avg_price = u.Field<string>("avg_price"),
                             ytd_end_price = u.Field<string>("ytd_end_price"),
                             total_money = u.Field<string>("total_money"),
                             num2_buy_price = u.Field<string>("num2_buy_price"),
                             amplitude_ratio = u.Field<string>("amplitude_ratio"),
                             total_deal_amount = u.Field<string>("total_deal_amount"),
                             current_circulation_value = u.Field<string>("current_circulation_value"),
                             total_stock = u.Field<string>("total_stock"),
                             avg_circulation_value = u.Field<string>("avg_circulation_value"),
                             bull_stop_losses = u.Field<string>("bull_stop_losses"),
                             num1_sell_price = u.Field<string>("num1_sell_price"),
                             turn_per_deal = u.Field<string>("turn_per_deal"),
                             activity = u.Field<string>("activity"),
                             num1_buy_price = u.Field<string>("num1_buy_price"),
                             num3_buy_price = u.Field<string>("num3_buy_price"),
                             num3_sell_price = u.Field<string>("num3_sell_price")
                         });

            DataTable dt = ToDataTable(query.ToList(),"stock_day_info");
            ds.Tables.Remove("stockdayinfo");
            ds.Tables.Add(dt);

            return stop;
        }

        /// <summary>
        /// 导出列表中数据到CSV中
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        public static void ExportDataGridToCSV(DataTable dt)
        {
             SaveFileDialog saveFileDialog =  new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.csv";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "csv files|*.csv";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "";

            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != null) //打开保存文件对话框
            {
                string fileName = saveFileDialog.FileName;//文件名字
                using (StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Default))
                {
                    //Tabel header
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        streamWriter.Write(dt.Columns[i].ColumnName);
                        streamWriter.Write(",");
                    }
                    streamWriter.WriteLine("");
                    //Table body
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            streamWriter.Write(dt.Rows[i][j]);
                            streamWriter.Write(",");
                        }
                        streamWriter.WriteLine("");
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
        }

        /// <summary>
        /// Delete special symbol
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelQuota(string str)
        {
            string result = str;
            string[] strQuota = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "`", ";", "'", ",", ".", "/", ":", "/,", "<", ">", "?" };
            for (int i = 0; i < strQuota.Length; i++)
            {
                if (result.IndexOf(strQuota[i]) > -1)
                    result = result.Replace(strQuota[i], "");
            }
            return result;
        }

        /// <summary>
        /// 从dataGridView中的选中行或所有行并放到一个新表中
        /// </summary>
        /// <param name="isSelect">是否选中行</param>
        /// <returns>datatable</returns>
        public static DataTable StructrueDataTable(DataGridView dataGridView, bool isSelect)
        {
            #region 从dataGridView中选取行并放到一个新表中，然后再绑定到dataGridView中
            DataTable dataTable = new DataTable();

            int length = 0;

            switch (dataGridView.Name)
            {
                case "rawDataGrid":
                    length = colNum;
                    break;
                case "ndayGrid":
                    length = 5;
                    break;
                case "calResultGrid":
                    length = 7;
                    break;
                case "sectionResultGrid":
                    length = 11;
                    break;
                default:
                    break;
            }

            //添加表头
            for (int col = 0; col < length; col++)
            {
                string columnName = dataGridView.Columns[col].HeaderText;
                dataTable.Columns.Add(columnName,dataGridView.Columns[col].ValueType);
            }

            if (isSelect)
            {
                for (int r = dataGridView.SelectedRows.Count - 1; r >= 0; r--)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int c = 0; c < length; c++)
                    {
                        dataRow[c] = dataGridView.SelectedRows[r].Cells[c].Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            else
            {
                for (int r = 0; r < dataGridView.Rows.Count; r++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int c = 0; c < length; c++)
                    {
                        dataRow[c] = dataGridView.Rows[r].Cells[c].Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
            #endregion
        }

        /// <summary>
        /// 判断是不是交易日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool isTradeDay(String date)
        {          
            return true;
        }

        /// <summary>
        /// 查询股票所有详细信息，区分版块
        /// </summary>
        /// <param name="record">版块记录</param>
        /// <param name="sortname">排序字段</param>
        /// <param name="asc">升序或者降序排序</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="page">分页查询中的第几页</param>
        /// <param name="pagesize">分页查询中，每页查询的数量</param>
        /// <param name="totalpage">总页数</param>
        /// <returns></returns>
        public static bool GetStockDayInfoBoard(Dictionary<int, string> record, String sortname, bool asc, String startDate, String endDate, int page, int pagesize, out int errorNo, out DataSet ds, out int totalpage)
        {
            string name = record.Values.First();
            ArrayList stocks = new ArrayList();
            DataRow[] rows;
            switch (record.Keys.First())
            {
                case (int)Board.Section:
                    rows = classfiDt.Select("areaname = '"+name+"'");
                    foreach (DataRow row in rows)
                    {
                        stocks.Add(row["stockid"]);
                    }
                    break;
                case (int)Board.Industry:
                    rows = classfiDt.Select("industryname = '" + name + "'");
                    foreach (DataRow row in rows)
                    {
                        stocks.Add(row["stockid"]);
                    }
                    break;
                default :
                    break;
            }

            return GetStockDayInfo(stocks, sortname, asc, startDate, endDate, page, pagesize,out errorNo,out ds,out totalpage);
        }

        /// <summary>
        /// 查询股票N日和
        /// </summary>
        /// <param name="stockid">股票市场中股票的id号</param>
        /// <param name="type">类型，1日和，2周和，3月和</param>
        /// <param name="num">参数</param>
        /// <param name="sumname">求和的指标名称</param>
        /// <param name="sumtype">求和类型</param>
        /// <param name="sortname">排序字段</param>
        /// <param name="asc">升序或者降序排序</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="page">分页查询中的第几页</param>
        /// <param name="pagesize">分页查询中，每页查询的数量</param>
        /// <param name="errorNo">错误码</param>
        /// <param name="ds">结果集</param>
        /// <param name="totalpage">总页数</param>
        /// <returns></returns>
        public static bool GetNDaysSum(ArrayList stockid, int type, int num, String sumname, String sumtype, String sortname, bool asc, String startDate, String endDate, int page, int pagesize, out int errorNo, out DataSet ds, out int totalpage)
        {
            bool stop = false;
            totalpage = 1;
            errorNo = 0;
            ds = new DataSet();

            return stop;
        }

        /// <summary>
        /// 查询股票N日和,区分版块
        /// </summary>
        /// <param name="record">版块记录</param>
        /// <param name="type">类型，1日和，2周和，3月和</param>
        /// <param name="num">参数</param>
        /// <param name="sumname">求和的指标名称</param>
        /// <param name="sumtype">求和类型</param>
        /// <param name="sortname">排序字段</param>
        /// <param name="asc">升序或者降序排序</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="page">分页查询中的第几页</param>
        /// <param name="pagesize">分页查询中，每页查询的数量</param>
        /// <param name="errorNo">错误码</param>
        /// <param name="ds">结果集</param>
        /// <param name="totalpage">总页数</param>
        /// <returns></returns>
        public static bool GetNDaysSumBoard(Dictionary<int, string> record, int type, int num, String sumname, String sumtype, String sortname, bool asc, String startDate, String endDate, int page, int pagesize, out int errorNo, out DataSet ds, out int totalpage)
        {
            bool stop = false;
            totalpage = 1;
            errorNo = 0;
            ds = new DataSet();
            return stop;
        }

        /// <summary>
        /// 查询股票的自定义计算结果
        /// </summary>
        /// <param name="stockid">股票市场中股票的id号</param>
        /// <param name="min">筛选最小值</param>
        /// <param name="max">筛选最大值</param>
        /// <param name="optname">自定义计算的指标名</param>
        /// <param name="opt">自定义计算方式</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="errorNo">错误码</param>
        /// <param name="ds">结果集</param>
        /// <returns></returns>
        public static bool GetStockDaysDiff(ArrayList stockid, double min, double max, String optname, String opt, String startDate, String endDate, out int errorNo, out DataSet ds)
        {
            bool stop = false;
            errorNo = 0;
            ds = new DataSet();
            return stop;
        }

        /// <summary>
        /// 查询股票的自定义计算结果，区分版块
        /// </summary>
        /// <param name="record">版块记录</param>
        /// <param name="min">筛选最小值</param>
        /// <param name="max">筛选最大值</param>
        /// <param name="optname">自定义计算的指标名</param>
        /// <param name="opt">自定义计算方式</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="errorNo">错误码</param>
        /// <param name="ds">结果集</param>
        /// <returns></returns>
        public static bool GetStockDaysDiffBoard(Dictionary<int, string> record, double min, double max, String optname, String opt, String startDate, String endDate, out int errorNo,out DataSet ds)
        {
            bool stop = false;

            errorNo = 0;
            ds = new DataSet();

            return stop;
        }

        /// <summary>
        /// 查询股票跨区的信息
        /// </summary>
        /// <param name="weight">跨区的权重值</param>
        /// <param name="optname">自定义计算的指标名</param>
        /// <param name="opt">自定义计算方式</param>
        /// <param name="startDate">查询起始时间点</param>
        /// <param name="endDate">查询结束时间点</param>
        /// <param name="errorNo">错误码</param>
        /// <param name="ds">结果集</param>
        /// <returns></returns>
        public static bool GetCrossInfoCmd(double weight, String optname, String startDate, String endDate, out int errorNo,out DataSet ds)
        {
            bool stop = false;
            errorNo = -1;
            ds = JSONHandler.GetCrossInfo(2, "avg_price", "2012-12-03", "2013-12-2");

            var query = (from u in ds.Tables["crossinfo"].AsEnumerable()
                         join r in classfiDt.AsEnumerable()
                         on u.Field<string>("stockid") equals r.Field<string>("stockid")
                         select new
                         {
                             stock_id = u.Field<string>("stockid"),
                             stock_name = r.Field<string>("stockname"),
                             start_date = u.Field<string>("startdate"),
                             end_date = u.Field<string>("enddate"),
                             start_value = u.Field<string>("startvalue"),
                             end_value = u.Field<string>("endvalue"),
                             start_list_date = u.Field<string>("startlistdate"),
                             end_list_date = u.Field<string>("endlistdate"),
                             cross_type = u.Field<string>("crosstype"),
                             avg = u.Field<string>("avg"),
                             difference = u.Field<string>("difference"),
                         });

            DataTable dt = ToDataTable(query.ToList(), "cross_info");
            ds.Tables.Remove("crossinfo");
            ds.Tables.Add(dt);

            //Console.WriteLine("ds: {0}")
            return stop;

        }

        /// <summary>
        /// list转化成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> varlist, string name)
        {
            DataTable dtReturn = new DataTable(name);
            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;
            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                             == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// 拼接功能
        /// </summary>
        /// <param name="dataGridView">要拼接的</param>
        /// <param name="isSelect">是否选中行</param>
        public static DataGridView Combine(DataGridView dataGridView, DataGridView combineGridView,bool isSelect)
        {
            DataGridView result = new DataGridView();

            if (isSelect)
            {
                DataTable tb1 = (DataTable)dataGridView.DataSource;
                DataTable tb2 = new DataTable();
                tb2 = tb1.Copy();

                for (int r = dataGridView.SelectedRows.Count - 1; r >= 0; r--)
                {
                    DataRow dataRow = tb2.NewRow();
                    for (int c = 0; c < dataGridView.Columns.Count; c++)
                    {
                        dataRow[c] = dataGridView.SelectedRows[r].Cells[c].Value;
                    }
                    tb2.Rows.Add(dataRow);
                }
                result.DataSource = tb2;
                if (combineGridView.RowCount > 0)
                {
                    for (int i = 0; i < combineGridView.Columns.Count; ++i)
                        tb2.Columns.Add(combineGridView.Columns[i].Name, combineGridView.Columns[i].ValueType);
                    foreach (DataRow dr in combineGridView.Rows)
                   {
                       foreach (DataRow re in tb2.Rows)
                       {
                           if (re[0].ToString() == dr[0].ToString())//相同ID则拼接
                           {
                           }
                       }
                   }
                }
                
            }
            else
            {
                DataTable tb1 = (DataTable)dataGridView.DataSource;
                DataTable tb2 = new DataTable();
                tb2 = tb1.Copy();

                for (int r = 0; r < dataGridView.Rows.Count; r++)
                {
                    DataRow dataRow = tb2.NewRow();
                    for (int c = 0; c < dataGridView.Columns.Count; c++)
                    {
                        dataRow[c] = dataGridView.Rows[r].Cells[c].Value;
                    }
                    tb2.Rows.Add(dataRow);
                }
                result.DataSource = tb2;
                if (combineGridView.RowCount > 0)
                {
                   
                }
                
            }

            return result;
        }
    }
}
